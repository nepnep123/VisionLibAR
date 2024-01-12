using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSetting : MonoBehaviour
{
    public GameObject exploEffect_pre;

    GameObject exploEffect;

    public bool isColl = false; //충돌했는지 여부 


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "TARGET")
        {
            isColl = true;
            // Debug.Log("HIT : " + other.gameObject.name);

            exploEffect = Instantiate(exploEffect_pre, gameObject.transform);
            Destroy(exploEffect, 1.0f);
        }
    }
}
