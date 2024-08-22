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
        // Variable with data to be transferred to the camera so it is always positioned at the lowest point the ball has reached.
        // This ensures that the camera will always show a clear overview of the level and player.
        // TODO Check later with 2 balls if this is still a viable option.
        if (transform.position.y < LowestY)
        {
            LowestY = transform.position.y;
        }
    }

    // Custom made bounce modified by the ball's mass.
    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision) return;

        // Start the level reset if the player has hit a kill part.
        KillSlice _killPart = collision.transform.GetComponent<KillSlice>();
        if (_killPart) 
        {
            _killPart.KillPartHit();
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreCollider"))
        {
            GameManager.Instance.AddScore(3);
            other.GetComponent<Collider>().enabled = false;
        }
        else if (other.CompareTag("Powerup"))
        {
            StartCoroutine(other.GetComponent<Powerup>().ActivatePowerup());
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
