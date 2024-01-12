using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosIkaros
{
    public class CameraDebug
    {
        public static bool enableDebug = false;
        public static void Log(string info)
        {
            if(enableDebug)
                Debug.Log(info);
        }
    }
    public class AARplugin : MonoBehaviour
    {
        public enum FrameTransferMode
        {
            //
            // Summary:
            //     Use GPU(OpenGL) to convert frame (recommended mode)
            GPU_Mode = 0,
            //
            // Summary:
            //     Use GPU and CPU to convert frame (not as efficient as GPU_Mode, do not use this mode unless you failed to launch GPU_Mode )
            Heterogeneous_Mode = 1,
        }
        public const int FRAME_FORMAT_DEFAULT = 1;
        public const int FRAME_FORMAT_YUYV = 0;
        public const int FRAME_FORMAT_MJPEG = 1;
        public FrameTransferMode frameTransferMode = FrameTransferMode.GPU_Mode;
        public int CameraInitializationTime = 2000;//milli seconds
        public USBCamera[] cameraScreens = new USBCamera[4];
        public Texture2D[] rawTextures = new Texture2D[4];

        /// <summary>
        /// receive feedback from plugin
        /// </summary>
        public Text messageText;

        /// <summary>
        /// android object
        /// </summary>
        public AndroidJavaObject androidJavaObject;

        /// <summary>
        /// In Promiscuous mode, the plugin will recognize all USB devices as a USB camera. Do not enable this mode unless you failed to connect to your camera.
        /// </summary>
        public bool PromiscuousModeOn = false;
        public bool screenNeverSleep = false;
        public bool enableDebug = false;
        public bool enableNativeToast = false;
        void Start()
        {
            CameraDebug.enableDebug = enableDebug;
            if(screenNeverSleep)
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if UNITY_EDITOR && UNITY_ANDROID
            CameraDebug.Log("Please switch the platform to standalone if you want to preview on the Editor");
#endif
#if UNITY_ANDROID
            InitPlugin();
#endif
        }

        /// <summary>
        /// initialize camera format and USBCamera Manager
        /// </summary>
        public void InitPlugin()
        {
            androidJavaObject = new AndroidJavaObject("com.chaosikaros.unityplugin.Plugin");
            SetCameraInitializationTime(CameraInitializationTime);
            androidJavaObject.Call<bool>("SetToast", enableNativeToast);
            if (PromiscuousModeOn)
            {
                CameraDebug.Log("Enable Promiscuous Mode");
                SetPromiscuousMode(true);
            }
            OnBtnClick();
            //Unsupported resolution will cause error
            int connectedCameras = 0;
            for (int i = 0; i < cameraScreens.Length; i++)
            {
                if (cameraScreens[i] != null)
                {
                    connectedCameras++;
                    CameraDebug.Log("Create camera foramt " + i.ToString() + " : " + CreateUSBCameraFormat(
                    i, FRAME_FORMAT_DEFAULT,
                    cameraScreens[i].width, cameraScreens[i].height,
                    1, cameraScreens[i].FPS,
                    (float)1 / cameraScreens.Length));
                }
            }
            CameraDebug.Log("Max cameras: " + androidJavaObject.Call<int>("SetMaxDeviceCount", connectedCameras));
            //CameraDebug.Log("Create USBCamera Manager: " + CreateUSBCameraManagerWithSpecificDevices("usb/001/003", "usb/001/004"));
            CameraDebug.Log("Create USBCamera Manager: " + CreateUSBCameraManagerWithRandomDevices());
            for (int i = 0; i < cameraScreens.Length; i++)
            {
                if (cameraScreens[i] != null)
                {
                    cameraScreens[i].InitSupportedFormat();
                    cameraScreens[i].InitScreen();
                }
            }
        }

        /// <summary>
        /// In Promiscuous mode, the plugin will recognize all USB devices as a USB camera. Do not enable this mode unless you failed to connect to your camera.
        /// </summary>
        public bool SetPromiscuousMode(bool input)
        {
            return androidJavaObject.Call<bool>("SetPromiscuousMode", input);
        }

        /// <summary>
        /// Set the Camera Initialization Time in milli seconds
        /// </summary>
        public int SetCameraInitializationTime(int input)
        {
            return androidJavaObject.Call<int>("SetCameraInitializationTime", input);
        }

        /// <summary>
        /// create USB Camera Format with specific id (0-3) frameFormat(0-1) bandwidthFactor (float 0-1)
        /// </summary>
        public bool CreateUSBCameraFormat(int deviceID, int i_frameFormat = FRAME_FORMAT_DEFAULT, int i_width = 640,
            int i_height = 480, int i_minFPS = 1, int i_maxFPS = 30, float i_bandwidthFactor = 1.0f)
        {
            return androidJavaObject.Call<bool>("CreateFormat", deviceID, i_frameFormat, i_width,
                i_height, i_minFPS, i_maxFPS, i_bandwidthFactor);
        }


        /// <summary>
        /// create USB Camera Manager with specific device names
        /// multiple cameras is not recommended for Android device with USB2.0 (due to the limit of transimision bandwidth)
        /// </summary>
        public bool CreateUSBCameraManagerWithSpecificDevices(string deviceName1, string deviceName2 = "usb/001/004",
            string deviceName3 = "usb/003/005", string deviceName4 = "usb/003/006")
        {
            return androidJavaObject.Call<bool>("CreateUSBCamera", deviceName1, deviceName2, deviceName3, deviceName4, true);
        }

        /// <summary>
        /// create USB Camera Manager with random devices
        /// multiple cameras is not recommended for Android device with USB2.0 (due to the limit of transimision bandwidth)
        /// </summary>
        public bool CreateUSBCameraManagerWithRandomDevices(string deviceName1 = "usb/001/002", string deviceName2 = "usb/001/004",
            string deviceName3 = "usb/003/005", string deviceName4 = "usb/003/006")
        {
            return androidJavaObject.Call<bool>("CreateUSBCamera", deviceName1, deviceName2, deviceName3, deviceName4, false);
        }

        /// <summary>
        /// get the Texture2D of camera frame with specific device ID
        /// </summary>
        public Texture2D GetFrame(int deviceID)
        {
            if (androidJavaObject.Call<bool>("getCameraState", 0, deviceID))
            {
                int textureId = 0;
                if (frameTransferMode == FrameTransferMode.GPU_Mode)
                    textureId = androidJavaObject.Call<int>("getTextureID", deviceID);
                else
                    textureId = androidJavaObject.Call<int>("getTextureIDByRS", deviceID);
                if (textureId != 0)
                {
                    CameraDebug.Log("create external texture");
                    if (rawTextures[deviceID] == null || rawTextures[deviceID].width != cameraScreens[deviceID].width ||
                        rawTextures[deviceID].height != cameraScreens[deviceID].height)
                    {
                        rawTextures[deviceID] = null;
                        rawTextures[deviceID] = Texture2D.CreateExternalTexture(cameraScreens[deviceID].width,
                            cameraScreens[deviceID].height, TextureFormat.RGB565, false, false, (IntPtr)textureId);
                        rawTextures[deviceID].wrapMode = TextureWrapMode.Clamp;
                        rawTextures[deviceID].filterMode = FilterMode.Bilinear;
                    }
                    else
                    {
                        rawTextures[deviceID].UpdateExternalTexture((IntPtr)textureId);
                    }
                }
                return rawTextures[deviceID];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// refresh camera states with specific device ID
        /// </summary>
        public void RefreshCameraStates(int deviceID)
        {
            if(androidJavaObject != null)
                cameraScreens[deviceID].playing = androidJavaObject.Call<bool>("getCameraState", 0, deviceID);
        }

        /// <summary>
        /// test API in Android aar
        /// </summary>
        public void OnBtnClick()
        {
            if (androidJavaObject == null)
                return;
            //call function in Android aar
            bool success = androidJavaObject.Call<bool>("SendMessageToUnity", "Echo test for plugin");
            if (true == success)
            {
                CameraDebug.Log("Success");
            }
        }

        /// <summary>
        /// receive msg from Android aar
        /// </summary>
        /// <param name="content"></param>
        public void FromAndroid(string content)
        {
            messageText.text = content;
            if (content.StartsWith("SupportedSizeList;"))
            {
                int tempID = int.Parse(content.Split(';')[1]);
                cameraScreens[tempID].InitSupportedSizes(content);
            }
            CameraDebug.Log("From Android: " + content);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}