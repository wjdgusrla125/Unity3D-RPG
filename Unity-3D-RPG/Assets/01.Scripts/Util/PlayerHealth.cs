using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHP;
    public float MaxHP;
    public int LifeCount;

    private void Awake()
    {
        currentHP = MaxHP;
        LifeCount = 3;
        StartCoroutine(RegenerateHP());
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        
        if (IsDeath())
        {
            currentHP = 0.0f;
        }
    }
    public void RecoveryHP(float hp)
    {
        currentHP += hp;

        if (IsFullHealth())
        {
            currentHP = MaxHP;
        }
    }
    private IEnumerator RegenerateHP()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            currentHP += 3f;

            currentHP = Mathf.Clamp(currentHP, 0f, MaxHP);
        }
    }

    public bool IsDeath()
    {
        return currentHP <= 0;
    }
    public bool IsFullHealth()
    {
        return currentHP >= MaxHP;
    }
    public float HP
    {
        get { return currentHP;}
        set { currentHP = value; }
    }
    public float RemainingAmount()
    {
        return Mathf.Clamp01(currentHP / MaxHP);
    }
}
