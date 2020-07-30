using System;
using GooglePlayGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class coin_amount : MonoBehaviour
{
    private PlayButtons _playButtons;
    // Start is called before the first frame update
    private void Start()
    {
        if (GameObject.Find("AchievementsButton") != null) _playButtons = GameObject.Find("AchievementsButton").GetComponent<PlayButtons>();
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Money").ToString();
    }

    public void Update()
    {
        if (_playButtons != null) _playButtons.CheckAuthentication();
    }

    public void UpdateMoney()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Money").ToString();
    }
}