using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayButtons : MonoBehaviour
{
    private GameObject achievementsButton;
    private GameObject leaderboardsButton;
    // Start is called before the first frame update
    void Start()
    {
        achievementsButton = GameObject.Find("AchievementsButton");
        leaderboardsButton = GameObject.Find("LeaderboardsButton");
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            achievementsButton.SetActive(false);
            leaderboardsButton.SetActive(false);
        }
    }

    public void ShowLeaderboards()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }

    public void ShowAchievements()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }
    
    public void CheckAuthentication()
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            achievementsButton.SetActive(false);
            leaderboardsButton.SetActive(false);
        }
        else
        {
            achievementsButton.SetActive(true);
            leaderboardsButton.SetActive(true);
            
        }
    }

    public void ConnectToPlay()
    {        
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, status =>
        {
            Debug.Log("Status of google play authentication: " + status);
        });
    }

    public void HandlePlayButton()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.SignOut();
        }
        else
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, status =>
            {
                Debug.Log("Status of google play authentication: " + status);
            });
            if (PlayGamesPlatform.Instance.IsAuthenticated()) PlayGamesPlatform.Activate();
        }
    }
    
    
}
