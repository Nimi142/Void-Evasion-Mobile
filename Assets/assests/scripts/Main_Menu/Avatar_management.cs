using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Avatar_management : MonoBehaviour
{
    [FormerlySerializedAs("Female")] public Sprite female;
    [FormerlySerializedAs("Male")] public Sprite male;

    [FormerlySerializedAs("Other")] public Sprite other;

    // Start is called before the first frame update
    private void Start()
    {
        if (PlayerPrefs.GetString("Gender").Equals("Woman"))
            GetComponent<Image>().sprite = female;
        else if (PlayerPrefs.GetString("Gender").Equals("Man"))
            GetComponent<Image>().sprite = male;
        else if (PlayerPrefs.GetString("Gender").Equals("Other")) GetComponent<Image>().sprite = other;
    }

    // Update is called once per frame
    private void Update() { }
}