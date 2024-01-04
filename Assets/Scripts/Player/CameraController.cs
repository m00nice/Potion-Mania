using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Cinemachine.AxisState xAxis, yAxis;
    private bool isAiming;

    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (Player.Instance.InventoryActive)return;
        followTarget.localEulerAngles = new Vector3(-yAxis.Value, followTarget.localEulerAngles.y, followTarget.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
    
    
    
}
