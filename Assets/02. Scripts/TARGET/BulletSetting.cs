using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSetting : MonoBehaviour
{
    [SerializeField] ParticleSystem arrowEffect;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);

    }
}
