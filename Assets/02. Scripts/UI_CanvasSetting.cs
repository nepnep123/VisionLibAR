using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Visometry.VisionLib.SDK.Core;

[System.Serializable]
public struct MainButtons
{
    public ButtonSetting parent_btn;
    public ButtonSetting[] child_btn;
}

public class UI_CanvasSetting : MonoBehaviour
{
    [SerializeField] Animator menuList_anim;
    [SerializeField] public MainButtons[] main_btns;

    [Header(" [ BACKGROUND RESOURCE ] ")]
    [SerializeField] public Image background; //정합하지 않을때는 활성화 시켜놓는다. 
    [SerializeField] public GameObject[] page_obj;
    [SerializeField] Sprite 메인_back, 작전운용개념_back, 모듈별기능_back, AESA소개_back, 시스템별구분_back;

    [Header(" [ BUTTON RESOURCE ] ")]
    [SerializeField] public ButtonSetting select_btn;
    
    //[Header(" [ 모듈별 기능 RESOURCE ] ")]
    //[SerializeField] public GameObject[] 모듈별기능_contents;
    //[SerializeField] public GameObject[] 모듈별기능_activeObject; //눌럿을때 활성화 비활성화될 객체들 
    //[SerializeField] public GameObject[] 모듈별기능_switchButton; //홈버튼 -> 뒤로가기버튼 / 뒤로가기버튼 -> 홈버튼 
    
    [Header(" [ AESA 소개 RESOURCE ] ")]
    [SerializeField] public Animator AESA소개_anim;

    [Header(" [ AR시뮬레이션 RESOURCE ] ")]
    [SerializeField] public ButtonSetting shoot_btn;

    //[Header(" [ RENDER TEXTURE ] ")]
    //[SerializeField] public Camera render_Camera;
    //[SerializeField] public GameObject renderCameraTexture_obj;

    [Header(" [ SCRIPTS RESOURCE ] ")]
    [SerializeField] public Main_UIManager UM;

    private void Start()
    {
        BackgroundController("MAIN");
        
        ResourceInit();
    }

    public void ResourceInit()
    {
        ButtonInit();

        background.gameObject.SetActive(true);

        //RenderTextrueController(false);
    }


    #region INIT 
    //public void RenderTextrueController(bool isAcitve)
    //{
    //    render_Camera.GetComponent<RenderCameraSetting>().InitTransform();

    //    render_Camera.gameObject.SetActive(isAcitve);
    //    renderCameraTexture_obj.SetActive(isAcitve);

    //    //cancel_btn.gameObject.SetActive(isAcitve);
    //    //shoot_btn.gameObject.SetActive(isAcitve);
    //}

    void ButtonInit()
    {
        for (int i = 0; i < main_btns.Length; i++)
        {
            main_btns[i].parent_btn.SetDefault();

            for (int j = 0; j < main_btns[i].child_btn.Length; j++)
            {
                main_btns[i].child_btn[j].gameObject.SetActive(false);

                main_btns[i].child_btn[j].SetDefault();
            }
        }
    }

    //void ButtonStateInit()
    //{
    //    for (int i = 0; i < main_btns.Length; i++)
    //    {
    //        for (int j = 0; j < main_btns[i].child_btn.Length; j++)
    //        {
    //            main_btns[i].child_btn[j].SetDefault();
    //        }
    //    }
    //}

    void PageInit()
    {
        for (int i = 0; i < page_obj.Length; i++)
        {
            page_obj[i].SetActive(false);
        }
    }
    #endregion

    #region BACKGROUND 
    public void BackgroundController(string mode)
    {
        PageInit();

        switch (mode)
        {
            case "MAIN":
                page_obj[0].SetActive(true);
                background.sprite = 메인_back;

                menuList_anim.SetBool("Active_0", false);
                menuList_anim.gameObject.SetActive(true);

                select_btn.gameObject.SetActive(false);
                break;

            case "작전운용개념":
                page_obj[1].SetActive(true);
                background.sprite = 작전운용개념_back;

                menuList_anim.gameObject.SetActive(false);

                select_btn.gameObject.SetActive(true);
                break;

            case "모듈별 구분":
                page_obj[2].SetActive(true);
                background.sprite = 모듈별기능_back;

                menuList_anim.gameObject.SetActive(false);

                select_btn.gameObject.SetActive(true);
                break;

            case "AESA 소개":
                page_obj[3].SetActive(true);
                background.sprite = AESA소개_back;

                menuList_anim.gameObject.SetActive(false);

                select_btn.gameObject.SetActive(true);
                break;

            case "시스템별 구분":
                page_obj[4].SetActive(true);
                background.sprite = 시스템별구분_back;

                menuList_anim.gameObject.SetActive(false);

                select_btn.gameObject.SetActive(true);
                break;

            case "발사체 \nAR 애니메이션":
                page_obj[5].SetActive(true); //배경은 없다. 
                //background.sprite = 메인_back;

                menuList_anim.gameObject.SetActive(false);

                select_btn.gameObject.SetActive(false);
                shoot_btn.gameObject.SetActive(false);
                break;
        }
    }

    
    public void SelectButtonSetting(Sprite back_spr, string text)
    {
        select_btn._image.sprite = back_spr;

        select_btn._text.text = text;
    }

    #endregion


    #region BUTTON EVENT

    //버튼 관리 
    public void ButtonControll(int tmp)
    {
        //클릭 시 나머지 자식 버튼 비활성화 
        for (int i = 0; i < main_btns.Length; i++)
        {
            if (i == tmp)
            {
                if (main_btns[i].parent_btn.isActive)
                {
                    menuList_anim.SetBool("Active_0", false);

                    for (int j = 0; j < main_btns[i].child_btn.Length; j++)
                    {
                        main_btns[i].child_btn[j].SetDefault();
                        main_btns[i].child_btn[j].gameObject.SetActive(false);
                    }
                }
                else
                {
                    menuList_anim.SetBool("Active_0", true);

                    for (int j = 0; j < main_btns[i].child_btn.Length; j++)
                    {
                        main_btns[i].child_btn[j].gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                main_btns[i].parent_btn.SetDefault();

                for (int j = 0; j < main_btns[i].child_btn.Length; j++)
                {
                    main_btns[i].child_btn[j].SetDefault();

                    main_btns[i].child_btn[j].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ContentButtonClick(ButtonSetting button)
    {
        //ButtonStateInit();

        switch (button._text.text)
        {
            case "작전운용개념":
                BackgroundController(button._text.text);
                SelectButtonSetting(button._image.sprite, button._text.text);
                //UM.model_Canvas.SelectButtonSet(button._image.sprite, button._text.text);

                UM.TargettingOff();

                break;

            case "모듈별 구분":
                BackgroundController(button._text.text);
                SelectButtonSetting(button._image.sprite, button._text.text);
                UM.model_Canvas.SelectButtonSet(button._image.sprite, button._text.text);

                UM.TargettingOff();

                break;

            case "AESA 소개":
                BackgroundController(button._text.text);
                SelectButtonSetting(button._image.sprite, button._text.text);
                UM.model_Canvas.SelectButtonSet(button._image.sprite, button._text.text);

                UM.TargettingOff();

                break;

            case "시스템별 구분":
                BackgroundController(button._text.text);
                SelectButtonSetting(button._image.sprite, button._text.text);
                //UM.model_Canvas.SelectButtonSet(button._image.sprite, button._text.text);

                UM.TargettingOff();

                break;

            case "발사체 \nAR 애니메이션":
                BackgroundController(button._text.text);
                SelectButtonSetting(button._image.sprite, button._text.text);
                //UM.model_Canvas.SelectButtonSet(button._image.sprite, button._text.text);

                UM.TargettingSetting();
                break;
        }

    }

    //Home 버튼 
    public void ToMain()
    {
        BackgroundController("MAIN");

        ResourceInit();
    }

    //뒤로가기 버튼 
    public void ToBack()
    {

    }

    public void 모듈별기능_ContentStart(string mode)
    {
        StartCoroutine(UM.ModelingControlSetting(mode, false));


    }

    public void AESA소개_buttonClick()
    {
        AESA소개_anim.SetBool("Trigger", true);

        StartCoroutine(UM.ModelingControlSetting("AesaRadarModel", true));
    }

    public void AR시뮬레이션_HomeButtonClick()
    {
        BackgroundController("MAIN");

        ResourceInit();

        UM.TargettingOff();
    }

    #endregion
}
