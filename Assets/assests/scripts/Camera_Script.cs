using UnityEngine;

public class Camera_Script : MonoBehaviour
{
    private GameObject _target;

    // Start is called before the first frame update
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = new Vector3(_target.transform.position[0], _target.GetComponent<Player>().GetCameraSupposedY(), transform.position[2]);
    }
}