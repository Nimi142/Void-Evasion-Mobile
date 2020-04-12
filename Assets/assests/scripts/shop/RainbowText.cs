using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class RainbowText : MonoBehaviour
{
    private Color _newColor;
    private Color _oldColor;
    private Random _rand;

    private float _t;

    // Start is called before the first frame update
    private void Start()
    {
        _rand = new Random();
        // InvokeRepeating("SwitchColors", 0, 5);
        SwitchColors();
    }

    // Update is called once per frame
    private void Update()
    {
        _t += Time.deltaTime / 5;
        GetComponent<Text>().color = Color.Lerp(a: _oldColor, b: _newColor, t: _t);
        if (GetComponent<Text>().color == _newColor) SwitchColors();
    }

    private void SwitchColors()
    {
        _newColor = new Color((float) _rand.NextDouble(), (float) _rand.NextDouble(), (float) _rand.NextDouble());
        _oldColor = GetComponent<Text>().color;
        _t = 0;
    }
}