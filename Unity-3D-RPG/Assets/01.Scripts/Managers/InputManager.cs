using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private Vector2 moveInput = Vector2.zero;
    private Vector3 moveRotation;
    private bool isInput;

    private Vector3 rotateInput;

    protected Vector3 camInput;
    protected bool isCamInput;

    public Vector2 MoveInput
    {
        get { return moveInput; }
        set => moveInput = value;
    }

    public Vector3 MoveRotation
    {
        get { return moveRotation; }
        set => moveRotation = value;
    }
    
    public bool IsInput
    {
        get { return isInput; }
        set => isInput = value;
    }

    
    
}
