using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bonusPhysics;

public class BoatController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform sails;
    [SerializeField] private Transform rudder;
    [SerializeField] private float steerPower;
    [SerializeField] private float speed;
    [SerializeField] private float backSpeed;
    [SerializeField] private float maxSpeed;

    private Quaternion StartRot;
    private Vector3 Velocity;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        StartRot = rudder.localRotation;
    }

    private void FixedUpdate()
    {
        var forceDirection = transform.forward;
        var steer = 0;

        if (Input.GetKey(KeyCode.A))
        {
            steer = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            steer = -1;
        }

        rb.AddForceAtPosition(steer * transform.right * steerPower/10, rudder.position);

        //var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);

        if (Input.GetKey(KeyCode.W)) 
        {
            
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForceAtPosition((speed * -transform.up * speed / 10) * Time.deltaTime, sails.position);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            
        }

    }
}
