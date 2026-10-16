using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public int speed = 15;
    public int jump = 200;
    public InputActionReference move;
    private Vector2 moveDirection;
    Rigidbody rb;
    public bool OnRail = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Debug.Log("on rail = " + OnRail);
        if (!OnRail)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
        }
        else
        {
            rb.linearVelocity.Set(0f, 0f, 0f);
        }
    }
}
