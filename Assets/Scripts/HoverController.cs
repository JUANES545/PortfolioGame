﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class HoverController : MonoBehaviour
{
    public float GoUpForce = 12500f;
    public float ForwardForce = 20000f;
    public float RotationTorque = 10000f;
    public Transform[] RaycastHelpers;
    public Transform CenterRaycastHelper;
    public float HeightFromGround = 2f;
    public float GroundedThreshold = 4f;
    public bool BlockAirControl;

    public LayerMask GroundLayer;

    Rigidbody hoverBody;

    private float horizontal;
    private float vertical;
    [SerializeField] private float eulerAngX;
    [SerializeField] private float eulerAngY;
    [SerializeField] private float eulerAngZ;
    [SerializeField] private bool saveZone;
    [SerializeField] private bool handBreak;
    [SerializeField] private Vector3 to3;
    
    private float inputX;
    private float inputY;
    
    [SerializeField] private const float TurboForce = 2.5f;
    public float turboFactor;

    bool grounded = false;

    private void Start()
    {
        hoverBody = GetComponent<Rigidbody>();
        turboFactor = ForwardForce;
    }

    private void Update()
    {
        CheckGrounded();
        HeightUp();
    }
    

    private void FixedUpdate()
    {
        /*     Experimental
         var targetVector = new Vector3(horizontal, 0, vertical);
        AlwaysDown(eulerAngX, targetVector);
        
        AlwaysDown(eulerAngX, eulerAngY, eulerAngZ, targetVector);
        InputMovement(vertical, horizontal, targetVector);
        */
        InputMovement(inputY, inputX);

    }

    private void CheckGrounded()
    {
        var ray = new Ray(CenterRaycastHelper.position, -CenterRaycastHelper.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, GroundedThreshold, GroundLayer))
        {
            grounded = true;
            hoverBody.drag = 1.5f;
        }
        else
        {
            grounded = false;
            hoverBody.drag = 0.2f;
        }
    }

    private void HeightUp()
    {
        foreach (var raycastHelper in RaycastHelpers)
        {
            var ray = new Ray(raycastHelper.position, -raycastHelper.up);

            if (!Physics.Raycast(ray, out var hitInfo, HeightFromGround, GroundLayer)) continue;
            var distance = Vector3.Distance(raycastHelper.position, hitInfo.point);

            if (distance < HeightFromGround)
            {
                hoverBody.AddForceAtPosition(raycastHelper.up * GoUpForce * (1f - distance / HeightFromGround),
                    raycastHelper.position, ForceMode.Force);
            }
        }
    }

    private void InputMovement(float forward, float side)
    {
        hoverBody.AddRelativeTorque(
            Vector3.up * (RotationTorque * side * (forward == 0 ? 1f : Mathf.Sign(forward))), ForceMode.Force);
        if (grounded || !BlockAirControl)
        {
            hoverBody.AddRelativeForce(Vector3.forward * forward * turboFactor, ForceMode.Force);
            
        }
    }

    private void AlwaysDown(float eulerAngX, Vector3 targetVector)
    {
        if (eulerAngX > 300 || eulerAngX == 0  || eulerAngX < 37)
        {
            saveZone = true;
            
        }
        else
        {
            saveZone = false;
            to3 = Vector3.Scale(transform.localEulerAngles, new Vector3(0,1,0));
            gameObject.transform.Rotate(to3);            
            //to3 = transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0, 1, 1), Time.deltaTime);
        }
        
        //to3 = Vector3.Scale(transform.eulerAngles, new Vector3(0,1,1));
    } //Experimental
    
    public void Move(InputAction.CallbackContext context){
        inputX = context.ReadValue<Vector2>().x;
        inputY = context.ReadValue<Vector2>().y;
    }

    private void Turbo(bool turbo)
    {
        switch (turbo)
        {
            case true:
                turboFactor *= TurboForce;
                break;
            case false:
                turboFactor = ForwardForce;
                break;
        }
    }

    public void HandBreak(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            handBreak = true;
        }else if (context.canceled && grounded)
        {
            handBreak = false;
        }
    }

    public void TurboBoost(InputAction.CallbackContext context)
    {
        if (context.started) {
            Turbo(true);
        }else if (context.canceled) {
            Turbo(false);
        }
    }
}