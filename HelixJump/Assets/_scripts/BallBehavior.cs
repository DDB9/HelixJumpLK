using UnityEngine;
using System;

public class BallBehavior : MonoBehaviour
{
    [NonSerialized] public float LowestY;
    [NonSerialized] public bool ShieldActive;
    [NonSerialized] public bool IsFinished = false;

    private Rigidbody rb;
    private bool ignoreNextCollision;
    private Vector3 startPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        LowestY = transform.position.y;
    }

    private void Update()
    {
        // Variable with data to be transferred to the camera so it is always positioned at the lowest point of the ball that is currently highest up.
        // This ensures that the player will not loose sight of the ball that is behind, making it impossible to move down with said ball.
        if (transform.position.y < LowestY)
        {
            LowestY = transform.position.y;
        }
        transform.LookAt(Camera.main.transform.position);

        // Enable the shield sprite once if the shield is active and disable it once when not. 
        // Doing it once increases performance.
        if (ShieldActive && !transform.GetChild(0).GetComponent<SpriteRenderer>().enabled)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (!ShieldActive && transform.GetChild(0).GetComponent<SpriteRenderer>().enabled)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Custom made bounce modified by the ball's mass.
    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision) return;

        // Start the level reset if the player has hit a kill part.
        KillSlice _killSlice = collision.transform.GetComponent<KillSlice>();
        if (_killSlice && !ShieldActive)
        {
            _killSlice.KillPartHit();
        }
        else if (_killSlice && ShieldActive)
        {
            ShieldActive = false;
        }

        // bounce... bounce... bounce...
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up, ForceMode.Impulse);

        if (!IsFinished)
        {
            ignoreNextCollision = true;
            Invoke(nameof(AllowCollision), 0.2f);
        }

        // If the ball reaches the end, advance to the next level.
        if (collision.transform.CompareTag("finish"))
        {
            IsFinished = true;
            ignoreNextCollision = true;
        }
    }

    // If the ball passes through a score collider, add score.
    // Else if the ball passes through a powerup, active that powerup.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreCollider"))
        {
            GameManager.Instance.AddScore(3);
            other.GetComponent<Collider>().enabled = false;
        }
        else if (other.CompareTag("Powerup"))
        {
            StartCoroutine(other.GetComponentInParent<Powerup>().ActivatePowerup());
        }
    }

    /// <summary>
    /// Prevent the ball from colliding with 2 platforms at the same time, resulting in a bounce twice as high.
    /// </summary>
    // This can happen when the ball bounces exactly on the border of 2 platforms.
    public void AllowCollision()
    {
        ignoreNextCollision = false;
    }

    /// <summary>
    /// Resets the ball, as well as the lowest Y position for the camera.
    /// </summary>
    public void ResetBall()
    {
        transform.position = startPosition;
        LowestY = transform.position.y;
    }
}
