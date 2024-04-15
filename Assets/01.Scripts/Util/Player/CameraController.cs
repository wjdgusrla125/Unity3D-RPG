using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera VCamera;
    
    private Transform playerObject;

    private void Awake()
    {
        playerObject = GameManager.Instance.Player.transform;
    }

    private void Start()
    {
        if (VCamera != null && playerObject != null)
        {
            VCamera.Follow = playerObject;
        }
    }
}
