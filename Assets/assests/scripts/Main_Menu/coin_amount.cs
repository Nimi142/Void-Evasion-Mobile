using System;
using GooglePlayGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class coin_amount : MonoBehaviour
{
    private Text nameText;
    private PlayButtons _playButtons;
    // Start is called before the first frame update
    private void Start()
    {
        nameText = GameObject.Find("User_Management").transform.Find("Name").GetComponent<Text>();
        _playButtons = GameObject.Find("AchievementsButton").GetComponent<PlayButtons>();
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Money").ToString();
    }

    public void Update()
    {
        _playButtons.CheckAuthentication();
    }

    public void UpdateMoney()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Money").ToString();
    }
}