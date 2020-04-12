using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    private Camera _mainCamera;
    private Player _player;
    private GameObject _playerObject;
    private Vector2 _screenBounds;
    public float choke;
    public GameObject[] levels;

    public float speedRatio;

    // Start is called before the first frame update
    private void Start()
    {
        _playerObject = GameObject.Find("Player");
        if (_playerObject != null) _player = _playerObject.GetComponent<Player>();
        _mainCamera = GetComponent<Camera>();
        _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(x: Screen.width, y: Screen.height, z: _mainCamera.transform.position.z));
        LoadChildObjects(objects: levels);
    }

    private void LoadChildObjects(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
            int childrenNeeded = (int) Mathf.Ceil((_screenBounds.x * 2) / objectWidth) + 2;
            GameObject clone = Instantiate(original: obj);
            for (int i = 0; i < childrenNeeded; i++)
            {
                GameObject c = Instantiate(original: clone);
                c.transform.SetParent(p: obj.transform);
                c.transform.position = new Vector3(objectWidth * i, y: obj.transform.position.y, z: obj.transform.position.z);
                c.name = obj.name + i;
            }

            Destroy(obj: clone);
            // Destroy(obj.GetComponent<SpriteRenderer>());
        }
    }

    private void LateUpdate()
    {
        RepositionChildObjects(objects: levels);
    }

    private void FixedUpdate()
    {
        if (_player == null) return;
        foreach (GameObject obj in levels)
            obj.transform.position = new Vector3(obj.transform.position.x - (_player.GetDisplacement() / speedRatio),
                y: obj.transform.position.y, z: obj.transform.position.z);
    }

    private void RepositionChildObjects(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            Transform[] children = obj.GetComponentsInChildren<Transform>();
            if (children.Length <= 1) continue;
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if (transform.position.x + _screenBounds.x > lastChild.transform.position.x + halfObjectWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + (halfObjectWidth * 2), y: lastChild.transform.position.y, z: lastChild.transform.position.z);
            }
            else if (transform.position.x - _screenBounds.x < firstChild.transform.position.x - halfObjectWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - (halfObjectWidth * 2), y: firstChild.transform.position.y, z: firstChild.transform.position.z);
            }
        }
    }

    // Update is called once per frame
    private void Update() { }
}