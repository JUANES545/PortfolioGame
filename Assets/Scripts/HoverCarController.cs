using System;
using UnityEngine;
using System.Collections;

public class HoverCarController : MonoBehaviour {

    [SerializeField] private float speed = 90f;
    [SerializeField] private float hoverForce = 65f;
    [SerializeField] private float hoverHeight = 3.5f;
    private float _vectorV;
    private float _vectorH;
    
    [SerializeField] private float RotationSpeed;
    [SerializeField] private Camera Camera;
    [SerializeField] private Rigidbody sphereRB;
    [SerializeField] private Rigidbody carRigidbody;
    
    private void Start()
    {
        sphereRB.transform.parent = null;
        carRigidbody.transform.parent = null;
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
        
        var ray = new Ray (transform.position, -transform.up);

        if (Physics.Raycast(ray, out var hit, hoverHeight))
        {
            var proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            var appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            sphereRB.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }

        sphereRB.AddRelativeForce(0f, 0f, _vectorV * speed);
        sphereRB.AddRelativeForce(_vectorH * speed, 0, 0);

    }
}