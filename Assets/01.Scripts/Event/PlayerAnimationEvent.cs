using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerStats _stats;

    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
    }

    private void ColliderEnabled()
    {
        _stats.Colliders[0].enabled = true;
        _stats.Colliders[1].enabled = true;
    }
    private void ColliderDisabled()
    {
        _stats.Colliders[0].enabled = false;
        _stats.Colliders[1].enabled = false;
    }
    
    public void PlayerAtkStart()
    {
        ColliderEnabled();
    }
    public void PlayerAtkEnd()
    {
        ColliderDisabled();
    }
}
