using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private bool ignoreNextCollision;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Custom made bounce modified by the ball's mass.
    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision) return;

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up, ForceMode.Impulse);

        ignoreNextCollision = true;
        Invoke(nameof(AllowCollision), 0.2f);
    }

    // Prevent the ball from colliding with 2 platforms at the same time, resulting in a bounce twice as high.
    // This can happen when the ball bounces exactly on the border of 2 platforms.
    private void AllowCollision() 
    { 
        ignoreNextCollision = false; 
    }
}
