using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options_script : MonoBehaviour
{
    private music _musicManager;
    private Slider _soundSlider;
    private GameObject _soundX;
    public Transform nameInput;
    public GameObject nameText;

    private void Awake()
    {
        _musicManager = GameObject.Find("Music manager").GetComponent<music>();
        _soundX = transform.Find("Sound Image").transform.Find("Checkmark").gameObject;
        _soundSlider = transform.Find("Sound Slider").GetComponent<Slider>();
        nameInput = transform.Find("SetName");
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        _soundSlider.value = PlayerPrefs.GetFloat("volume");
        nameInput.GetComponent<InputField>().text = PlayerPrefs.GetString("Name");
    }

    public void OnSoundSliderValueChange(float value)
    {
        _musicManager.SetVolume(volume: _soundSlider.value);
    }


    public void Save()
    {
        PlayerPrefs.SetInt("was_volume_changed",1);
        PlayerPrefs.SetFloat("volume", value: _soundSlider.value);
        _musicManager.UpdateDefaultVolume();
        PlayerPrefs.SetString("Name", value: nameInput.GetComponent<InputField>().text);
        if (SceneManager.GetActiveScene().name.Equals("Main_Menu")) nameText.GetComponent<setName>().UpdateName();
        gameObject.SetActive(false);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void X()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        _soundX.SetActive(_soundSlider.value < 0.01);
    }
}