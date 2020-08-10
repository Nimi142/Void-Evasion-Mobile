using System;
using GooglePlayGames;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() == null) return;
        coinAmount++;
        PlayerPrefs.SetInt("CoinAmount",PlayerPrefs.GetInt("CoinAmount")+1);
        Destroy(obj: gameObject);
        PlayGamesPlatform.Instance.IncrementAchievement(
            "CgkIkNbx2-YEEAIQBw", 1, (bool success) => {
                // handle success or failure
            }); // 500 coins.
        PlayGamesPlatform.Instance.IncrementAchievement(
            "CgkIkNbx2-YEEAIQCA", 1, (bool success) => {
                // handle success or failure
            }); // 1,000 coins.
        PlayGamesPlatform.Instance.IncrementAchievement(
            "", 1, (bool success) => {
                // handle success or failure
            }); // 10,000 coins.
    }
}