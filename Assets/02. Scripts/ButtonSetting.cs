using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSetting : MonoBehaviour
{
    [Header(" [ BUTTON OPTION ] ")]
    [SerializeField] public Button _button;
    [SerializeField] public Image _image;
    [SerializeField] public Image _icon_image;
    [SerializeField] public TextMeshProUGUI _text;

    [HideInInspector] public bool isActive; //클릭된 상태인지 
    [SerializeField] public bool isToggle = false; //스위칭 되는 버튼인지 (변경되는 이미지가 필요하다)
    [SerializeField] public bool isTextString = false;
    [SerializeField] public string default_text, active_text;

    [Header(" [ BUTTON RESOURCE ] ")]
    [SerializeField] public bool isSpr = false;
    [SerializeField] Sprite default_spr, acitve_spr;

    [SerializeField] public bool isBackColor = false; 
    [SerializeField] Color default_back_color, active_back_color;

    [SerializeField] public bool isFontColor = false; 
    [SerializeField] Color default_font_color, active_font_color;


    //기본상태 
    public void SetDefault()
    {
        isActive = false;

        if (isSpr)
        {
            _image.sprite = default_spr;
        }

        if (isBackColor)
        {
            _image.color = default_back_color;
            if (_icon_image != null) _icon_image.color = default_back_color;
        }

        if (isFontColor)
        {
            _text.color = default_font_color;
        }

        if (_button != null)
        {
            _button.interactable = true;
            _image.raycastTarget = true;
        }
    }

    //일반 버튼 클릭 이벤트 (항상 초기화를 해줘야된다)
    public void SetActive()
    {
        isActive = true;

        if (isSpr)
        {
            _image.sprite = acitve_spr;
        }

        if (isBackColor)
        {
            _image.color = active_back_color;
            if (_icon_image != null) _icon_image.color = active_back_color;
        }

        if (isFontColor)
        {
            _text.color = active_font_color;
        }

        if (_button != null)
        {
            _button.interactable = false;
            _image.raycastTarget = false;
        }

    }

    //강제로 토글버튼 초기화 
    public void InitToggleButton()
    {
        if (isSpr)
        {
            _image.sprite = default_spr;
        }

        if (isBackColor)
        {
            _image.color = default_back_color;
            if (_icon_image != null) _icon_image.color = default_back_color;
        }

        if (isFontColor)
        {
            _text.color = default_font_color;
        }
    }

    public void ToggleButtonPointerUp()
    {
        if (isSpr)
        {
            if(_image.sprite == default_spr)
            {
                _image.sprite = acitve_spr;
            }
            else
            {
                _image.sprite = default_spr;
            }
        }

        if (isTextString)
        {
            if (_text.text == default_text)
            {
                _text.text = active_text;
            }
            else
            {
                _text.text = default_text;
            }
        }

        if (isBackColor)
        {
            _image.color = default_back_color;
            if (_icon_image != null) _icon_image.color = default_back_color;
        }

        if (isFontColor)
        {
            _text.color = default_font_color;
        }
    }

    //토글인 경우에만 해당 OnPointerDown
    public void ToggleButtonPointerDown()
    {
        if (isBackColor)
        {
            _image.color = active_back_color;
            if (_icon_image != null) _icon_image.color = active_back_color;
        }

        if (isFontColor)
        {
            _text.color = active_font_color;
        }
    }
}
