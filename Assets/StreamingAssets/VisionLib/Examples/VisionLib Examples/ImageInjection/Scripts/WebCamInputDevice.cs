using UnityEngine;
using Visometry.VisionLib.SDK.Core.Details;
using Visometry.VisionLib.SDK.Core;
using Visometry.VisionLib.SDK.Core.API.Native;

namespace Visometry.VisionLib.SDK.Examples
{
    /**
     *  @ingroup Examples
     */
    [AddComponentMenu("VisionLib/Examples/WebCam Input Device")]
    public class WebCamInputDevice : MonoBehaviour
    {
        public int width = 640;
        public int height = 480;
        public int fps = 60;

        private WebCamTexture cameraImage;
        private byte[] rawByteData;

        public static class Details
        {
            public static ExtrinsicData UnityTransformToVLExtrinsic(Transform transform)
            {
                Quaternion r = new Quaternion(0, 0, 0, 1);
                Vector4 t = new Vector4(0, 0, 0, 0);
                var matrix_CameraFromUnityWorld = Matrix4x4.Inverse(
                    Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, -1)));
                var matrix_CameraFromWorld = matrix_CameraFromUnityWorld * CameraHelper.flipXY;
                CameraHelper.WorldToCameraMatrixToVLPose(matrix_CameraFromWorld, out t, out r);
                return new ExtrinsicData(r, t);
            }

            public static IntrinsicData GenerateImageInjectionDefaultIntrinsic(int width, int height)
            {
                double fx = 0.7;
                double fy = 0.7 * width / height;
                double cx = 0.5;
                double cy = 0.5;
                double skew = 0.0;

                return new IntrinsicData(width, height, fx, fy, cx, cy, skew);
            }
        }
        
        private void Start()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
            {
                LogHelper.LogError("No camera detected");
                return;
            }

            foreach (WebCamDevice device in devices)
            {
                // if (!device.isFrontFacing)
                {
                    this.cameraImage =
                        new WebCamTexture(device.name, width, height, fps); //, 800, 400);
                    break;
                }
            }

            if (this.cameraImage == null)
            {
                LogHelper.LogError("Unable to find a valid camera");
                return;
            }

            this.cameraImage.Play();
        }

        public Frame GetFrameFromCamera()
        {
            Frame frame = new Frame();
            frame.image = Image.CreateFromTexture(this.cameraImage, ref this.rawByteData);
            frame.intrinsicData = Details.GenerateImageInjectionDefaultIntrinsic(
                this.cameraImage.width,
                this.cameraImage.height);
            frame.extrinsicData = Details.UnityTransformToVLExtrinsic(this.transform);
            return frame;
        }

        public void Update()
        {
            if (!TrackingManager.Instance.GetTrackerInitialized())
            {
                return;
            }

            SynchronousTrackingManager.Instance.Push(GetFrameFromCamera());
        }

        public void OnDestroy()
        {
            if (this.cameraImage)
            {
                this.cameraImage.Stop();
            }
        }
    }
}


