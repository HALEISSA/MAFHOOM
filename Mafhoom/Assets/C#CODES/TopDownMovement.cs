using Photon.Pun;
using UnityEngine;

public class TopDownMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer sr;

    private float lastMoveX = 1f; // ⭐ remembers last direction

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            movement.y = 1;

        if (Input.GetKey(KeyCode.S))
            movement.y = -1;

        if (Input.GetKey(KeyCode.A))
            movement.x = -1;

        if (Input.GetKey(KeyCode.D))
            movement.x = 1;

        movement = movement.normalized;

        // ⭐ Save last direction
        if (movement.x != 0)
        {
            lastMoveX = movement.x;
        }

        // ⭐ Flip based on last direction (even when idle)
        if (lastMoveX < 0)
            sr.flipX = true;
        else if (lastMoveX > 0)
            sr.flipX = false;

        // ⭐ Animation
        bool isMoving = movement.x != 0 || movement.y != 0;

        if (animator != null)
        {
            animator.SetBool("isRunning", isMoving);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed - interact button");
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        rb.velocity = movement * moveSpeed;
    }

    private void OnDisable()
    {
        if (rb != null)
            rb.velocity = Vector2.zero;
    }
}