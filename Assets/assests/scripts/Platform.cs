using UnityEngine;
using UnityEngine.Serialization;

public class Platform : MonoBehaviour
{
    private BoxCollider2D _col;

    [FormerlySerializedAs("PlatformImage")]
    public Sprite platformImage;

    public bool wasReached;

    // Start is called before the first frame update
    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
        // sr.size = col.size;
        // sr.sprite = PlatformImage;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetWidth()
    {
        return _col.size[0];
    }

    public float GetHeight()
    {
        return _col.size[1];
    }

    public bool IsReached()
    {
        return wasReached;
    }

    public void Reach()
    {
        wasReached = true;
    }

    public EdgeCollider2D GetUpperEdge()
    {
        return GetComponent<EdgeCollider2D>();
    }
}