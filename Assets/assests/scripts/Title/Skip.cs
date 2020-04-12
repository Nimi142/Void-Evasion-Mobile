using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour
{
    private bool _isLoading;

    // Start is called before the first frame update
    private void Start() { }

    private void Update()
    {
        if (_isLoading) return;
        SceneManager.LoadSceneAsync("Main_Menu");
        _isLoading = true;
    }
}