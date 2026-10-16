using UnityEngine;

public class OnTopOfRailScript : MonoBehaviour
{
    public GameObject player;
    private Movement movement;
    private Transform TopOfRailPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = player.GetComponent<Movement>();
        TopOfRailPosition = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Board" && player != null)
        {
            player.transform.position = new Vector3(TopOfRailPosition.transform.position.x, TopOfRailPosition.transform.position.y + 0.5f, 0);
            Debug.Log("Entered top of rail, at x " + TopOfRailPosition.transform.position.x + " and y " + TopOfRailPosition.transform.position.y);
            movement.OnRail = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Board" && player != null)
        {
            player.transform.position = new Vector3(TopOfRailPosition.transform.position.x, TopOfRailPosition.transform.position.y + 0.5f, 0);
            Debug.Log("in top of rail");
            movement.OnRail = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        movement.OnRail = false;
        Debug.Log("left rail");
    }
}
