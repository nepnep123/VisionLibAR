using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//[System.Serializable]
//public struct MainBtn
//{
//    public ButtonSetting parent_btn;
//    public ButtonSetting[] child_btn;
//}

public class MainCanvasSetting : MonoBehaviour
{
    [SerializeField] Animator menuList_anim;

    [SerializeField] public OperationCanvasSetting operationMenu;

    [Header(" [ BACKGROUND RESOURCE ] ")]
    [SerializeField] public GameObject background;
    [SerializeField] public Animator fadeBackground;
    [SerializeField] public GameObject MainMenuPanel, ScanGuidePanel;


    [Header(" [ BUTTON RESOURCE ] ")]
    [SerializeField] public ButtonSetting backBtn; //있는 경우 체크 필요
    [SerializeField] ButtonSetting InOutBtn, engineRoomBtn, scanBtn;

    [Header(" [ 스캔해야 나오는 버튼 (각 Scene 마다 다르다) ] ")]
    [SerializeField] public ButtonSetting operBtn; //엔진룸
    [SerializeField] public ButtonSetting infoBtn, alphaBtn; //험비외부 

    [Header(" [ SCRIPTS RESOURCE ] ")]
    [SerializeField] public TargetManager TargetManager;

    private void Start()
    {
        //초기상태 
        if(background != null) background.gameObject.SetActive(true);

        ActiveButtonController(false);
        
        ButtonInit();
        scanBtn.SetDefault();
        
        InitPanel();
        MainMenuPanel.SetActive(true);
        if (operationMenu != null) operationMenu.gameObject.SetActive(false);
    }


    #region INIT 

    public void InitPanel()
    {
        MainMenuPanel.SetActive(false);
        ScanGuidePanel.SetActive(false);
        if (operationMenu != null) operationMenu.gameObject.SetActive(false);
    }

    public void ButtonInit()
    {
        backBtn.SetDefault();
        InOutBtn.SetDefault();
        engineRoomBtn.SetDefault();
        //scanBtn.SetDefault();
    }

    #endregion

    #region BACKGROUND 

    IEnumerator SceneLoader(string scene)
    {
        fadeBackground.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        LoadingSceneManager.LoadScene(scene);
    }

    #endregion


    #region BUTTON EVENT

    //정합 성공시에만 작동 
    public void ActiveButtonController(bool active)
    {
        backBtn.gameObject.SetActive(active);

        if (operBtn != null) operBtn.gameObject.SetActive(active);
        if (infoBtn != null) infoBtn.gameObject.SetActive(active);
        if (alphaBtn != null) alphaBtn.gameObject.SetActive(active);
    }
    
    public void ButtonClickContentEvent(string name)
    {
        ButtonInit();

        switch (name)
        {
            case "이전으로":
                TargetManager.TargettingOff();
                if(operationMenu != null) operationMenu.gameObject.SetActive(false);

                scanBtn.SetDefault();
                break;

            case "실내/실외보기":
                if(SceneManager.GetActiveScene().name == "Humvee" || SceneManager.GetActiveScene().name == "EngineRoom")
                {
                    StartCoroutine(SceneLoader("HumveeInside"));
                }
                else
                {
                    StartCoroutine(SceneLoader("Humvee"));
                }
                break;

            case "엔진룸":
                StartCoroutine(SceneLoader("EngineRoom"));
                break;

            case "험비":
                StartCoroutine(SceneLoader("Humvee"));
                break;

            //case "스캔하기":
            //    //각 Scene마다 타겟절차 존재
            //    TargetManager.TargettingSetting();

            //    scanBtn.SetActive();
            //    break;

            case "장비설명":
                if (TargetManager.targetModel.GetComponent<HMMWV>())
                {
                    TargetManager.targetModel.GetComponent<HMMWV>().InterativeStart();

                    trigger = false;
                    TargetManager.targetModel.GetComponent<HMMWV>().AlphaObjectController(trigger);
                }
                break;

            case "정비절차":
                if (TargetManager.targetModel.GetComponent<HMMWV_Engine>())
                    TargetManager.targetModel.GetComponent<HMMWV_Engine>().InterativeStart();
                break;

            case "내부보기":
                trigger = !trigger;
                if (TargetManager.targetModel.GetComponent<HMMWV>())
                    TargetManager.targetModel.GetComponent<HMMWV>().AlphaObjectController(trigger);
                break;
        }
    }
    //스캔하기 버튼 클릭 
    public void TargetScanButtonClick()
    {
        TargetManager.TargettingSetting();

        scanBtn.SetActive();
    }

    bool trigger = false;

    //인포 팝업창 취소버튼(험비 메인에서만 사용)
    public void InfoPopupCancel()
    {
        if(TargetManager.targetModel.GetComponent<HMMWV>())
            TargetManager.targetModel.GetComponent<HMMWV>().ResetInterativeData();
        else if (TargetManager.targetModel.GetComponent<HMMWV_Engine>())
            TargetManager.targetModel.GetComponent<HMMWV_Engine>().ResetInterativeData();

    }

    public void InfoFadePopUpCancel()
    {
        if (TargetManager.targetModel.GetComponent<HMMWV>())
            TargetManager.targetModel.GetComponent<HMMWV>().ResetFadeData();
    }

    #endregion
}
