using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visometry.VisionLib.SDK.Core;
using Visometry.VisionLib.SDK.Core.API;

public class Main_UIManager : MonoBehaviour
{
    [Header(" [ CANVAS RESOURCE ] ")]
    [SerializeField] public UI_CanvasSetting ui_Canvas;
    [SerializeField] public Model_CanvasSetting model_Canvas;

    [Header(" [ CAMERA RESOURCE ] ")]
    [SerializeField] public Camera vl_Camera;
    [SerializeField] public GameObject model_Camera;
    [SerializeField] public Camera ui_Camera;

    [Header(" [ SCRIPTS RESOURCE ] ")]
    [SerializeField] public TrackingConfiguration trackConfig_cs;
    [SerializeField] public TrackingManager trackManager_cs;

    [Header(" [ MODELING RESOURCE ] ")]
    [SerializeField] public GameObject msam_targetzone_obj;
    [SerializeField] public ShootingBullet msam_cs;
    [SerializeField] public RocketLauncher_B[] rocket_cs;

    private void Start()
    {
        CanvasDataInit();

        vl_Camera.gameObject.SetActive(false);
        model_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        msam_targetzone_obj.gameObject.SetActive(true);
        msam_cs.gameObject.SetActive(false);

        ui_Canvas.gameObject.SetActive(true);
        model_Canvas.gameObject.SetActive(false);
    }

    //모든 리소스 데이터 리셋 
    public void CanvasDataInit()
    {
        ui_Canvas.ResourceInit();
        model_Canvas.ModelingInit();
    }

    //정합 환경 셋팅
    public void TargettingSetting()
    {
        vl_Camera.gameObject.SetActive(true);
        model_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        ui_Canvas.background.gameObject.SetActive(false);

        trackConfig_cs.StartTracking();
    }

    //정합 환경 비활성화 
    public void TargettingOff()
    {
        vl_Camera.gameObject.SetActive(false);
        model_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        ui_Canvas.background.gameObject.SetActive(true);

        trackConfig_cs.CancelCameraSelection();
        trackManager_cs.StopTracking();

        MSAMReadyFail();

        //ui_Canvas.RenderTextrueController(false);
    }

    //컨트롤가능한 모델링 호출 
    public IEnumerator ModelingControlSetting(string model, bool isdelay)
    {
        if(isdelay)
            yield return new WaitForSeconds(1.2f);
        
        vl_Camera.gameObject.SetActive(false);
        model_Camera.gameObject.SetActive(true);
        ui_Camera.gameObject.SetActive(true);

        ui_Canvas.gameObject.SetActive(false);
        model_Canvas.gameObject.SetActive(true);

        model_Canvas.ModelingSetting(model);
    }

    //뒤로가기로 인해 다시 돌아온경우
    public void BackResoruce()
    {
        vl_Camera.gameObject.SetActive(false);
        model_Camera.gameObject.SetActive(false);
        ui_Camera.gameObject.SetActive(true);

        ui_Canvas.gameObject.SetActive(true);
        model_Canvas.gameObject.SetActive(false);
    }


    //발사 버튼 클릭
    public void MSAMShootStart()
    {
        msam_cs.anim.SetBool("Trigger", true);

        //ui_Canvas.RenderTextrueController(true);
    }

    //정합 성공시 
    public void MSAMReadySuceess()
    {
        msam_targetzone_obj.SetActive(false);
        msam_cs.gameObject.SetActive(true);

        ui_Canvas.shoot_btn.gameObject.SetActive(true);
        //ui_Canvas.RenderTextrueController(false);
    }

    //타겟 정합 실패시 
    public void MSAMReadyFail()
    {
        msam_cs.anim.SetBool("Trigger", false);

        msam_targetzone_obj.SetActive(true);
        msam_cs.gameObject.SetActive(false);

        ui_Canvas.shoot_btn.gameObject.SetActive(false);
        //ui_Canvas.RenderTextrueController(false);
    }

    //타겟 정합 불안할시 


    //미사일 발사 (애니메이션 X)
    //public void MSAMShoot()
    //{
    //    StartCoroutine(msam_cs.Fire());

    //    //ui_Canvas.RenderTextrueController(true);
    //}

    //미사일 발사 (애니메이션 O)
    public IEnumerator MSAMShoot_Anim()
    {
        if (rocket_cs.Length == 0) yield break;

        foreach (var item in rocket_cs)
            item.gameObject.SetActive(false);

        for (int i = 0; i < rocket_cs.Length; i++)
        {
            rocket_cs[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void rocketInit()
    {
        foreach (var item in rocket_cs)
            item.gameObject.SetActive(false);
    }
}
