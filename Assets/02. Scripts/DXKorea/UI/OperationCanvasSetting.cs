using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OperationCanvasSetting : MonoBehaviour
{
    [Header(" [ UI RESOURCE ] ")]
    [SerializeField] TextMeshProUGUI operateNum_txt;
    [SerializeField] TextMeshProUGUI operateState_txt;
    [SerializeField] public GameObject preBtn, nextBtn;
    [SerializeField] public GameObject operationFinishInfo;

    public void NarrationTextSetting(int step, int maxcnt)
    {
        operationFinishInfo.SetActive(false);

        switch (step)
        {
            case 0:
                preBtn.SetActive(false);
                nextBtn.SetActive(true);
                operateNum_txt.text = "1/" + maxcnt;
                operateState_txt.text = " Step.1 공기필터 교체 정비절차를 시작합니다. ";        
                break;
            case 1:
                preBtn.SetActive(true);
                nextBtn.SetActive(true);
                operateNum_txt.text = "2/" + maxcnt;
                operateState_txt.text = " Step.2 클립을 제거합니다. ";
                break;
            case 2:
                preBtn.SetActive(true);
                nextBtn.SetActive(true);
                operateNum_txt.text = "3/" + maxcnt;
                operateState_txt.text = " Step.3 공기필터 나사와 마개를 제거합니다. ";
                break;
            case 3:
                preBtn.SetActive(true);
                nextBtn.SetActive(true);
                operateNum_txt.text = "4/" + maxcnt;
                operateState_txt.text = " Step.4 오염된 공기필터를 제거합니다. ";
                break;
            case 4:
                preBtn.SetActive(true);
                nextBtn.SetActive(true);
                operateNum_txt.text = "5/" + maxcnt;
                operateState_txt.text = " Step.5 새로운 공기필터로 교체합니다. ";
                break;
            case 5:
                preBtn.SetActive(true);
                nextBtn.SetActive(true);
                operateNum_txt.text = "6/" + maxcnt;
                operateState_txt.text = " Step.6 공기필터 나사와 마개를 결합합니다. ";
                break;
            case 6:
                preBtn.SetActive(true);
                nextBtn.SetActive(true);
                operateNum_txt.text = "7/" + maxcnt;
                operateState_txt.text = " Step.7 클립을 조립합니다. ";
                break;
            case 7:
                preBtn.SetActive(true);
                nextBtn.SetActive(false);
                operateNum_txt.text = "8/" + maxcnt;
                operateState_txt.text = " Step.8 공기필터 교체 절차 완료. ";

                operationFinishInfo.SetActive(true);
                break;
        }
    }


    private void OnDisable()
    {
        operationFinishInfo.SetActive(false);
    }
}
