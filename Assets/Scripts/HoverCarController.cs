using System;
using UnityEngine;
using System.Collections;

public class HoverCarController : MonoBehaviour {

    public float speed = 90f;
    public float turnSpeed = 5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
    private float powerInput;
    private float turnInput;
    public Rigidbody carRigidbody;


    private void Awake () 
    {
        carRigidbody = GetComponent <Rigidbody>();
    }

    private void Start()
    {
        carRigidbody.transform.parent = null;
    }

    private void Update () 
    {
        powerInput = Input.GetAxis ("Vertical");
        turnInput = Input.GetAxis ("Horizontal");
    }

    private void FixedUpdate()
    {
        transform.position = carRigidbody.transform.position;
        Ray ray = new Ray (transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }

        carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed);
        carRigidbody.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);

    }
}