using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 velocity;
    public float magnitude;
    public Vector3 angularVelocity;
    public float angularVelocityMag;

    private void Start() {
        rb = GetComponent<Rigidbody>();

    }

    private void Update() {
        velocity = rb.velocity;
        magnitude = rb.velocity.magnitude;

        angularVelocity = rb.angularVelocity;
        angularVelocityMag = rb.angularVelocity.magnitude;

        if (velocity.y > 0) {
            velocity.y = 0;
            rb.velocity = velocity;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.TryGetComponent(out BallPhysics ball)) {
            // if (magnitude >= ball.magnitude) {
            //     // Vector3 forceDir = (ball.transform.position - transform.position).normalized;
            //     ball.GetComponent<Rigidbody>().AddForce((other.impulse/Time.fixedDeltaTime) * 0.75f);
            //     rb.AddForce((other.impulse/Time.fixedDeltaTime) * -0.25f);
            // }
        }
    }
}
