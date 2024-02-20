using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private Collider[] _colliders;
    private FSMEnemy _enemy;
    //private FSMPlayer _player;

    private void Start()
    {
        _enemy = GetComponent<FSMEnemy>();
        _colliders = _enemy.Colliders;
        //_player = GameManager.Instance.Player.GetComponent<FSMPlayer>();
    }

    private void ColliderEnabled()
    {
        for (int i = 0; i < _colliders.Length; ++i)
        {
            _colliders[i].enabled = true;
        }
    }
    private void ColliderDisabled()
    {
        for (int i = 0; i < _colliders.Length; ++i)
        {
            _colliders[i].enabled = false;
        }
    }
    
    public void MinionAtkStart()
    {
        ColliderEnabled();
    }
    public void MinionAtkEnd()
    {
        ColliderDisabled();
    }
    public void BossAtkStart()
    {
        ColliderEnabled();
    }
    public void BossAtkEnd()
    {
        ColliderDisabled();
    }
}
