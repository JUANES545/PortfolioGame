using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

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
    [SerializeField] private float jumpForce = 20000f;
    [SerializeField] private float JumpRateTime = 0.4f; 
    private const float TurboForce = 2.5f;

    private Rigidbody hoverBody;
    private bool jumpLoaded = true;
    private float horizontal;
    private float vertical;
    private float inputX;
    private float inputY;
    
    
    public float turboFactor;

    private bool _grounded;
    
    private void Start()
    {
        Application.targetFrameRate = 60;
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
        InputMovement(inputY, inputX);
    }

    private void CheckGrounded()
    {
        var ray = new Ray(CenterRaycastHelper.position, -CenterRaycastHelper.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, GroundedThreshold, GroundLayer))
        {
            _grounded = true;
            hoverBody.drag = 1.5f;
        }
        else
        {
            _grounded = false;
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
        if (_grounded || !BlockAirControl)
        {
            hoverBody.AddRelativeForce(Vector3.forward * forward * turboFactor, ForceMode.Force);
            
        }
    }

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

    public void TurboBoost(InputAction.CallbackContext context)
    {
        if (context.started) {
            Turbo(true);
        }else if (context.canceled) {
            Turbo(false);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed || !jumpLoaded) return;
        jumpLoaded = false;
        hoverBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(ReloadJump());
    }
    
    private IEnumerator ReloadJump()
    {
        yield return new WaitForSeconds(JumpRateTime);
        jumpLoaded = true;
    }
}