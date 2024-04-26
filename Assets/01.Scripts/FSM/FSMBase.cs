using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FSMBase : MonoBehaviour
{
    [Header("FSMBase")]
    public CharacterController characterController;
    public Animator animator;
    public CH_STATE CHState;
    public CharacterState characterState;
    public bool IsNewState;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterState = GetComponent<CharacterState>();
    }

    protected virtual void OnEnable()
    {
        CHState = CH_STATE.IDLE;
        StartCoroutine(FSMMain());
    }

    public void SetState(CH_STATE newState)
    {
        IsNewState = true;
        CHState = newState;
        
        animator.SetInteger("State", (int)CHState);
    }

    protected virtual IEnumerator FSMMain()
    {
        while (true)
        {
            IsNewState = false;
            yield return StartCoroutine(CHState.ToString());//CHState.XXX 코루틴을 실행
        }
    }

    protected virtual IEnumerator IDLE()
    {
        do
        {
            yield return null;
        } while (!IsNewState);
    }
}
