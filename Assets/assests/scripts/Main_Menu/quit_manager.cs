using UnityEngine;

public class quit_manager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start() { }

    public void KeepPlaying()
    {
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }


    // Update is called once per frame
    private void Update() { }
}