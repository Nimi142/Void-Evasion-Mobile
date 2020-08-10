using GooglePlayGames;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int _score;

    private TextMeshProUGUI _txt;

    // Start is called before the first frame update
    private void Awake()
    {
        _txt = GetComponent<TextMeshProUGUI>();
    }

    public void SetScore(int newScore)
    {
        _score = newScore;
        _txt.text = "Score: " + _score;
    }

    public int GetScore()
    {
        return _score;
    }
}