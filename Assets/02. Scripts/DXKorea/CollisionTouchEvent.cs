using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTouchEvent : MonoBehaviour
{
    [HideInInspector] Camera modelingCamera;
    [SerializeField] public UnityEvent collEvent;
    [SerializeField] BoxCollider _col;

    [Header(" [ CHILD RESOURCE ] ")]
    [SerializeField] GameObject _effect;
    [SerializeField] GameObject _effect_selected;

    [HideInInspector] bool trigger = false;
    [HideInInspector] Ray ray;
    [HideInInspector] RaycastHit hit;
    [HideInInspector] float startT, endT;

    private void Start()
    {
        modelingCamera = GameObject.Find("VLCamera").GetComponent<Camera>();
    }

    //작동
    public void SelectEvent()
    {
        _col.enabled = false;
        _effect.SetActive(false);
        if (_effect_selected != null) _effect_selected.SetActive(true);
    }

    //초기화
    public void InitInteraction()
    {
        _col.enabled = false;

        _effect.SetActive(false);
        if (_effect_selected != null) _effect_selected.SetActive(false);

        trigger = false; //작동 불가 
    }

    //활성화
    public void SetInteraction()
    {
        _col.enabled = true;

        //애니메이터 싱크 맞추기 위해 
        _effect.SetActive(false);
        _effect.SetActive(true);

        if (_effect_selected != null) _effect_selected.SetActive(false);

        trigger = true;
    }

    private void Update()
    {
        if (trigger)
        {
            ray = modelingCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startT = Time.time;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    endT = Time.time;
                    if (endT - startT < 0.15f)
                    {
                        if (hit.transform.gameObject == this.gameObject)
                        {
                            if (collEvent != null) collEvent.Invoke();
                        }

                        startT = 0;
                        endT = 0;
                    }
                }
            }
        }
    }
}
