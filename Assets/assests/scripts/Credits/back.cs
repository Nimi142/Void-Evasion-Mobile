using System;
using UnityEngine;
using UnityEngine.SceneManagement;


// Special thanks to Daniel Kaufman!
public class back : MonoBehaviour
{
    public void Start()
    {
        Social.ReportProgress("CgkIkNbx2-YEEAIQCQ", 100.0f, (bool success) => {
            // handle success or failure
        });
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync("Main_Menu");
    }
}