using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Vector3 velocity;
    [SerializeField] private float magnitude;

    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private float angularVelocityMag;

    private void Start() {
        rb = GetComponent<Rigidbody>();

    }

    private void Update() {
        velocity = rb.velocity;
        magnitude = rb.velocity.magnitude;

        angularVelocity = rb.angularVelocity;
        angularVelocityMag = rb.angularVelocity.magnitude;
    }
}
