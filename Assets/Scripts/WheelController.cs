using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private InputHandler _input;
    private TopDownCharacterMover _RB;
    private float CurrentWheelVelocity;
    
    public GameObject[] wheelsToRotate;
    public TrailRenderer[] trails;
    public ParticleSystem smoke;
    
    public float rotationSpeed;
    private Animator anim;
    
    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _RB = GetComponent<TopDownCharacterMover>();
    }
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        float verticalAxis = Input.GetAxisRaw("Vertical");
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        CurrentWheelVelocity = _RB.sphereRB.velocity.magnitude;
        foreach (var wheel in wheelsToRotate)
            wheel.transform.Rotate(Time.deltaTime * CurrentWheelVelocity * 
                                   rotationSpeed,0,0, Space.Self);

        /*if (horizontalAxis > 0)
        {
            //turning right
            anim.SetBool("goingLeft", false);
            anim.SetBool("goingRight", true);
        }
        else if (horizontalAxis < 0)
        {
            //turning left
            anim.SetBool("goingRight", false);
            anim.SetBool("goingLeft", true);
        }
        else
        {
            //must be going straight
            anim.SetBool("goingRight", false);
            anim.SetBool("goingLeft", false);
        }*/

        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            foreach (var trail in trails)
            {
                trail.emitting = true;
            }

            //var emission = smoke.emission;
            //emission.rateOverTime = 50f;
        }
        else
        {
            foreach (var trail in trails)
            {
                trail.emitting = false;
            }
            
            //var emission = smoke.emission;
            //emission.rateOverTime = 0f;
        }
    }
}
