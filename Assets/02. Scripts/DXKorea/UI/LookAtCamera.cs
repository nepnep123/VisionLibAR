using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] Camera target;

    private void Update()
    {
        if (target != null)
        {
            StartCoroutine(LookAtTarget());
        }
    }

    IEnumerator LookAtTarget()
    {
        yield return null;

        transform.LookAt(transform.position + target.transform.rotation * Vector3.forward,
            target.transform.rotation * Vector3.up);
    }
}
