using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelCanvasSetting : MonoBehaviour
{
    [SerializeField] public ModelingTouchModule shoot_model, radar_model, aesaRadar_model;

    [Header(" [ SCRIPTS RESOURCE ] ")]
    [SerializeField] public UIManager UM;

    [SerializeField] public ButtonSetting select_btn;

    [SerializeField] public Image background;
    [SerializeField] public Sprite 모듈별기능_back_spr, AESA소개_back_spr;


    [HideInInspector] public Sprite select_btn_back;
    [HideInInspector] public string select_btn_text;

    public void ModelingInit()
    {
        shoot_model.gameObject.SetActive(false);
        radar_model.gameObject.SetActive(false);
        aesaRadar_model.gameObject.SetActive(false);
    }


    //3D모델링 버튼 클릭
    public void ModelingSetting(string model)
    {
        ModelingInit();
        SelectButtonSetting();

        switch (model)
        {
            case "RadarModel":
                background.sprite = 모듈별기능_back_spr;
                radar_model.gameObject.SetActive(true);
                break;

            case "ShootModel":
                background.sprite = 모듈별기능_back_spr;
                shoot_model.gameObject.SetActive(true);
                break;

            case "AesaRadarModel":
                background.sprite = AESA소개_back_spr;
                aesaRadar_model.gameObject.SetActive(true);
                break;
        }
    }

    public void SelectButtonSet(Sprite back_spr, string text)
    {
        select_btn_back = back_spr;

        select_btn_text = text;
    }

    public void SelectButtonSetting()
    {
        select_btn._image.sprite = select_btn_back;

        select_btn._text.text = select_btn_text;
    }

    public void ModelingZoneBackButtonClick()
    {
        UM.BackResoruce();
    }

    public void ToMain()
    {
        UM.BackResoruce();

        //UM.mainCanvas.BackgroundController("MAIN");

        //UM.mainCanvas.ResourceInit();
    }

    private void OnDisable()
    {
        ModelingInit();
    }
}
