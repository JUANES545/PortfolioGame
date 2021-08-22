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
    public GameObject c;

    bool grounded = false;

    private void Start()
    {
        hoverBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        checkGrounded();
        heightUp();
        AlwaysDown();
        var targetVector = new Vector3(horizontal, 0, vertical);

        InputMovement(vertical, horizontal, targetVector);

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

            if (Physics.Raycast(ray, out hitInfo, HeightFromGround, GroundLayer))
            {
                float distance = Vector3.Distance(raycastHelper.position, hitInfo.point);

                if (distance < HeightFromGround)
                {
                    hoverBody.AddForceAtPosition(raycastHelper.up * GoUpForce * (1f - distance / HeightFromGround),
                        raycastHelper.position, ForceMode.Force);
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

    private void AlwaysDown()
    {
        Debug.Log("Doggy");
        Debug.Log(c.transform.localRotation.eulerAngles.x);
        //Debug.Log("Doggy");
        /*if (gameObject.transform.localRotation.eulerAngles.x > 34 || gameObject.transform.localRotation.eulerAngles.x < -34)
        {
            Debug.Log("doggy");
            //gameObject.transform.Rotate(0,180,0);
        }*/
    }
}