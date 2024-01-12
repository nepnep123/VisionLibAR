using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OperationSetting
{
    public Animator operation_anim;

    public int animClip_cnt;
}

public class HMMWV_Engine : MonoBehaviour
{
    [SerializeField] public Animator hood_anim;
    
    [Header(" [ OBJECT RESOURCE ]")]
    [SerializeField] public OperationSetting airFilterOperation;

    [SerializeField] CollisionTouchEvent[] interatives;
    [SerializeField] GameObject[] interative_popups;
    [SerializeField] GameObject[] infoButtons;
    [SerializeField] GameObject[] operButtons;

    [Header(" [ SCRIPTS RESOURCE ]")]
    [SerializeField] OperationCanvasSetting operationCanvasSetting;

    private void OnEnable()
    {
        operStep = 0;

        InitInterative();
        StartCoroutine(ScanModelStart());

        InitButton();

        InitInterativePopup();

        InitAnim();
    }



    private void OnDisable()
    {
        InitInterative();

        InitButton();

        InitInterativePopup();

        InitAnim();
    }

    public void ResetInterativeData()
    {
        InterativeStart();

        InitInterativePopup();
    }

    void InitInterative()
    {
        for (int i = 0; i < interatives.Length; i++)
        {
            interatives[i].InitInteraction();
        }
    }

    void InitInterativePopup()
    {
        for (int i = 0; i < interative_popups.Length; i++)
        {
            interative_popups[i].SetActive(false);
        }
    }

    void InitButton()
    {
        foreach (GameObject item in infoButtons)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in operButtons)
        {
            item.SetActive(false);
        }
    }

    IEnumerator ScanModelStart()
    {
        hood_anim.gameObject.SetActive(true);

        yield return new WaitForSeconds(3.5f); //본네트 열리는 시간 

        hood_anim.gameObject.SetActive(false);
    }

    public void InterativeStart()
    {
        hood_anim.gameObject.SetActive(false);

        for (int i = 0; i < interatives.Length; i++)
        {
            interatives[i].SetInteraction();
        }

        foreach (GameObject item in infoButtons)
            item.SetActive(true);

        foreach (GameObject item in operButtons)
            item.SetActive(true);

        InitInterativePopup();

        InitAnim();
        operationCanvasSetting.gameObject.SetActive(false);
    }

    //정보 확인 버튼 클릭
    public void SelectInteration(int tmp)
    {
        foreach (GameObject item in infoButtons)
            item.SetActive(true);

        foreach (GameObject item in operButtons)
            item.SetActive(true);

        for (int i = 0; i < interatives.Length; i++)
            interatives[i].SetInteraction();

        InitInterativePopup();

        InitAnim();
        operationCanvasSetting.gameObject.SetActive(false);

        infoButtons[tmp].gameObject.SetActive(false);
        operButtons[tmp].gameObject.SetActive(false);

        interatives[tmp].SelectEvent();
        interative_popups[tmp].SetActive(true);
    }

    #region Operation
    //정비절차 버튼 클릭 (아직까진 정비절차가 한개밖에 없어서 따로 X)
    public void OperationStart()
    {
        //초기화 
        InitInterative();
        InitInterativePopup();
        InitButton();

        //아직은 하나밖에 없어서 처리 X
        operationCanvasSetting.gameObject.SetActive(true);
        OperationSetting(0);
    }


    void InitAnim()
    {
        airFilterOperation.operation_anim.SetInteger("Oper", -1);
    }


    int operStep = 0;
    public void OperationSetting(int _operNum)
    {
        InitAnim();
        operStep = _operNum;

        airFilterOperation.operation_anim.SetInteger("Oper", operStep);

        operationCanvasSetting.NarrationTextSetting(operStep, airFilterOperation.animClip_cnt);
    }

    public void OperationNext()
    {
        operStep += 1;
        OperationSetting(operStep);
    }

    public void OperationPre()
    {
        operStep -= 1;
        OperationSetting(operStep);
    }

    #endregion
}
