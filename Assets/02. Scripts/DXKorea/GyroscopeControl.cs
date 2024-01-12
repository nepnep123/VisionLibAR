using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeControl : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;

    private GameObject camreaContainer;
    private Quaternion rot;

    private void Start() {
        camreaContainer = new GameObject("Camera Container");
        camreaContainer.transform.position = transform.position;
        transform.SetParent(camreaContainer.transform);

        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            camreaContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0);
            rot = new Quaternion(0, 0, 1, 0);

            return true; 
        }

        return false;
    }

    private void Update() {
        if(gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;
        }
    }
}
