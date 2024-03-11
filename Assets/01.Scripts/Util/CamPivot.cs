using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPivot : MonoBehaviour
{
    private void LateUpdate()
    {
        FollowCam();
    }

    private void FollowCam()
    {
        Vector3 pos = transform.position;

        transform.position = Vector3.Lerp(pos, GameManager.Instance.Player.transform.position, 0.4f);
    }
}
