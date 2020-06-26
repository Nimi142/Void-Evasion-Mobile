using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private int _moneyNeeded;
    private PlayerColorManager _pcm;
    public GameObject confirmManager;
    public ColorPicker cp;
    public GameObject lackMoneyManager;

    public StorageHandler sh;

    // Start is called before the first frame update
    private void Start()
    {
        _moneyNeeded = 500;
        sh = new StorageHandler();
        _pcm = GameObject.Find("Player_Outer").GetComponent<PlayerColorManager>();
    }

    public void Return()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void ResetButton()
    {
        _pcm.LoadColors();
        if (_pcm.IsInner()) _pcm.SetColorPickerColor(sh.LoadData("InnerColor") as MyColor);
        else _pcm.SetColorPickerColor(sh.LoadData("OuterColor") as MyColor);
        _pcm.ResetTemp();
    }

    public void CommitButton()
    {
        if (PlayerPrefs.GetInt("Money") >= _moneyNeeded)
            confirmManager.SetActive(true);
        else
            lackMoneyManager.SetActive(true);
    }

    public void NotEnoughMoneyButton()
    {
        lackMoneyManager.SetActive(false);
    }

    public void CompletePurchase()
    {
        MyColor[] colors = _pcm.GetCurrentColors();
        sh.SaveData(colors[0], "OuterColor");
        sh.SaveData(colors[1], "InnerColor");
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - _moneyNeeded);
        GameObject.Find("Coins_background").transform.Find("Amount").GetComponent<coin_amount>().UpdateMoney();
        Social.ReportProgress("CgkIkNbx2-YEEAIQBg", 100.0f, (bool success) => {
            // handle success or failure
        });
        confirmManager.SetActive(false);
    }

    public void AbortPurchase()
    {
        confirmManager.SetActive(false);
    }

    // Update is called once per frame
    private void Update() { }
}