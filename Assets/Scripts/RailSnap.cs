using UnityEngine;

public class RailSnap : MonoBehaviour
{
    private GameObject currentRail = null;
    private GameObject previousRail = null;
    private float totalPlayerOffset = 0.85f;
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
        if(previousRail != null)
        {
            countdownToPreviousRailReset -= Time.deltaTime;
            if(countdownToPreviousRailReset < 0)
            {
                previousRail = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        float playerOffsetX;
        float playerOffsetY;
        float playerOffsetZ;
        Debug.Log("my name is " + gameObject.name + " and I have entered " + collision.gameObject.name);
        if (collision.gameObject.tag == "Rail")
        {
            Debug.Log("snapping");
            int likelyContacts = 5;
            ContactPoint[] contactPoints = new ContactPoint[likelyContacts];
            int numOfContacts = collision.GetContacts(contactPoints);
            for(int i = 0; i < numOfContacts; i++)
            {
                GameObject possibleRail = contactPoints[i].otherCollider.gameObject;
                if (possibleRail.tag == "Rail" && possibleRail != currentRail && possibleRail != previousRail)
                {
                    playerOffsetX = totalPlayerOffset * possibleRail.transform.up.x;
                    playerOffsetY = totalPlayerOffset * possibleRail.transform.up.y;
                    playerOffsetZ = totalPlayerOffset * possibleRail.transform.up.z;

                    transform.position = new Vector3(contactPoints[i].point.x + playerOffsetX, contactPoints[i].point.y + playerOffsetY, contactPoints[i].point.z + playerOffsetZ);
                    if(currentRail != null)
                    {
                        previousRail = currentRail;
                        movementScript.SetPreviousRailSpeed();
                        countdownToPreviousRailReset = targetTimeForPreviousRailReset;
                    }
                    currentRail = collision.gameObject;
                    movementScript.JustOnRail(collision.gameObject);
                    float lowPitch = 0.6f;
                    float highPitch = 1.4f;
                    source.Play();
                    source.pitch = Random.Range(lowPitch, highPitch);
                    break;
                }
            }
            //transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + PlayerOffset, collision.transform.position.z);
        }
    }



    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == currentRail)
        {
            currentRail = null;
            source.Stop();
        }

    }
}
