using System;
using UnityEngine;
using System.Collections;

public class HoverCarController : MonoBehaviour {

    [SerializeField] private float speed = 90f;
    [SerializeField] private float hoverForce = 65f;
    [SerializeField] private float hoverHeight = 3.5f;
    [SerializeField] private float rotationSpeed = 2f;

    [SerializeField] private Camera Camera;
    [SerializeField] private Rigidbody sphereRB;
    [SerializeField] private Rigidbody carRigidbody;
    
    private float _vectorV;
    private float _vectorH;
    
    private void Start()
    {
        sphereRB.transform.parent = null;
        //carRigidbody.transform.parent = null;
    }

    private void Update () 
    {
        _vectorV = Input.GetAxis ("Vertical");
        _vectorH = Input.GetAxis ("Horizontal");
    }

    private void FixedUpdate()
    {
        // Set Cars Position to Our Sphere
        transform.position = sphereRB.transform.position;
        //carRigidbody.MoveRotation(transform.rotation);

        var targetVector = new Vector3(_vectorH, 0, _vectorV);
        var movementVector = MoveTowardTarget(targetVector);
        
        RotateTowardMovementVector(movementVector);
        
    }
    
    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        //var speed = this.speed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, 
            Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        
        //*sphereRB.AddForce(targetVector * speed, ForceMode.Acceleration); // Add Movement
        
        var ray = new Ray (transform.position, -transform.up);
        if (Physics.Raycast(ray, out var hit, hoverHeight))
        {
            var proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            var appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            sphereRB.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }

        sphereRB.AddRelativeForce(targetVector * speed, ForceMode.Acceleration);
        return targetVector;
    }
    
    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if(movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
    }
}