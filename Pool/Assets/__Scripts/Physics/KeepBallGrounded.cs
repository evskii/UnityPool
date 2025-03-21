using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBallGrounded : MonoBehaviour
{
    private Rigidbody ballRb;

    private void Start() {
        ballRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector3 velocity = ballRb.velocity;
        if (velocity.y > 0) {
            velocity.y = 0;
            ballRb.velocity = velocity;
        }
    }
}
