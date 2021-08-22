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

    bool grounded = false;

	void Start ()
    {
        hoverBody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        checkGrounded();
        heightUp();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        InputMovement(vertical, horizontal);
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

    public void InputMovement(float forward, float side)
    {
        if (grounded || !BlockAirControl)
        {
            hoverBody.AddRelativeForce(Vector3.forward * forward * ForwardForce, ForceMode.Force);
            hoverBody.AddRelativeTorque(Vector3.up * RotationTorque * side * (forward == 0 ? 1f : Mathf.Sign(forward)), ForceMode.Force);
        }
    }
}
