using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChaosIkaros;

public class USBCamera : MonoBehaviour
{
    public Album album;
    public bool playing = false;
    public bool stopping = false;
    public AARplugin plugin;
    public int deviceID = 0;
    public int width = 640;
    public int height = 480;
    public int FPS = 30;
    public GameObject screen;
    public GameObject screenUI;
    public RawImage screenImage;
    public MeshRenderer screenRender;
    public GameObject detailsPanel;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public bool onlyRenderOnUI = false;
    public bool isUSBCamera = false;
    public bool autoFit = true;
    public bool enableFPSDisplay = false;
    public int frameID = 0;
    private WebCamTexture webCamTexture;
    private Vector3 tempScreenSize;
    public List<string> supportedSizes = new List<string>();
    private List<string> supportedFormats = new List<string>() {"YUYV", "MJPEG"};
    public Dropdown sizeSelector;
    public Dropdown frameFormatSelector;
    public string currentSize = "";
    public string currentFormat = "";
    public Texture2D tempTexture2D;
    public Text FPS_text;
    public bool isNotAndroid = false;
    public bool enableFrameFormatAdjustment = false;
    public Vector2 textureSize = new Vector2(1, 1);
    private Vector2 orginalSize = new Vector2(1, 1);
    private Vector2 flipSize = new Vector2(-1, -1);
    // Use this for initialization
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(screen.transform.localScale != Vector3.zero)
            tempScreenSize = screen.transform.localScale;
        if (onlyRenderOnUI)
        {   
            screen.transform.localScale = Vector3.zero;
        }
        else
            screen.transform.localScale = tempScreenSize;
    }
#endif
    void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        isNotAndroid = true;
        isUSBCamera = false;
        InitScreen();
#endif
        sizeSelector.onValueChanged.AddListener(OnSizeChanged);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderChange);
        contrastSlider.onValueChanged.AddListener(OnContrastSliderChange);
        if (onlyRenderOnUI)
            screen.transform.localScale = Vector3.zero;
        else
            tempScreenSize = screen.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        plugin.RefreshCameraStates(deviceID);
#endif
        if (!onlyRenderOnUI && Input.GetMouseButton(0))
        {
            screenRender.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * 3.0f, -Input.GetAxis("Mouse X") * 3.0f, 0), Space.World);
        }
    }
    public void InitScreen()
    {
        if (plugin.frameTransferMode == AARplugin.FrameTransferMode.GPU_Mode)
            textureSize = orginalSize;
        else
            textureSize = flipSize;
        FPS_text.enabled = enableFPSDisplay;
        if (tempTexture2D == null)
        {
            tempTexture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
        }
        else
        {
            tempTexture2D.Resize(width, height, TextureFormat.RGBA32, false);
            tempTexture2D.Apply();
        }
        if (autoFit)
        {
            if (!onlyRenderOnUI)
                AutoFitScreen(screen, 1.0f);
            AutoFitScreen(screenUI, 5.0f);
        }
#if  UNITY_EDITOR || UNITY_STANDALONE_WIN
        StopCoroutine("InitCamera");
        StartCoroutine("InitCamera");
#endif
#if UNITY_ANDROID
        StopCoroutine("InitCameraForAndroid");
        StartCoroutine("InitCameraForAndroid");
#endif
    }
    public void InitSupportedFormat()
    {
        if (enableFrameFormatAdjustment)
        {
            frameFormatSelector.onValueChanged.AddListener(OnFrameFormatChanged);
            frameFormatSelector.ClearOptions();
            frameFormatSelector.AddOptions(supportedFormats);
            frameFormatSelector.value = AARplugin.FRAME_FORMAT_DEFAULT;
        }
    }
    public void InitSupportedSizes(string rawConfigs)
    {
        List<string> options = rawConfigs.Split(';').ToList();
        supportedSizes.Clear();
        for (int i = 2; i < options.Count; i++)
        {
            if (options[i] != "")
                supportedSizes.Add(options[i]);
        }
        sizeSelector.ClearOptions();
        sizeSelector.AddOptions(supportedSizes);
    }
    public void GetSupportedSize()
    {
        RequireSupportedSize();
    }
    public bool RequireSupportedSize()
    {
        if (isNotAndroid)
        {
            CameraDebug.Log("Dynamic resolution is only supported on Android");
            return false;
        }
        return plugin.androidJavaObject.Call<bool>("RequireSupportedSizes", deviceID);
    }
    public void OnSizeChanged(int value)
    {
        if (isNotAndroid)
        {
            CameraDebug.Log("Dynamic resolution is only supported on Android");
            return;
        }
        currentSize = supportedSizes[value];
        string[] temp = currentSize.Split('-');
        plugin.androidJavaObject.Call<bool>("ChangeSizes", deviceID, int.Parse(temp[0]), int.Parse(temp[1]), false);
        if (int.Parse(temp[0]) != width || int.Parse(temp[1]) != height)
        {
            CameraDebug.Log("Size Changed: " + currentSize);
            width = int.Parse(temp[0]);
            height = int.Parse(temp[1]);
            InitScreen();
        }
    }
    public void OnFrameFormatChanged(int value)
    {
        if (isNotAndroid)
        {
            CameraDebug.Log("Frame format adjustment is only supported on Android");
            return;
        }
        currentFormat = supportedFormats[value];
        plugin.androidJavaObject.Call<bool>("ChangeFrameFormat", deviceID, value);
        CameraDebug.Log("Format Changed: " + currentFormat);
        InitScreen();
    }
    public IEnumerator InitCamera()
    {
        frameID = 0;
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if (!isUSBCamera)
            {
                WebCamDevice[] devices = WebCamTexture.devices;
                try
                {
                    webCamTexture = new WebCamTexture(devices[deviceID].name, width, height, FPS);
                    webCamTexture.Play();
                    if (!onlyRenderOnUI)
                        screenRender.material.mainTexture = webCamTexture;
                    screenImage.texture = webCamTexture;
                    screenImage.material.mainTexture = webCamTexture;
                }
                catch (Exception e)
                {
                    CameraDebug.Log("Can not connect camera " + deviceID + ": " + e);
                }
            }
        }
    }
    public IEnumerator InitCameraForAndroid()
    {
        if (!isUSBCamera)
            StartCoroutine("InitCamera");
        if (isUSBCamera)
        {
            supportedSizes.Clear();
            //yield return new WaitForSeconds(1F);
            yield return new WaitUntil(() => RequireSupportedSize());
            yield return new WaitUntil(() => supportedSizes.Count != 0);
            sizeSelector.value = supportedSizes.IndexOf(width + "-" + height);
            if (enableFPSDisplay)
            {
                StopCoroutine("FPSCounter");
                StartCoroutine("FPSCounter");
            }
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            float runTime = 0;
            while (!stopping)
            {
                frameID++;
                stopwatch.Stop();
                runTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Reset();
                if (runTime < ((float)1 / FPS))
                    runTime = ((float)1 / FPS) - runTime;
                else
                    runTime = 0;
                yield return new WaitForSeconds(runTime);
                stopwatch.Start();
                yield return new WaitUntil(() => playing);
                //Uncomment this line for debug output
                //yield return new WaitForSeconds(1f + UnityEngine.Random.value);
                try
                {
                    tempTexture2D = plugin.GetFrame(deviceID);
                    if (tempTexture2D != null)
                    {
                        if (!onlyRenderOnUI)
                            screenRender.material.mainTexture = tempTexture2D;
                        screenImage.texture = tempTexture2D;
                        screenImage.material.mainTexture = tempTexture2D;
                    }
                }
                catch (Exception e)
                {
                    CameraDebug.Log("Empty frame: " + e);
                }
            }
        }
    }
    public IEnumerator FPSCounter()
    {
        int tempID = 0;
        while (!stopping)
        {
            tempID = frameID;
            yield return new WaitForSeconds(1.0f);
            FPS_text.text = "FPS: " + (frameID - tempID);
        }
    }
    public void AutoFitScreen(GameObject screenObject, float weight)
    {
        Vector3 scale = Vector3.one * weight;
        screenObject.transform.localScale = new Vector3(textureSize.x*scale.x * (float)width / (float)height, textureSize.y * scale.y, scale.z);
    }
    public void SwitchDetailsPanel()
    {
        detailsPanel.SetActive(!detailsPanel.activeSelf);
    }
    public void OnBrightnessSliderChange(float value)
    {
        if (isNotAndroid)
        {
            CameraDebug.Log("Brightness adjustment is only supported on Android");
            return;
        }
        plugin.androidJavaObject.Call<int>("setCameraDetailsParameter", deviceID, 0, (int)value, false);
    }
    public void OnContrastSliderChange(float value)
    {
        if (isNotAndroid)
        {
            CameraDebug.Log("Contrast adjustment is only supported on Android");
            return;
        }
        plugin.androidJavaObject.Call<int>("setCameraDetailsParameter", deviceID, 1, (int)value, false);
    }
    public void TakePhoto()
    {
#if UNITY_ANDROID
        album.SavePhoto(TextureToTexture2D(screenImage.texture));
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        album.SavePhoto(TextureToTexture2D(screenImage.texture, true));
#endif
    }
    public Texture2D TextureToTexture2D(Texture texture, bool setWithCameraSize = false)
    {
        Texture2D texture2D = null;
        if (setWithCameraSize)
            texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
        else
            texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }
}
