using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class music : MonoBehaviour
{
    private static bool _isSpawned;
    private AudioSource _auds;
    private float _defaultVolume;
    private float _interpolationRate;
    private float _interpolationStatus;
    private bool _isPlaying;
    private AudioClip _newAudio;
    private int[] _previousTracks;
    private Random _rand;
    private int gameMusicsLength;
    private string[] gameMusicsNames;
    private int menuMusicsLength;
    private string[] menuMusicsNames;
    private int shopMusicsLength;
    private string[] shopMusicsNames;

    private void Awake()
    {
        DontDestroyOnLoad(target: gameObject);
        if (_isSpawned)
        {
            Destroy(obj: gameObject);
            return;
        } // For some reason it also kills the first object
        _isSpawned = true;
        if (PlayerPrefs.GetInt("was_volume_changed") == 0) PlayerPrefs.SetFloat("volume",0.5f);
        _isPlaying = false;
        _previousTracks = new[] {-1, -1, -1}; // [Game, Menu, Shop]
        _interpolationRate = 3;
        _interpolationStatus = 0;
        _defaultVolume = PlayerPrefs.GetFloat("volume");
        _auds = GetComponent<AudioSource>();
        _auds.volume = _defaultVolume;
        _rand = new Random();
        AudioClip[] gameMusics = Resources.LoadAll<AudioClip>("music\\SampleScene");
        AudioClip[] menuMusics = Resources.LoadAll<AudioClip>("music\\Main_Menu");
        AudioClip[] shopMusics = Resources.LoadAll<AudioClip>("music\\Shop");
        _newAudio = menuMusics[0];
        gameMusicsLength = gameMusics.Length;
        menuMusicsLength = menuMusics.Length;
        shopMusicsLength = shopMusics.Length;
        gameMusicsNames = new string[gameMusicsLength];
        menuMusicsNames = new string[menuMusicsLength];
        shopMusicsNames = new string[shopMusicsLength];
        int longestArray = new int[3] {gameMusicsLength, menuMusicsLength, shopMusicsLength}.Max();
        for (int i = 0; i < longestArray; i++)
        {
            if (i < gameMusicsLength) gameMusicsNames[i] = gameMusics[i].name;
            if (i < menuMusicsLength) menuMusicsNames[i] = menuMusics[i].name;
            if (i < shopMusicsLength) shopMusicsNames[i] = shopMusics[i].name;
        }
        
        Resources.UnloadUnusedAssets();
        SceneManager.activeSceneChanged += ChangeScene;
    }

    private void ChangeScene(Scene oldScene, Scene newScene)
    {
        String newSceneName = newScene.name;
        int newClipIndex;
        string newClipName = "";
        switch (newSceneName)
        {
            case "SampleScene":
                newClipIndex = _rand.Next(0, maxValue: gameMusicsLength);
                while (newClipIndex == _previousTracks[0] && gameMusicsLength > 1) newClipIndex = _rand.Next(0, maxValue: gameMusicsLength);

                _previousTracks[0] = newClipIndex;
                newClipName = gameMusicsNames[newClipIndex];
                break;
            case "Main_Menu":
                newClipIndex = _rand.Next(0, maxValue: menuMusicsLength);
                while (newClipIndex == _previousTracks[1] && menuMusicsLength > 1) newClipIndex = _rand.Next(0, maxValue: menuMusicsLength);

                _previousTracks[1] = newClipIndex;
                newClipName = menuMusicsNames[newClipIndex];
                break;
            case "Shop":
                newClipIndex = _rand.Next(0, maxValue: shopMusicsLength);
                while (newClipIndex == _previousTracks[2] && shopMusicsLength > 1) newClipIndex = _rand.Next(0, maxValue: shopMusicsLength);

                _previousTracks[2] = newClipIndex;
                newClipName = shopMusicsNames[newClipIndex];
                break;
        }

        _newAudio = Resources.Load<AudioClip>("music\\" + newSceneName + "\\" + newClipName);

        if (PlayerPrefs.GetFloat("volume") > 0.01) _interpolationStatus = 0;
        if (_defaultVolume < 0.01) _auds.clip = _newAudio;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_interpolationStatus <= 1)
        {
            _interpolationStatus += Time.deltaTime * _interpolationRate;
            _auds.volume = Mathf.Lerp(a: _defaultVolume, 0, t: _interpolationStatus);
            if (_interpolationStatus > 1)
            {
                _auds.clip = _newAudio;
                if (PlayerPrefs.GetFloat("volume") > 0.01) _auds.Play();
                Resources.UnloadUnusedAssets();
            }
        }

        if (_interpolationStatus <= 2 && _interpolationStatus >= 1)
        {
            _interpolationStatus += Time.deltaTime * _interpolationRate;
            _auds.volume = Mathf.Lerp(0, b: _defaultVolume, _interpolationStatus - 1);
        }

        if (PlayerPrefs.GetFloat("volume") < 0.01)
        {
            _isPlaying = false;
            _auds.Pause();
        }
        else if (PlayerPrefs.GetFloat("volume") > 0.01 && !_isPlaying)
        {
            _isPlaying = true;
            _auds.UnPause();
            if (!_auds.isPlaying) _auds.Play();
        }
    }

    public void SetVolume(float volume)
    {
        _auds.volume = volume;
    }

    public float GetDefaultVolume()
    {
        return _defaultVolume;
    }

    public void UpdateDefaultVolume()
    {
        _auds.volume = PlayerPrefs.GetFloat("volume");
        _defaultVolume = PlayerPrefs.GetFloat("volume");
    }

    public float GetVolume()
    {
        return _auds.volume;
    }
}