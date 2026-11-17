using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    [SerializeField] float deceleration = 0.9f;
    public int jumpAmount = 600;
    public InputActionReference move;
    private Vector2 moveDirection;
    private Vector3 refForward;
    private Vector3 refRight;
    private bool isJump = false;
    Rigidbody rb;
    private RailSnap railSnapScript;
    public GameObject camera;

    private void Start()
    {
        railSnapScript = GetComponent<RailSnap>();
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 8;
        rb.maxAngularVelocity = 8;
    }

    private void Update()
    {
        // grabs the forward and right of the main camera for later use
        refForward = camera.transform.forward;
        refRight = camera.transform.right;
        // initializes the Vector2 for the player's movement that is used later
        moveDirection = moveDirection = new Vector2(0,0);
        // for each input that doesn't counter act each other (W & S etc.) sets the movement
        // direction vector to the appropriate vector2 from the componets of the camera's vector3
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection = new Vector2(refForward.x, refForward.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = new Vector2(-refRight.x, -refRight.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection = new Vector2(-refForward.x, -refForward.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection = new Vector2(refRight.x, refRight.z);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            moveDirection = (new Vector2(-refRight.x, -refRight.z) + new Vector2(refForward.x, refForward.z)) / 2;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            moveDirection = (new Vector2(refRight.x, refRight.z) + new Vector2(refForward.x, refForward.z)) / 2;
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            moveDirection = (new Vector2(-refRight.x, -refRight.z) + new Vector2(-refForward.x, -refForward.z)) / 2;
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            moveDirection = (new Vector2(refRight.x, refRight.z) + new Vector2(-refForward.x, -refForward.z)) / 2;
        }
    }

    float railSpeed;
    const float minimumRailSpeed = 5;
    float previousRailSpeed = minimumRailSpeed;
    Vector3 direction;
    public void JustOnRail(GameObject rail)
    {
        Debug.Log("just on the rail");
        railSpeed = (Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.z)); //normalise the speed of the player into one positive number
        Debug.Log("railspeed: " +  railSpeed + " & previousRailSpeed: " + previousRailSpeed + " & countdownToPreviousRailReset: " + railSnapScript.countdownToPreviousRailReset);
        if(railSpeed < minimumRailSpeed) //provides a minimum rail speed
        {
            railSpeed = minimumRailSpeed;
        }
        else if(railSpeed < previousRailSpeed && railSnapScript.countdownToPreviousRailReset > 0) //if the previous railspeed is faster than the current one after just transitioning rails make the current railspeed the previous one
        {
            railSpeed = previousRailSpeed;
            Debug.Log("railSpeed = previousRailSpeed");
        }

        direction = rail.transform.forward; //the forward direction of the rail as a normalised vector pointing somewhere

        float xVelocity = rb.linearVelocity.normalized.x;
        float zVelocity = rb.linearVelocity.normalized.z;

        Debug.Log("if(" + direction.x + " - " + xVelocity + ") + (" + direction.z + " - " + zVelocity + ") > (" + -direction.x + " - " + xVelocity + ") + (" + -direction.z + " - " + zVelocity + ")");
        if (Mathf.Abs(direction.x - xVelocity) + Mathf.Abs(direction.z - zVelocity) > Mathf.Abs(-direction.x - xVelocity) + Mathf.Abs(-direction.z - zVelocity)) //if the player is going in the same direction that points backwards from the rails local axis
        {
            direction = -direction;
            Debug.Log("true, reversing direction");
        }
        rb.MoveRotation(rail.transform.rotation * Quaternion.Euler(0,90,0)); //makes the player face the direction they are going
    }

    public void SetPreviousRailSpeed()
    {
        previousRailSpeed = railSpeed;
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            GameObject currentRail = railSnapScript.GetCurrentRail();
            if (currentRail != null) //if on a rail
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
        // checks if the player is on the ground or on a rail, if so they are allowed to jump
        if (collide.gameObject.tag == "Ground" || collide.gameObject.tag == "Rail")
        {
            isJump = true;
        }
    }

    private void OnJump()
    {
        // if jump is allowed where the player currently is, sets the jump bool to false to prevent infinite jumping
        // adds a force vector upwards by the jump height value
        if (isJump)
        {
            isJump = false;
            rb.AddForce(new Vector3(0, jumpAmount, 0));
        }else if(rb.linearVelocity.y < 0) //if the player is falling
        {
            rb.linearVelocity += new Vector3(0, -1, 0);
        }
    }
}
