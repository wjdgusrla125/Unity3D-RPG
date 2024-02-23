using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    public float currentMP;
    public float MaxMP;

    private void Awake()
    {
        currentMP = MaxMP;
        StartCoroutine(RegenerateMP());
    }

    public void UseMana(float mana)
    {
        currentMP -= mana;

        if (IsEmptyMana())
        {
            currentMP = 0f;
        }
    }
    public void RecoveryMP(float mana)
    {
        currentMP += mana;

        if (IsFullMana())
        {
            currentMP = MaxMP;
        }
    }
    private IEnumerator RegenerateMP()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentMP += 1f;

            currentMP = Mathf.Clamp(currentMP, 0f, MaxMP);
        }
    }

    public bool IsEmptyMana()
    {
        return currentMP <= 0;
    }
    public bool IsFullMana()
    {
        return currentMP >= MaxMP;
    }
    public float MP
    {
        get { return currentMP; }
        set { currentMP = value; }
    }
    public float RemainingAmount()
    {
        return Mathf.Clamp01(currentMP / MaxMP);
    }
}
