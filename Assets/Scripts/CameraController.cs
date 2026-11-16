using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private float offset = 5f;
    public float cameraSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 goToPosition = player.transform.position + player.transform.right * offset;
        goToPosition = new Vector3(goToPosition.x, goToPosition.y + 2.5f, goToPosition.z);
        if (transform.position != goToPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, goToPosition, cameraSpeed * Time.deltaTime);
        }
        transform.LookAt(player.transform.position);
    }
}
