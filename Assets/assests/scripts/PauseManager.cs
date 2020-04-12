using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _once;
    private GameObject _options;
    protected DeathManager deathManager;
    protected Player player;
    protected PlatformManager pm;

    protected void Awake()
    {
        _options = GameObject.Find("Canvas").transform.Find("Options").gameObject;
        pm = GameObject.Find("PlatformManager").GetComponent<PlatformManager>();
        deathManager = GameObject.Find("Death manager").GetComponent<DeathManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected void Start() { }

    // Update is called once per frame
    private void Update()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!player.IsPause()) SetInActive();
        if (Input.GetKeyUp("space")) Continue();
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void SetInActive()
    {
        gameObject.SetActive(false);
    }

    public void Continue()
    {
        player.Unpause();
        SetInActive();
    }

    public virtual void Restart()
    {
        player.Restart();
        pm.Restart();
        deathManager.SetInActive();
        SetInActive();
    }

    public void Options()
    {
        _options.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadSceneAsync("Main_Menu");
    }
}