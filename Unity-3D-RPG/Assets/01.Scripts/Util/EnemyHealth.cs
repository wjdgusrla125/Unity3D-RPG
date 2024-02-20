using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHP;
    public float MaxHP;
    private float healingRate = 1f;

    private void Awake()
    {
        currentHP = MaxHP;
    }

    public void Takedamage(float damage)
    {
        currentHP -= damage;

        if (IsDeath())
        {
            currentHP = 0.0f;
        }
    }

    public void RecoveryHP()
    {
        currentHP = MaxHP / 2;
    }

    public bool IsDeath()
    {
        return (currentHP <= 0);
    }
    public bool IsHalfHealth()
    {
        return (currentHP >= MaxHP / 2);
    }
    public float HP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
}
