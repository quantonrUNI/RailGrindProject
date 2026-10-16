using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    public int jump = 250;
    public InputActionReference move;
    private Vector2 moveDirection;
    private bool isJump = false;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 8;
        rb.maxAngularVelocity = 8;
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity + new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
    }

    private void OnCollisionEnter (Collision collide)
    {
        if (collide.gameObject.name == "Ground")
        {
            isJump = true;
        }
        if (collide.gameObject.name == "LongStraightRail")
        {
            isJump = true;
        }
    }

    private void OnJump()
    {
        if (isJump)
        {
            isJump = false;
            rb.AddForce(new Vector3(0, 1 * jump, 0));
        }
    }
}
