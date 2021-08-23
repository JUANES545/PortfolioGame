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
    [SerializeField] private float eulerAngX;
    [SerializeField] private float eulerAngY;
    [SerializeField] private float eulerAngZ;
    [SerializeField] private bool saveZone;
    [SerializeField] private float degrees = 180;
    [SerializeField] private Vector3 to3;

    bool grounded = false;

    private void Start()
    {
        hoverBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        checkGrounded();
        heightUp();
    }
    

    private void FixedUpdate()
    {
        var targetVector = new Vector3(horizontal, 0, vertical);
        //AlwaysDown(eulerAngX, targetVector);
        
        //AlwaysDown(eulerAngX, eulerAngY, eulerAngZ, targetVector);
        InputMovement(vertical, horizontal, targetVector);

    }

    void checkGrounded()
    {
        Ray ray = new Ray(CenterRaycastHelper.position, -CenterRaycastHelper.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, GroundedThreshold, GroundLayer))
        {
            grounded = true;
            hoverBody.drag = 1.5f;
            if (Input.GetKeyDown(KeyCode.Space))
                hoverBody.drag = 2.5f;

        }
        else
        {
            grounded = false;
            hoverBody.drag = 0.2f;
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
        hoverBody.AddRelativeTorque(
            Vector3.up * (RotationTorque * side * (forward == 0 ? 1f : Mathf.Sign(forward))), ForceMode.Force);
        if (grounded || !BlockAirControl)
        {
            //hoverBody.AddRelativeForce(Vector3.forward * forward * ForwardForce, ForceMode.Force);
            hoverBody.AddRelativeForce(Vector3.forward * forward * ForwardForce, ForceMode.Force);
            
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
        
        
        
    }
}