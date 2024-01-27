using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;
    private Vector2 mvmtInput;
    private Vector2 smoothedMvmt;
    private Vector2 smoothedVelocity;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        SetPlayerVelocity();
        SetAnimation();
    }

    private void SetAnimation()
    {
        bool isWalking = mvmtInput != Vector2.zero;

        animator.SetBool("isWalking", isWalking);
    }

    private void SetPlayerVelocity()
    {
        //Helps make stop/start movement smoother
        smoothedMvmt = Vector2.SmoothDamp(
                    smoothedMvmt,
                    mvmtInput,
                    ref smoothedVelocity,
                    0.1f);

        rb.velocity = smoothedMvmt * speed;
    }

    private void OnMove(InputValue inputValue)
    {
        mvmtInput = inputValue.Get<Vector2>();
    }
}
