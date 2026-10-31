using UnityEngine;

public class RailSnap : MonoBehaviour
{
    private GameObject currentRail = null;
    private GameObject previousRail = null;
    private float playerOffset = 0.85f;
    private Movement movementScript;

    public GameObject GetCurrentRail()
    {
        return currentRail;
    }

    public float GetPlayerOffsetFromRail()
    {
        return playerOffset;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<Movement>();
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
        Debug.Log("my name is " + gameObject.name + " and I have entered " + collision.gameObject.name);
        if (collision.gameObject.tag == "Rail")
        {
            Debug.Log("snapping");
            int likelyContacts = 5;
            ContactPoint[] contactPoints = new ContactPoint[likelyContacts];
            int numOfContacts = collision.GetContacts(contactPoints);
            for(int i = 0; i < numOfContacts; i++)
            {
                if (contactPoints[i].otherCollider.gameObject.tag == "Rail" && contactPoints[i].otherCollider.gameObject != currentRail && contactPoints[i].otherCollider.gameObject != previousRail)
                {
                    transform.position = new Vector3(contactPoints[i].point.x, contactPoints[i].point.y + playerOffset, contactPoints[i].point.z);
                    if(currentRail != null)
                    {
                        previousRail = currentRail;
                        movementScript.SetPreviousRailSpeed();
                        countdownToPreviousRailReset = targetTimeForPreviousRailReset;
                    }
                    currentRail = collision.gameObject;
                    movementScript.JustOnRail(collision.gameObject);
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
        }
    }
}
