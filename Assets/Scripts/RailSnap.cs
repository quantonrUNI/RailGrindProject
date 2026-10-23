using UnityEngine;

public class RailSnap : MonoBehaviour
{
    private GameObject currentRail = null;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("my name is " + gameObject.name + " and I have entered " + collision.gameObject.name);
        if (collision.gameObject.tag == "Rail" && currentRail == null )
        {
            Debug.Log("snapping");
            int likelyContacts = 5;
            ContactPoint[] contactPoints = new ContactPoint[likelyContacts];
            int numOfContacts = collision.GetContacts(contactPoints);
            for(int i = 0; i < numOfContacts; i++)
            {
                if (contactPoints[i].otherCollider.gameObject.tag == "Rail")
                {
                    transform.position = new Vector3(contactPoints[i].point.x, contactPoints[i].point.y + playerOffset, contactPoints[i].point.z);
                    movementScript.JustOnRail();
                    break;
                }
            }
            //transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + PlayerOffset, collision.transform.position.z);
            currentRail = collision.gameObject;
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
