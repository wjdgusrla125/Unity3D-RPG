using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPivot : MonoBehaviour
{
    private Transform target;  // 회전할 대상 오브젝트
    
    private void Awake()
    {
        target = GameManager.Instance.Player.transform;
    }

    private void LateUpdate()
    {
        FollowCam();
    }

    private void FollowCam()
    {
        Vector3 pos = this.transform.position;

        this.transform.position = Vector3.Lerp(pos, target.position, 0.4f);
    }
}
