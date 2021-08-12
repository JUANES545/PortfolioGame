using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 InputVector { get; private set; }
    private Vector3 _moveDirection;
    
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
    }


    public void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector3>();
        InputVector = new Vector2(_moveDirection.x, _moveDirection.y);
    }

}
