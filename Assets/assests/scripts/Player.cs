using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Camera _cam;
    private BoxCollider2D _col;
    private float _currVolume;
    private Image _darkener;
    private DeathManager _deathManager;
    private float _height;
    private MyColor _innerColor;
    private bool _isDead;
    private bool _isJumping;
    private bool _isPause;
    private bool _isStartJump;
    private float _lastVolume;
    private float _lastX;
    private float _lastx2;
    private float _musicInterpolation;
    private music _musicManager;
    private GameObject _options;
    private MyColor _outerColor;
    private PauseManager _pauseMenu;
    private PlatformManager _pm;
    private Rigidbody2D _rb;
    private GameObject _redBar;
    private StorageHandler _sh;
    private Vector2 _storedVelocity;
    public float acceleration;
    public bool isDebug;
    public float maxSpeed;

    private void Awake()
    {
        _options = GameObject.Find("Canvas").transform.Find("Options").gameObject;
        _currVolume = 0.5f;
        _musicInterpolation = 0;
        try
        {
            _musicManager = GameObject.Find("Music manager").GetComponent<music>();
        }
        catch (NullReferenceException) { }

        isDebug = Application.isEditor;
        _redBar = GameObject.Find("Red_bar");
        _sh = new StorageHandler();
        ReadColors();
        _pauseMenu = GameObject.Find("Canvas").GetComponentInChildren<PauseManager>();
        _deathManager = GameObject.Find("Death manager").GetComponent<DeathManager>();
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        _pm = GameObject.Find("PlatformManager").GetComponent<PlatformManager>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        transform.position = new Vector3(0, GetHeight() / 2, z: transform.position.z);
        _darkener = GameObject.Find("Canvas").transform.Find("Darkener").GetComponent<Image>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _lastx2 = 0f;
        _lastX = 0f;
        _height = _cam.ScreenToWorldPoint(new Vector3(x: Screen.width, y: Screen.height, 0))[1];
        _isDead = false;
        _pauseMenu.SetInActive();
        _deathManager.SetInActive();
        _isPause = false;
        _isStartJump = Input.touchCount > 0;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 currentPosition = transform.position;
        _redBar.transform.position = currentPosition.y + _col.bounds.extents.y >= _height ? new Vector3(x: currentPosition.x, currentPosition.y + _col.bounds.extents.y + ((_redBar.GetComponent<SpriteRenderer>().sprite.rect.height * _redBar.transform.localScale[1]) / 200), z: _redBar.transform.position.z) : new Vector3(x: currentPosition.x, y: _redBar.transform.position.y, z: _redBar.transform.position.z);
        // Check quit command

        // check to pause
    }

    private void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        if (_musicManager != null)
        {
            if (!_isDead && !_options.activeSelf) _currVolume = _musicManager.GetDefaultVolume();
            if (Mathf.Abs(_musicManager.GetVolume() - _currVolume) > 0.001 && !_options.activeSelf)
            {
                _musicInterpolation += Time.deltaTime * 4;
                _musicManager.SetVolume(Mathf.Lerp(a: _lastVolume, b: _currVolume, t: _musicInterpolation));
            }
            else
            {
                _musicInterpolation = 0;
            }
        }

        _rb.velocity = new Vector2(x: maxSpeed, _rb.velocity[1]);
        if (currentPosition[1] + (_col.size[1] / 2) < -_height && !_isDead) Die();
        Vector2 v = new Vector2(_rb.position[0], _rb.position[1]);
        Vector3 vAngle = new Vector3(0, 0, 0);
        v[0] += acceleration;
        vAngle[0] = 1;
        // Check jump
        if (Input.GetKey("space") && isDebug || Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && !_isStartJump && (_rb.velocity[1] > 0 && _isJumping || -0.001 < _rb.velocity[1] && _rb.velocity[1] < 0.001 && IsOnPlatform()))
        {
            _isJumping = true;
            v[1] += acceleration;
            vAngle[1] = 1;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.touchCount == 0)
        {
            _isStartJump = false;
            _isJumping = false;
        }

        //Checking borders
        if (currentPosition.y + _col.bounds.extents.y >= _height) vAngle[1] = -0.3f;
        if (_rb.velocity[0] > maxSpeed) vAngle[0] = 0;
        if (!_isPause && !IsOnSide()) _rb.AddForce(vAngle * acceleration, mode: ForceMode2D.Impulse); // The actual movement
        if (IsOnSide()) _rb.AddForce((new Vector2(0, -1) * (acceleration * maxSpeed) / 2.3f), mode: ForceMode2D.Impulse);
        _lastx2 = _lastX;
        _lastX = currentPosition.x;
    }

    private bool IsOnPlatform()
    {
        // To be made
        foreach (Platform platform in GameObject.Find("PlatformManager").GetComponent<PlatformManager>().GetPlatforms())
            if (_col.IsTouching(platform.GetUpperEdge()))
                return true;
        return false;
    }

    private void ReadColors()
    {
        _innerColor = _sh.LoadData("InnerColor") as MyColor;
        _outerColor = _sh.LoadData("OuterColor") as MyColor;
        if (_innerColor == null)
        {
            _innerColor = (Color) new Color32(199, 0, 1, 255); // Courtesy of Julius
            _sh.SaveData(objectToSave: _innerColor, "InnerColor");
        }

        if (_outerColor == null)
        {
            _outerColor = new Color(0, 0, 0);
            _sh.SaveData(objectToSave: _outerColor, "OuterColor");
        }

        GetComponent<SpriteRenderer>().color = _outerColor;
        transform.Find("Inner_Color").GetComponent<SpriteRenderer>().color = _innerColor;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetWidth()
    {
        return _col.size[0];
    }

    public float GetHeight()
    {
        return _col.bounds.extents.y;
    }

    private bool IsOnSide()
    {
        return Mathf.Abs(transform.position.x - _lastX) < 0.1;
    }

    private void Die()
    {
        if (_musicManager != null)
        {
            _lastVolume = _musicManager.GetVolume();
            _currVolume = _musicManager.GetDefaultVolume() / 10;
        }
        _darkener.color = new Color(r: _darkener.color.r, g: _darkener.color.g, b: _darkener.color.b, 0.5f);
        maxSpeed = 0.2f;
        _isDead = true;
        GetComponent<SpriteRenderer>().enabled = false;
        _pm.SetCreating(false);
        _deathManager.SetActive();
        _deathManager.coinText.text = "Coins:\n<sprite=0> " + Coin.coinAmount;
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + Coin.coinAmount);
        Coin.coinAmount = 0;
        ManageGooglePlayStats();
        GC.Collect();
    }

    private void ManageGooglePlayStats()
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated()) return;
        // Coins gathered Leaderboard
        PlayGamesPlatform.Instance.ReportScore(PlayerPrefs.GetInt("CoinAmount"), GPGSIds.leaderboard_coins_gathered, (bool success) => {
            Debug.Log("Log to coin's play leaderboard result" + success);
        });
        // Highscore leaderboard
        PlayGamesPlatform.Instance.ReportScore(PlayerPrefs.GetInt("Highscore"), GPGSIds.leaderboard_highscores, (bool success) => {
                Debug.Log("Log to score's play leaderboard result" + success);
            });
        // 10 Platforms Achievement
        if (_pm.GetScore() > 10) PlayGamesPlatform.Instance.ReportProgress("CgkIkNbx2-YEEAIQAg", 100.0f, (bool success) => {
            // handle success or failure
        });
        // 50 Platforms Achievement
        if (_pm.GetScore() > 50) PlayGamesPlatform.Instance.ReportProgress("CgkIkNbx2-YEEAIQAw", 100.0f, (bool success) => {
            // handle success or failure
        });
        // 100 Platforms Achievement
        if (_pm.GetScore() > 100) PlayGamesPlatform.Instance.ReportProgress("CgkIkNbx2-YEEAIQBA", 100.0f, (bool success) => {
            // handle success or failure
        });
        // 300 Platform Achievement
        if (_pm.GetScore() > 300) PlayGamesPlatform.Instance.ReportProgress("CgkIkNbx2-YEEAIQBQ", 100.0f, (bool success) => {
            // handle success or failure
        });
        
    }

    public void Restart()
    {
        if (_musicManager != null)
        {
            _lastVolume = _musicManager.GetVolume();
            _currVolume = _musicManager.GetDefaultVolume();
        }

        _darkener.color = new Color(r: _darkener.color.r, g: _darkener.color.g, b: _darkener.color.b, 0f);
        maxSpeed = 7;
        _cam.transform.position = new Vector3(0, 0, z: _cam.transform.position.z);
        transform.position = new Vector3(0, 0 + (_col.bounds.extents.y / 2), z: transform.position.z);
        _isDead = false;
        _isPause = false;
        GetComponent<SpriteRenderer>().enabled = true;
        _rb.velocity = new Vector2(0, 0);
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _isStartJump = Input.touchCount > 0;
        // pm.Restart();
    }

    public bool IsPause()
    {
        return _isPause;
    }

    public void ChangePause()
    {
        _isPause = !_isPause;
        if (_isPause)
        {
            _pauseMenu.SetActive();
            _storedVelocity = _rb.velocity;
            _rb.velocity = Vector2.zero;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            Unpause();
        }
    }

    public void Unpause()
    {
        _isPause = false;
        _rb.velocity = _storedVelocity;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public float GetDisplacement()
    {
        return _lastX - _lastx2;
    }

    public float GetCameraSupposedY()
    {
        if (transform.position.y + (_col.bounds.extents.y / 2) > _height) return ((transform.position.y + (_col.bounds.extents.y / 2)) - _height) + Mathf.Lerp(0, _height / 2, (transform.position.y - _height) / 10);
        return 0;
    }
}