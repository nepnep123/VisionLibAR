using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCameraSetting : MonoBehaviour
{
    public Transform target;        // 따라다닐 타겟 오브젝트의 Transform
    public RocketLauncher_B target_cs;

    [HideInInspector] Vector3 default_pos, default_scale;
    [HideInInspector] Quaternion default_rot;

    private Transform tr;                // 카메라 자신의 Transform

    void Start()
    {
        tr = GetComponent<Transform>();

        default_pos = gameObject.transform.localPosition;
        default_scale = gameObject.transform.localScale;
        default_rot = gameObject.transform.localRotation;
    }

    void LateUpdate()
    {
        if (target != null && target_cs.isSet)
        {
            tr.position = new Vector3(target.position.x -20f, target.position.y, target.position.z);

            //tr.LookAt(target);
        }
    }


    public void InitTransform()
    {
        gameObject.transform.localPosition = default_pos;
        gameObject.transform.localRotation = default_rot;
        gameObject.transform.localScale = default_scale;
    }
}
