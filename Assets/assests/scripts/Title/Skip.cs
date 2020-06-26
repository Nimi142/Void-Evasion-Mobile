using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class Skip : MonoBehaviour
{
    private bool _isLoading;

    // Start is called before the first frame update
    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, status =>
        {
            Debug.Log("Status of google play authentication: " + status);
        });
        PlayGamesPlatform.Activate();
    }

    private void Update()
    {
        if (_isLoading) return;
        SceneManager.LoadSceneAsync("Main_Menu");
        _isLoading = true;
    }
}