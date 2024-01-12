using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visometry.VisionLib.SDK.Core;
using Visometry.VisionLib.SDK.Core.API;

public class TargetManager : MonoBehaviour
{
    [Header(" [ CANVAS RESOURCE ] ")]
    [SerializeField] public MainCanvasSetting mainCanvas;

    [Header(" [ CAMERA RESOURCE ] ")]
    [SerializeField] public Camera vl_Camera;
    [SerializeField] public Camera ui_Camera;

    [Header(" [ SCRIPTS RESOURCE ] ")]
    [SerializeField] public TrackingConfiguration trackConfig_cs;
    [SerializeField] public TrackingManager trackManager_cs;

    [Header(" [ MODELING RESOURCE ] ")]
    [SerializeField] public GameObject targetModel;
    [SerializeField] public GameObject targetModel_dummy;

    private void Start()
    {
        targetModel_dummy.gameObject.SetActive(true);
        targetModel.gameObject.SetActive(false);

        vl_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);
    }

    #region INIT


    #endregion

    //정합 환경 셋팅
    public void TargettingSetting()
    {
        vl_Camera.gameObject.SetActive(true);
        ui_Camera.gameObject.SetActive(true);

        trackConfig_cs.StartTracking();

        mainCanvas.InitPanel();
    }

    //정합 성공시 
    public void MSAMReadySuceess()
    {
        targetModel_dummy.SetActive(false);
        targetModel.gameObject.SetActive(true);

        mainCanvas.InitPanel();
        mainCanvas.MainMenuPanel.SetActive(true);
        mainCanvas.ActiveButtonController(true);
    }

    //타겟 정합 실패시 
    public void MSAMReadyFail()
    {
        targetModel_dummy.SetActive(true);
        targetModel.gameObject.SetActive(false);

        mainCanvas.background.SetActive(false);
        mainCanvas.InitPanel();
        mainCanvas.ScanGuidePanel.SetActive(true);
        mainCanvas.ActiveButtonController(false);

        if (targetModel.GetComponent<HMMWV>())
            targetModel.GetComponent<HMMWV>().AlphaObjectController(false);
    }

    //정합 환경 비활성화 
    public void TargettingOff()
    {
        vl_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        trackConfig_cs.CancelCameraSelection();
        trackManager_cs.StopTracking();

        targetModel_dummy.SetActive(true);
        targetModel.gameObject.SetActive(false);

        mainCanvas.background.SetActive(true);
        mainCanvas.InitPanel();
        mainCanvas.MainMenuPanel.SetActive(true);

        mainCanvas.ActiveButtonController(false);

        mainCanvas.ButtonInit();

        if (targetModel.GetComponent<HMMWV>())
            targetModel.GetComponent<HMMWV>().AlphaObjectController(false);
    }
}
