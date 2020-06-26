﻿using GooglePlayGames;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static int coinAmount;
    private Collider2D _col;

    private GameObject _player;

    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.Find("Player");
        _col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_col.IsTouching(_player.GetComponent<Collider2D>())) return;
        coinAmount++;
        PlayerPrefs.SetInt("CoinAmount",PlayerPrefs.GetInt("CoinAmount")+1);
        Destroy(obj: gameObject);
        PlayGamesPlatform.Instance.IncrementAchievement(
            "CgkIkNbx2-YEEAIQBw", 1, (bool success) => {
                // handle success or failure
            });
        PlayGamesPlatform.Instance.IncrementAchievement(
            "CgkIkNbx2-YEEAIQCA", 1, (bool success) => {
                // handle success or failure
            });
    }
}