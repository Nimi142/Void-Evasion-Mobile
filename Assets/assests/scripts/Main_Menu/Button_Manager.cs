using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Button_Manager : MonoBehaviour
{
    [FormerlySerializedAs("Quit_manager")] public GameObject quitManager;

    // Start is called before the first frame update
    private void Start() { }

    public void Play()
    {
        SceneManager.LoadSceneAsync("Game", mode: LoadSceneMode.Single);
    }

    public void Shop()
    {
        SceneManager.LoadSceneAsync("Shop", mode: LoadSceneMode.Single);
    }

    public void Quit()
    {
        quitManager.SetActive(true);
    }

    // Update is called once per frame
    private void Update() { }
}