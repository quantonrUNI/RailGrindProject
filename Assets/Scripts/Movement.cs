using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    [SerializeField] float deceleration = 0.9f;
    public int jumpAmount = 300;
    public InputActionReference move;
    private Vector2 moveDirection;
    private bool isJump = false;
    Rigidbody rb;
    private RailSnap railSnapScript;

    private void Start()
    {
        railSnapScript = GetComponent<RailSnap>();
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 8;
        rb.maxAngularVelocity = 8;
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }

    float railSpeed;
    const float minimumRailSpeed = 5;
    Vector3 direction;
    public void JustOnRail(GameObject rail)
    {
        Debug.Log("just on the rail");
        railSpeed = (Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.z)); /// 2;
        if(railSpeed < minimumRailSpeed) //provides a minimum rail speed
        {
            railSpeed = 5;
        }
        direction = rail.transform.forward;

        float xVelocity = rb.linearVelocity.normalized.x;
        float zVelocity = rb.linearVelocity.normalized.z;

        Debug.Log("if(" + direction.x + " - " + xVelocity + ") + (" + direction.z + " - " + zVelocity + ") > (" + -direction.x + " - " + xVelocity + ") + (" + -direction.z + " - " + zVelocity + ")");
        if (Mathf.Abs(direction.x - xVelocity) + Mathf.Abs(direction.z - zVelocity) > Mathf.Abs(-direction.x - xVelocity) + Mathf.Abs(-direction.z - zVelocity)) //if the player is going in the same direction that points backwards from the rails local axis
        {
            direction = -direction;
            Debug.Log("true, reversing direction");
        } 
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            GameObject currentRail = railSnapScript.GetCurrentRail();
            if (currentRail != null)
            {
                Debug.Log("direction x: " + direction.x + " y: " + direction.y + " z: " + direction.z + " railSpeed: " + railSpeed);
                rb.linearVelocity = direction * railSpeed;
            }
            else
            {
                if (moveDirection.y == 0 && moveDirection.x == 0)
                {
                    rb.linearVelocity = rb.linearVelocity * deceleration;
                    //Debug.Log(moveDirection.x + " is movedirection.x, " + moveDirection.y + " is movedirection.y, " + rb.linearVelocity.x + " is rb.linearVelocity.x, " + rb.linearVelocity.z + " is rb.linearVelocity.z, ");
                }
                else
                {
                    rb.linearVelocity = rb.linearVelocity + new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
                    //Debug.Log(moveDirection.x + " is movedirection.x, " + moveDirection.y + " is movedirection.y, " + rb.linearVelocity.x + " is rb.linearVelocity.x, " + rb.linearVelocity.z + " is rb.linearVelocity.z, ");
                }
            }
            
        }
        else
        {
            rb.linearVelocity = rb.linearVelocity + new Vector3(moveDirection.x * speed , 0, moveDirection.y * speed);
            //Debug.Log(moveDirection.x + " is movedirection.x, " + moveDirection.y + " is movedirection.y, " + rb.linearVelocity.x + " is rb.linearVelocity.x, " + rb.linearVelocity.z + " is rb.linearVelocity.z, ");
        }
    }

    private void OnCollisionEnter (Collision collide)
    {
        if (collide.gameObject.tag == "Ground" || collide.gameObject.tag == "Rail")
        {
            isJump = true;
        }
    }

    private void OnJump()
    {
        if (isJump)
        {
            isJump = false;
            rb.AddForce(new Vector3(0, jumpAmount, 0));
        }
    }
}
