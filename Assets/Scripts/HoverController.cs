using System;
using UnityEngine;
using System.Collections;

public class HoverController : MonoBehaviour
{
    [SerializeField] private float GoUpForce = 12500f;
    [SerializeField] private float ForwardForce = 20000f;
    [SerializeField] private float RotationTorque = 10000f;
    [SerializeField] private Transform[] RaycastHelpers;
    [SerializeField] private Transform CenterRaycastHelper;
    [SerializeField] private float HeightFromGround = 2f;
    [SerializeField] private float GroundedThreshold = 4f;
    [SerializeField] private bool BlockAirControl;

    [SerializeField] private LayerMask GroundLayer;

    private float horizontal;
    private float vertical;

    Rigidbody hoverRB;

    bool grounded;

    private void Start ()
    {
        hoverRB = GetComponent<Rigidbody>();
	}

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate ()
    {
        checkGrounded();
        heightUp();
        
        InputMovement(vertical, horizontal);
	}

    void checkGrounded()
    {
        var ray = new Ray(CenterRaycastHelper.position, -CenterRaycastHelper.up);
        RaycastHit hitInfo;

        grounded = Physics.Raycast(ray, out hitInfo, GroundedThreshold, GroundLayer);
    }

    void heightUp()
    {
        foreach (var raycastHelper in RaycastHelpers)
        {
            var ray = new Ray(raycastHelper.position, -raycastHelper.up);

            if (!Physics.Raycast(ray, out var hitInfo, HeightFromGround, GroundLayer)) continue;
            var distance = Vector3.Distance(raycastHelper.position, hitInfo.point);

            if(distance < HeightFromGround)
            {
                hoverRB.AddForceAtPosition(raycastHelper.up * GoUpForce * (1f - distance / HeightFromGround), 
                    raycastHelper.position, ForceMode.Force);
            }
        }
    }

    private void InputMovement(float forward, float side)
    {
        if (!grounded && BlockAirControl) return;
        hoverRB.AddRelativeForce(Vector3.forward * forward * ForwardForce, ForceMode.Force);
        hoverRB.AddRelativeTorque(Vector3.up * RotationTorque * side * (forward == 0 ? 1f : Mathf.Sign(forward)), ForceMode.Force);
    }
}
