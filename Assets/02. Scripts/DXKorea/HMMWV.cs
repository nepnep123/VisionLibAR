using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AlphaObject
{
    public Renderer object_Mr;
    public Material[] default_mats;
    public Material[] active_mats;
}

public class HMMWV : MonoBehaviour
{
    //[Header(" [ OBJECT RESOURCE ]")]
    //[SerializeField] Animator hood_anim;

    [SerializeField] CollisionTouchEvent[] interatives;
    [SerializeField] GameObject[] interative_popups;
    [SerializeField] GameObject[] infoButtons;

    [SerializeField] GameObject[] fade_info_popups;
    [SerializeField] GameObject[] fade_info_buttons;

    public AlphaObject[] alphaObjects;
    //[SerializeField] Slider transparencySlider;
    //[SerializeField] Image fill_IMG;
    //[SerializeField] TextMeshProUGUI valueTMP;
    //[SerializeField] float maxValue = 1f;
    //float sliderValue;

    private void Awake()
    {
        AlphaModelingSetting();
    }

    private void OnEnable()
    {
        InitInterative();

        InitInterativePopup();
        InitFadeInfoPopup();

        InitButton();
        InitFadeInfoButton();
    }

    private void OnDisable()
    {
        InitInterative();

        InitInterativePopup();
        InitFadeInfoPopup();

        InitButton();
        InitFadeInfoButton();
    }

    //인터렉션 시작 (초기화)
    public void ResetInterativeData()
    {
        InterativeStart();

        InitInterativePopup();

        InitFadeInfoPopup();
    }

    public void ResetFadeData()
    {
        InterativeFadeStart();

        InitInterativePopup();

        InitFadeInfoPopup();
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
    }

    void InitFadeInfoPopup()
    {
        for (int i = 0; i < fade_info_popups.Length; i++)
        {
            fade_info_popups[i].SetActive(false);
        }
    }

    void InitFadeInfoButton()
    {
        foreach (GameObject item in fade_info_buttons)
        {
            item.SetActive(false);
        }
    }


    public void InterativeStart()
    {
        InitFadeInfoPopup();
        InitFadeInfoButton();

        for (int i = 0; i < interatives.Length; i++)
        {
            interatives[i].SetInteraction();
        }

        foreach (GameObject item in infoButtons)
            item.SetActive(true);
    }

    public void SelectInteration(int tmp)
    {
        foreach (GameObject item in infoButtons)
            item.SetActive(true);

        for (int i = 0; i < interatives.Length; i++)
            interatives[i].SetInteraction();

        InitInterativePopup();

        infoButtons[tmp].gameObject.SetActive(false);
        interatives[tmp].SelectEvent();
        interative_popups[tmp].SetActive(true);
    }

    public void InterativeFadeStart()
    {
        InitInterative();
        InitInterativePopup();
        InitButton();

        foreach (GameObject item in fade_info_buttons)
            item.SetActive(true);
    }

    public void SelectFadeInteration(int tmp)
    {
        foreach (GameObject item in fade_info_buttons)
            item.SetActive(true);

        InitFadeInfoPopup();

        fade_info_buttons[tmp].gameObject.SetActive(false);
        fade_info_popups[tmp].SetActive(true);
    }

    //INIT
    public void AlphaModelingSetting()
    {
        for (int i = 0; i < alphaObjects.Length; i++)
        {
            alphaObjects[i].default_mats = new Material[alphaObjects[i].object_Mr.materials.Length];

            for (int j = 0; j < alphaObjects[i].object_Mr.materials.Length; j++)
            {
                alphaObjects[i].default_mats[j] = alphaObjects[i].object_Mr.materials[j];
            }
        }
    }

    public void AlphaObjectController(bool active)
    {
        if (active)
        {
            InterativeFadeStart();

            for (int i = 0; i < alphaObjects.Length; i++)
            {
                Material[] mats = alphaObjects[i].object_Mr.materials;
                for (int z = 0; z < mats.Length; z++)
                {
                    mats[z] = alphaObjects[i].active_mats[z];
                }
                alphaObjects[i].object_Mr.materials = mats;
            }
        }
        else
        {
            InitFadeInfoButton();
            InitFadeInfoPopup();

            for (int i = 0; i < alphaObjects.Length; i++)
            {
                Material[] mats = alphaObjects[i].object_Mr.materials;
                for (int z = 0; z < mats.Length; z++)
                {
                    mats[z] = alphaObjects[i].default_mats[z];
                }
                alphaObjects[i].object_Mr.materials = mats;
            }
        }
    }
}
