using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visometry.VisionLib.SDK.Core;
using Visometry.VisionLib.SDK.Core.API;

public class UIManager : MonoBehaviour
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
    [SerializeField] public RocketLauncher_B[] rocket_cs;

    private void Start()
    {
        CanvasDataInit();

        vl_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        targetModel_dummy.gameObject.SetActive(true);
        targetModel.gameObject.SetActive(false);

        mainCanvas.gameObject.SetActive(true);
    }

    //모든 리소스 데이터 리셋 
    public void CanvasDataInit()
    {
        //mainCanvas.ResourceInit();
    }

    //정합 환경 셋팅
    public void TargettingSetting()
    {
        vl_Camera.gameObject.SetActive(true);
        ui_Camera.gameObject.SetActive(true);

        //mainCanvas.background.gameObject.SetActive(false);

        trackConfig_cs.StartTracking();
    }

    //정합 환경 비활성화 
    public void TargettingOff()
    {
        vl_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        //mainCanvas.background.gameObject.SetActive(true);

        trackConfig_cs.CancelCameraSelection();
        trackManager_cs.StopTracking();

        MSAMReadyFail();

        //mainCanvas.RenderTextrueController(false);
    }

    //컨트롤가능한 모델링 호출 
    public IEnumerator ModelingControlSetting(string model, bool isdelay)
    {
        if (isdelay)
            yield return new WaitForSeconds(1.2f);

        vl_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        mainCanvas.gameObject.SetActive(false);
    }

    //뒤로가기로 인해 다시 돌아온경우
    public void BackResoruce()
    {
        vl_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        mainCanvas.gameObject.SetActive(true);
    }


    //정합 성공시 
    public void MSAMReadySuceess()
    {
        targetModel_dummy.SetActive(false);
        targetModel.gameObject.SetActive(true);

        //mainCanvas.shoot_btn.gameObject.SetActive(true);

    }

    //타겟 정합 실패시 
    public void MSAMReadyFail()
    {
        targetModel_dummy.SetActive(true);
        targetModel.gameObject.SetActive(false);

        //mainCanvas.shoot_btn.gameObject.SetActive(false);
    }


}
