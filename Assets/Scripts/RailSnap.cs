using UnityEngine;

public class RailSnap : MonoBehaviour
{
    private GameObject currentRail = null;
    private GameObject previousRail = null;
    private float totalPlayerOffset = 0.85f; //as the player origin is in the center of the capsule, adding this to the player position when they snap to a rail means they wont land inside the rail.
    private Movement movementScript;
    private AudioSource source;

    public GameObject GetCurrentRail()
    {
        return currentRail;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<Movement>();
        source = GetComponent<AudioSource>();
    }

    const float targetTimeForPreviousRailReset = 0.2f;
    public float countdownToPreviousRailReset = 0.2f;
    void Update()
    {
        if(previousRail != null) //this block of code helps stop the player from switching back and forth constantly between two rails when switching between them by implementing a timer that prevents the player from snapping to the previous rail if the timer is > 0
        {
            countdownToPreviousRailReset -= Time.deltaTime; //decreases time since last frame
            if(countdownToPreviousRailReset < 0)
            {
                previousRail = null; //empties the previous rail when been on new/no rail for a little
            }
        }
    }

    private void OnCollisionEnter(Collision collision) //collision enter used rather than trigger enter for point of contact
    {
        float playerOffsetX;
        float playerOffsetY;
        float playerOffsetZ;
        Debug.Log("my name is " + gameObject.name + " and I have entered " + collision.gameObject.name);
        if (collision.gameObject.tag == "Rail")
        {
            Debug.Log("snapping");
            int likelyContacts = 5; //the player really shouldnt be touching more than two things, a wall and a rail max usually, so 5 is enough.
            ContactPoint[] contactPoints = new ContactPoint[likelyContacts];
            int numOfContacts = collision.GetContacts(contactPoints);
            for(int i = 0; i < numOfContacts; i++)
            {
                GameObject possibleRail = contactPoints[i].otherCollider.gameObject;
                if (possibleRail.tag == "Rail" && possibleRail != currentRail && possibleRail != previousRail) //if the player has collided with a new rail not the one theyre currently on or one they were just on
                {
                    //translates the 0.85f player offset into separate pieces relative to the rails local "up" vector
                    playerOffsetX = totalPlayerOffset * possibleRail.transform.up.x;
                    playerOffsetY = totalPlayerOffset * possibleRail.transform.up.y;
                    playerOffsetZ = totalPlayerOffset * possibleRail.transform.up.z;

                    transform.position = new Vector3(contactPoints[i].point.x + playerOffsetX, contactPoints[i].point.y + playerOffsetY, contactPoints[i].point.z + playerOffsetZ); //applies the offset
                    if(currentRail != null) //if the player was just on a rail before this collision
                    {
                        previousRail = currentRail; //store the rail they were just on
                        movementScript.SetPreviousRailSpeed(); //store the speed the player was going on the previous rail to use later on the movement script
                        countdownToPreviousRailReset = targetTimeForPreviousRailReset; //reset timer
                    }
                    currentRail = collision.gameObject;
                    movementScript.JustOnRail(collision.gameObject); //call the logic to move the player on the rail
                    float lowPitch = 0.9f;
                    float highPitch = 1.1f;
                    source.Play(); //play the rail grinding sound
                    source.pitch = Random.Range(lowPitch, highPitch); //apply a random pitch shift
                    break; //we have found the rail so leave the loop
                }
            }
            //transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + PlayerOffset, collision.transform.position.z);
        }
    }



    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == currentRail) //stop the rail grind sound when hop off the rail
        {
            currentRail = null;
            source.Stop();
        }

    }
}
