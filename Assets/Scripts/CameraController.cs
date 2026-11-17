using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private float offset = 5f;
    public float cameraSpeed = 5f;

    void FixedUpdate()
    {
        // grabs the intended position for the camera to be based on the position of the player object and the camera offset
        Vector3 goToPosition = player.transform.position + player.transform.right * offset;
        // adds 2.5 to the height of the goto position to make the camera look more cinematic
        goToPosition = new Vector3(goToPosition.x, goToPosition.y + 2.5f, goToPosition.z);
        // if the positon of the camera is not the goto position, moves towards to the goto positon, as to not disorient the player
        if (transform.position != goToPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, goToPosition, cameraSpeed * Time.deltaTime);
        }
        // makes the main camera look at the player
        transform.LookAt(player.transform.position);
    }
}
