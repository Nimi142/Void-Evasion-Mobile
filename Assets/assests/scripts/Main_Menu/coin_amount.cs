using UnityEngine;
using UnityEngine.UI;

public class coin_amount : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Money").ToString();
    }

    public void UpdateMoney()
    {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Money").ToString();
    }

    // Update is called once per frame
    private void Update() { }
}