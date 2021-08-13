using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;

    [SerializeField] private float MovementSpeed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private Camera Camera;
    
    
    public Rigidbody sphereRB;
    public Rigidbody carRB;
    
    public LayerMask groundLayer;
    [SerializeField] private bool isCarGrounded;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    
    private void Start()
    {
        // Detach Sphere from car
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;
    }
    
    private void Update()
    {
        // Set Cars Position to Our Sphere
        transform.position = sphereRB.transform.position;
        // Raycast to the ground and get normal to align car with it.
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);

        RotateTowardMovementVector(movementVector);
    }

    private void FixedUpdate()
    {
        carRB.MoveRotation(transform.rotation);
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = MovementSpeed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;

        if (isCarGrounded)
            sphereRB.AddForce(targetVector * speed, ForceMode.Acceleration); // Add Movement
        else
            sphereRB.AddForce(transform.up * -50f); // Add Gravity
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if(movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }
}
