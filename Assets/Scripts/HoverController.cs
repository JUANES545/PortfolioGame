using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputHandler))]
public class HoverController : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField] private float GoUpForce = 12500f;
    [SerializeField] private float ForwardForce = 20000f;
    [SerializeField] private float RotationTorque = 10000f;
    
    [SerializeField] private float rotationSpeed = 2f;
    
    [SerializeField] private Transform[] RaycastHelpers;
    [SerializeField] private Transform CenterRaycastHelper;
    [SerializeField] private float HeightFromGround = 2f;
    [SerializeField] private float GroundedThreshold = 4f;
    [SerializeField] private bool BlockAirControl;

    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private Camera Camera;
    

    private float horizontal;
    private float vertical;
    private bool grounded;
    
    private Rigidbody hoverRB;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }

    private void Start ()
    {
        hoverRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        checkGrounded();
        heightUp();
    }

    private void FixedUpdate ()
    {
        
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        
        var movementVector = MoveTowardTarget(targetVector);
        
        RotateTowardMovementVector(movementVector);
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

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        targetVector = Quaternion.Euler(0, 
            Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;

        if (grounded || !BlockAirControl)
        {
            hoverRB.AddForce(targetVector * ForwardForce, ForceMode.Acceleration);
        }
        return targetVector;
    }
    
    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if(movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
    }
}
