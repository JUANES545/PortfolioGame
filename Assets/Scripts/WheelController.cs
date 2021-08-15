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
    private static readonly int GoingLeft = Animator.StringToHash("goingLeft");
    private static readonly int GoingRight = Animator.StringToHash("goingRight");

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
        var verticalAxis = Input.GetAxisRaw("Vertical");
        var horizontalAxis = Input.GetAxisRaw("Horizontal");
        //CurrentWheelVelocity = _RB.sphereRB.velocity.magnitude;
        
        foreach (var wheel in wheelsToRotate)
            wheel.transform.Rotate(Time.deltaTime * CurrentWheelVelocity * 
                                   rotationSpeed,0,0, Space.Self);
        

        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            foreach (var trail in trails)
            {
                trail.emitting = true; 
            }
        }
        else
        {
            foreach (var trail in trails)
            {
                trail.emitting = false;
            }
        }
    }
}
