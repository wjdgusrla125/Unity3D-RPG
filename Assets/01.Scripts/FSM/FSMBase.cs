using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBase : MonoBehaviour
{
    public CharacterController _charactercontroller;
    public Animator _animator;

    public CH_STATE CHState;
    public bool isNewState;

    public CharacterState State;

    protected virtual void Awake()
    {
        _charactercontroller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        State = GetComponent<CharacterState>();
    }

    protected virtual void OnEnable()
    {
        CHState = CH_STATE.IDLE;
        StartCoroutine(FSMMain());
    }

    public void SetState(CH_STATE newState)
    {
        isNewState = true;
        CHState = newState;
        
        _animator.SetInteger("State", (int)CHState);
    }

    protected virtual IEnumerator FSMMain()
    {
        while (true)
        {
            isNewState = false;
            yield return StartCoroutine(CHState.ToString());//CHState.XXX 코루틴을 실행
        }
    }

    protected virtual IEnumerator IDLE()
    {
        do
        {
            yield return null;
        } while (!isNewState);
    }
}
