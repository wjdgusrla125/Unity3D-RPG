using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPivot : MonoBehaviour
{
    public float distanceFromTarget = 6f; // 대상에서 떨어진 초기 거리
    public Vector3 offset = new Vector3(0, 4, -4); // 대상 대비 카메라의 오프셋
    public float smoothSpeed = 3f; // 카메라 이동 속도
    
    public Transform target; // 대상 (플레이어)
    
    private void LateUpdate()
    {
        FollowCam();
    }

    private void FollowCam()
    {
        target = GameManager.Instance.Player.transform;
        
        Vector3 desiredPosition = target.position - transform.forward * distanceFromTarget + offset;
        RaycastHit hit;

        Debug.DrawRay(target.position, -transform.forward * 13f, Color.blue);
    
        if (Physics.Raycast(target.position, -transform.forward, out hit, 13f))
        {
            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
               
                Vector3 hitAdjustedPosition = hit.point + transform.forward * 1f;
                
                float distanceToTarget = (hitAdjustedPosition - target.position).magnitude;
                
                if (distanceToTarget < 3f)
                {
                    desiredPosition = transform.position;
                }
                else
                {
                    desiredPosition = hitAdjustedPosition;
                }
            }
        }
    
        if (desiredPosition != transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
