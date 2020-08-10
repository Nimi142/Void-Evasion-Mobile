using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

/// <summary>
///     Platform manager manages the platforms and coins in the game.
///     Currently it creates platforms every certain x value, it creates a platform when the last one is completely in
///     view.
///     It deletes every platform that is behind the screen's left side.
///     The y component of the new platform's position is being created with a triangular probability curve.
///     The amount of coins is with a binomial distribution.
/// </summary>
public class PlatformManager : MonoBehaviour
{
    private float _basicHeight;
    private float _basicWidth;
    private Camera _cam;
    private GameObject _camObject;
    private float _coinHeight;
    private GameObject[] _coins;
    private float _coinWidth;
    private float _height;
    private bool _isCreating;
    private Platform[] _platforms;
    private GameObject[] _platformsObjects;
    private Player _player;
    private Vector3 _position;
    private Random _rand;
    private Score _score;
    public GameObject coinPrefab;
    public float platformBuffer;

    [FormerlySerializedAs("PrePlatform")] public GameObject prePlatform;

    // Start is called before the first frame update
    private void Awake()
    {
        _score = GameObject.Find("Score").GetComponent<Score>();
        _camObject = GameObject.Find("Main Camera");
        _cam = _camObject.GetComponent<Camera>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        GameObject exampleCoin = Instantiate(original: coinPrefab, new Vector3(-200, 5, 0), rotation: Quaternion.identity);
        _coinHeight = exampleCoin.GetComponent<Collider2D>().bounds.extents.y * 2;
        _coinWidth = exampleCoin.GetComponent<Collider2D>().bounds.extents.x * 2;
        GameObject p = Instantiate(original: prePlatform, new Vector3(-200, 0, (float) -1.160699), rotation: Quaternion.identity);
        _basicHeight = p.GetComponent<Collider2D>().bounds.extents.y * 2;
        _basicWidth = p.GetComponent<Collider2D>().bounds.extents.x * 2;
        CreateFirstPlatforms();
        _isCreating = true;
        _rand = new Random();
        _height = _cam.ScreenToWorldPoint(new Vector3(x: Screen.width, y: Screen.height, 0))[1];
        _score.SetScore(-2);
    }

    // Update is called once per frame
    private void Update()
    {
        _coins = GameObject.FindGameObjectsWithTag("Coin");
        _platformsObjects = GameObject.FindGameObjectsWithTag("Platform");
        _platforms = new Platform[_platformsObjects.Length];
        float xLastPlatform = float.MinValue;
        for (int i = 0; i < _platformsObjects.Length; i++)
        {
            _platforms[i] = _platformsObjects[i].GetComponent<Platform>();
            if (_platformsObjects[i].transform.position.x > xLastPlatform) xLastPlatform = _platformsObjects[i].transform.position[0];
        }

        xLastPlatform += +_basicWidth / 2;
        _position = _player.GetPosition();
        // Create new
        // Vector3 p = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        float width = 2 * _cam.orthographicSize * _cam.aspect;
        float camPosition = _camObject.gameObject.transform.position[0] + (width / 2);
        if (camPosition > xLastPlatform)
            try
            {
                if (_isCreating)
                {
                    Platform last = _platforms[_platforms.Length - 1];
                    // Instantiate coin
                    GameObject p = Instantiate(original: prePlatform, new Vector3(camPosition + platformBuffer, NextY(last.GetPosition()[1], 2f, _height - _player.GetHeight() - last.GetHeight(), -_height + _player.GetHeight() + last.GetHeight()), (float) -1.160699), rotation: Quaternion.identity);
                    foreach (float coinX in MakeCoinsLocation(p.transform.position.x - p.GetComponent<Collider2D>().bounds.extents.x,
                        p.transform.position.x + p.GetComponent<Collider2D>().bounds.extents.x, coinWidth: _coinWidth))
                        Instantiate(original: coinPrefab,
                            new Vector3(x: coinX,
                                p.transform.position.y + (_basicHeight / 2) + (_coinHeight / 2), -1.160699f),
                            rotation: Quaternion.identity);
                }
            }
            catch (NullReferenceException) { }

        //Delete platforms
        for (int i = 0; i < _platforms.Length; i++)
        {
            if (!_platforms[i].IsReached() && _position[0] - (_player.GetWidth() / 2) > _platforms[i].GetPosition()[0] - (_platforms[i].GetWidth() / 2))
            {
                _platforms[i].Reach();
                _score.SetScore(_score.GetScore() + 1);
                _player.numFlownPlatforms++;
                if (_player.maxFlownPlatforms < _player.numFlownPlatforms) _player.maxFlownPlatforms = _player.numFlownPlatforms;
            }

            if (_platforms[i].GetPosition()[0] < _position[0] - (width / 2) - (_basicWidth / 2)) Destroy(_platformsObjects[i]);
        }

        foreach (GameObject coin in _coins)
            if (coin.transform.position.x + (_coinWidth / 2) < _cam.transform.position.x - (width / 2))
                Destroy(obj: coin);
    }

    public void CreateFirstPlatforms()
    {
        Instantiate(original: prePlatform, new Vector3(0.001f, -(_basicHeight / 2), -1.160699f), rotation: Quaternion.identity);
        Instantiate(original: prePlatform, new Vector3(10.34f, 0.1215f, -1.160699f), rotation: Quaternion.identity);
    }

    public void Restart()
    {
        _isCreating = true;
        CreateFirstPlatforms();
        // Deleting all platforms
        foreach (GameObject platform in _platformsObjects) Destroy(obj: platform);
        // Deleting all coins
        foreach (GameObject coin in _coins) Destroy(obj: coin);
        _score.SetScore(0);
    }

    public Platform[] GetPlatforms()
    {
        return _platforms;
    }

    public void SetCreating(bool create)
    {
        _isCreating = create;
        if (create) return;
        foreach (GameObject platform in _platformsObjects) Destroy(obj: platform);
        foreach (GameObject coin in _coins) Destroy(obj: coin);
    }

    public int GetScore()
    {
        return _score.GetScore();
    }

    public float NextY(float lastY, float range, float boundaryUp, float boundaryDown)
    {
        double seed = _rand.NextDouble();
        // The probability of a certain offset from the previous Y (clamped so that the range would fit inside the boundaries) linearly-decreases
        return Mathf.Clamp(value: lastY, boundaryDown + range, boundaryUp - range) + (float) ((((_rand.Next() % 2) * 2) - 1) * (seed * (2 - seed)) * range);
    }

    private float[] MakeCoinsLocation(float leftEdge, float rightEdge, float coinWidth)
    {
        Random rand = new Random();
        double avgCoins = 0.5;
        float platformWidth = rightEdge - leftEdge;
        byte maxNumCoins = (byte) (platformWidth / coinWidth);
        double prob = avgCoins / maxNumCoins;
        byte numCoins = 0;
        for (int i = 0; i < maxNumCoins; i++)
            if (prob - rand.NextDouble() > 0)
                numCoins++;
        float[] coinsLocs = new float[numCoins];
        float sectionWidth = platformWidth / numCoins;
        float rangeWidth = sectionWidth - coinWidth;
        float rangeLeftEdge = leftEdge + (coinWidth / 2);
        for (int iCoin = 0; iCoin < numCoins; iCoin++)
        {
            coinsLocs[iCoin] = rangeLeftEdge + ((float) rand.NextDouble() * rangeWidth);
            rangeLeftEdge += sectionWidth;
        }

        return coinsLocs;
    }
}