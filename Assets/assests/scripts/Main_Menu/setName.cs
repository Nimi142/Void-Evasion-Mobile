﻿using UnityEngine;
using UnityEngine.UI;

public class setName : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        UpdateName();
        if (PlayerPrefs.GetInt("was_volume_changed") == 0) PlayerPrefs.SetString("Name","Name");
    }

    public void UpdateName()
    {
        GetComponent<Text>().text = PlayerPrefs.GetString("Name");
    }
}