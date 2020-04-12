using UnityEngine;

public class camera_manager : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), y: transform.position.y, z: transform.position.z);
    }
}