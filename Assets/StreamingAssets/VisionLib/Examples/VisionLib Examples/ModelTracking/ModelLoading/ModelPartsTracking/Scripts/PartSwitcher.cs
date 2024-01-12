using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Visometry.VisionLib.SDK.Core.Details;
using Visometry.VisionLib.SDK.Core;
using Visometry.VisionLib.SDK.Core.API;

namespace Visometry.VisionLib.SDK.Examples
{
    /// <summary>
    /// This is an example script that shows how parts can be switched on and off.
    /// Here we assume, that the parts have the same name as in the defined .vl file.
    ///
    /// Please checkout the MutableModelTracking folder in
    /// StreamingAssets/VisionLib/Example/ModelTracking and the documentation on
    /// UsingVisionLib/TrackingEssentials/DynamicModelTracking.
    /// </summary>
    /// @ingroup Examples
    [AddComponentMenu("VisionLib/Examples/Part Switcher")]
    public class PartSwitcher : MonoBehaviour
    {
        // Names of the objects defined in the vl file
        public string[] objects;

        // ModelTracker used in this tracking scenario
        [FormerlySerializedAs("modelTrackerBehaviour")]
        public ModelTracker modelTracker;

        /// Enable for recognition of update ( this will only work on significant changes in the
        /// tracking target!)
        public bool autoInstructionUpdate = false;

        // GameObject containing the objects, which should be showed, as children
        public GameObject father;

        // Material for pieces, which should be added in the current step
        public Material guidedMat;

        // Material for pieces, which have already been added
        public Material placedMat;

        // Print log messages about model state
        public bool verbose = false;

        private int curStep = 0;
        private int trackingCounter = 0;

        private bool showStepInformation = false;

        private async Task PrepareStepAsync(int step)
        {
            if (step > objects.Length)
            {
                return;
            }

            for (int i = 0; i < objects.Length; i++)
            {
                bool enabledState = false; // enable object for tracking
                bool showObject = false; // show object in GUI
                bool showGuide = false; // show object in GUI with guide material or placed material
                if (i < step)
                {
                    // set
                    enabledState = true;
                    showObject = true;
                }
                else if (i == step)
                {
                    // show instructions
                    enabledState = false;
                    showObject = true;
                    showGuide = true;
                }

                // We enable each object in the visionlib corresponding to the building state by
                // setting the enabled attribute
                modelTracker.SetModelPropertyEnabled(objects[i], enabledState);

                // We set the material and the visiblity for the user
                GameObject Go = this.father.transform.GetChild(i).gameObject;
                Go.transform.GetChild(0).gameObject.GetComponent<Renderer>().material =
                    showGuide ? guidedMat : placedMat;
                Go.SetActive(showObject);
            }

            // just for info purposes, retreive the current model properties (not needed)
            var result = await modelTracker.GetModelPropertiesAsync();
            if (!this.StillAliveAndEnabled())
            {
                return;
            }
            LogModelProperties(result.info);
        }

        /// <summary>
        /// Enables all models for the given step.
        /// </summary>
        /// <remarks> This function will be performed asynchronously.</remarks>
        private void PrepareStep(int step)
        {
            TrackingManager.CatchCommandErrors(PrepareStepAsync(step), this);
        }

        public void NextStep()
        {
            curStep++;
            if (curStep > objects.Length)
                curStep = 0;
            PrepareStep(curStep);
        }

        void OnTrackerInitialized()
        {
            // We start everything, when the tracker has been initialized.
            curStep = 0;
            PrepareStep(curStep);
            showStepInformation = true;
        }

        void AutoUpdateInstructions(TrackingState states)
        {
            if (!autoInstructionUpdate)
                return;

            // We start everything, when the tracker has been initialized.
            foreach (TrackingState.TrackingObject trackingObject in states.objects)
            {
                // Count how many frames the object has been tracked in a row
                if (trackingObject.state == "tracked")
                {
                    trackingCounter++;
                }
                else
                {
                    // If tracking is lost or critical, reset the tracking counter
                    trackingCounter = 0;
                }

                // If object has been tracked for 60 frames, go to the next
                // instruction step
                if (trackingCounter == 60)
                {
                    trackingCounter = 0;
                    if (curStep < objects.Length)
                    {
                        NextStep();
                    }
                }
            }
        }

        void LogModelProperties(ModelProperties[] properties)
        {
            if (!this.verbose)
            {
                return;
            }

            LogHelper.LogInfo("Got model properties: " + properties.Length);
            for (var i = 0; i < properties.Length; i++)
            {
                LogHelper.LogInfo(
                    "Model:" + i + " with name:" + properties[i].name + " is enabled:" +
                    properties[i].enabled);
            }
        }

        void OnEnable()
        {
            TrackingManager.OnTrackerInitialized += OnTrackerInitialized;
            TrackingManager.OnTrackingStates += AutoUpdateInstructions;
        }

        void OnDisable()
        {
            TrackingManager.OnTrackingStates -= AutoUpdateInstructions;
            TrackingManager.OnTrackerInitialized -= OnTrackerInitialized;
        }

        void OnGUI()
        {
            if (showStepInformation)
            {
                if (curStep >= objects.Length)
                {
                    GUI.Label(new Rect(Screen.width / 2f - 30f, 15f, 300, 30), "Finished.");
                }
                else
                {
                    GUI.Label(
                        new Rect(Screen.width / 2f - 90f, 15f, 300, 30),
                        "Step:" + (curStep + 1) + "/" + (objects.Length + 1) +
                        ". Press Next to Continue!");
                }
            }
        }
    }
}
