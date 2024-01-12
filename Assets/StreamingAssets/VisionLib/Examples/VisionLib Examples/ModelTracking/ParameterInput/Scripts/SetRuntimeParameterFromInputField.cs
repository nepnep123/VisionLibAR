using UnityEngine;
using UnityEngine.UI;
using Visometry.VisionLib.SDK.Core.Details;
using Visometry.VisionLib.SDK.Core;

namespace Visometry.VisionLib.SDK.Examples
{
    /// <summary>
    ///  Sets the value of a runtime parameter to the value of a InputField.
    /// </summary>
    /// <remarks>
    ///  The assignment happens inside the Apply function.
    /// </remarks>
    /// @ingroup Examples
    [AddComponentMenu("VisionLib/Examples/Set RuntimeParameter From InputField")]
    public class SetRuntimeParameterFromInputField : MonoBehaviour
    {
        /// <summary>
        ///  The text of this InputField will be used as value for a parameter.
        /// </summary>
        public InputField inputField;

        /// <summary>
        ///  The parameter whose value will be set to the InputFields text.
        /// </summary>
        public RuntimeParameter runtimeParameter;

        /// <summary>
        ///  Optional text object for showing the set value.
        /// </summary>
        /// <remarks>
        ///  The text will be set to the InputFields text directly after calling
        ///  the Apply function.
        /// </remarks>
        public Text text;

        private void Awake()
        {
            if (this.inputField == null)
            {
                this.inputField = GetComponent<InputField>();
            }
        }

        /// <summary>
        ///  Uses the text of the inputField to set the value of a runtime
        ///  parameter.
        /// </summary>
        public void Apply()
        {
            if (this.inputField == null)
            {
                LogHelper.LogWarning("'InputField' is null", this);
                return;
            }

            if (this.runtimeParameter == null)
            {
                LogHelper.LogWarning("'RuntimeParameter' is null", this);
                return;
            }

            this.runtimeParameter.SetValue(this.inputField.text);

            if (this.text != null)
            {
                this.text.text = this.inputField.text;
            }
        }
    }
}
