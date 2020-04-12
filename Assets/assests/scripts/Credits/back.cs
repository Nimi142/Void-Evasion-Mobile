using UnityEngine;
using UnityEngine.SceneManagement;


// Special thanks to Daniel Kaufman!
public class back : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadSceneAsync("Main_Menu");
    }
}