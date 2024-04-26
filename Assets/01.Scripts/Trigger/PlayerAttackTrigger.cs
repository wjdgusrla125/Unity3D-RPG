using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    public FSMPlayer _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _player.fsmEnemy = other.GetComponent<FSMEnemy>();
            _player.OnAttack();
        }
    }
}
