using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private float offset = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + player.transform.right * offset;
        transform.LookAt(player.transform.position);
    }
}
