using System;
using UnityEngine;
using System.Collections;

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

    bool grounded = false;

	void Start ()
    {
        hoverBody = GetComponent<Rigidbody>();
	}

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate ()
    {
        checkGrounded();
        heightUp();
        var targetVector = new Vector3(horizontal, 0, vertical);
        
        InputMovement(vertical, horizontal, targetVector);
        alwaysDown();
	}

    void checkGrounded()
    {
        Ray ray = new Ray(CenterRaycastHelper.position, -CenterRaycastHelper.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, GroundedThreshold, GroundLayer))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void heightUp()
    {
        foreach (Transform raycastHelper in RaycastHelpers)
        {
            Ray ray = new Ray(raycastHelper.position, -raycastHelper.up);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, HeightFromGround, GroundLayer))
            {
                float distance = Vector3.Distance(raycastHelper.position, hitInfo.point);

                if(distance < HeightFromGround)
                {
                    hoverBody.AddForceAtPosition(raycastHelper.up * GoUpForce * (1f - distance / HeightFromGround), raycastHelper.position, ForceMode.Force);
                }
            }
        }
    }

    private void InputMovement(float forward, float side, Vector3 targetVector)
    {
        if (grounded || !BlockAirControl)
        {
            //hoverBody.AddRelativeForce(Vector3.forward * forward * ForwardForce, ForceMode.Force);
            hoverBody.AddRelativeForce(Vector3.forward * forward * ForwardForce, ForceMode.Acceleration);
            hoverBody.AddRelativeTorque(
                Vector3.up * (RotationTorque * side * (forward == 0 ? 1f : Mathf.Sign(forward))), ForceMode.Force);
        }
    }

    private void alwaysDown()
    {
        if (gameObject.transform.rotation.x > 34 || gameObject.transform.rotation.x < -34)
        {
            Debug.Log("doggy");
            gameObject.transform.Rotate(0,180,0);
        }
    }
}


/*
    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        //targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;

        if (grounded || !BlockAirControl)
        {
            //hoverRB.AddForce(targetVector * ForwardForce, ForceMode.Acceleration);
            hoverRB.AddRelativeForce(Vector3.forward * targetVector.z * ForwardForce, ForceMode.Force);
        }
        return targetVector;
    }
    
    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if(movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
    }
*/
