﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathManager : PauseManager
{
    private TextMeshProUGUI _highText;
    private Text _scoreText;

    public TextMeshProUGUI coinText;

    // Start is called before the first frame update
    public new void Awake()
    {
        coinText = transform.Find("Coin_Amount").GetComponent<TextMeshProUGUI>();
        pm = GameObject.Find("PlatformManager").GetComponent<PlatformManager>();
        deathManager = GameObject.Find("Death manager").GetComponent<DeathManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected new void Start()
    {
        _scoreText = transform.Find("Final_score").GetComponent<Text>();
        _highText = transform.Find("High_score").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        _scoreText.text = "Score: " + pm.GetScore();
        if (pm.GetScore() > PlayerPrefs.GetInt("Highscore")) PlayerPrefs.SetInt("Highscore", pm.GetScore());
        _highText.text = "High Score:\n<sprite=0> " + PlayerPrefs.GetInt("Highscore");

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) Restart();
    }

    public override void Restart()
    {
        base.Restart();
        SetInActive();
    }

    public void Home()
    {
        SceneManager.LoadSceneAsync("Main_Menu", mode: LoadSceneMode.Single);
    }

    protected new void SetInActive()
    {
        gameObject.SetActive(false);
    }
}