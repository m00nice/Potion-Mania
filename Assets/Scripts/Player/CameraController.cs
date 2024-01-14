using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private Transform orientation;
    
    private bool isAiming;
    
    public bool IsAiming
    {
        get => isAiming;
        set => isAiming = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isAiming)
        {
            cinemachineVirtualCamera.gameObject.SetActive(true);
            cinemachineFreeLook.gameObject.SetActive(false);
            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);
            
        }
        else
        {
            cinemachineVirtualCamera.gameObject.SetActive(false);
            cinemachineFreeLook.gameObject.SetActive(true);
            Vector3 viewDirection = Player.Instance.transform.position - new Vector3(cinemachineFreeLook.transform.position.x,
                Player.Instance.transform.position.y, cinemachineFreeLook.transform.position.z);
            orientation.forward = viewDirection;
            
            
        }
        
    }

    private void LateUpdate()
    {
        if (Player.Instance.InventoryActive)return;
        if (isAiming)
        {
            followTarget.localEulerAngles = new Vector3(-yAxis.Value, followTarget.localEulerAngles.y, followTarget.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
        }
        else
        {
            followTarget.forward = orientation.forward;
        }
        
    }
    
    
    
}
