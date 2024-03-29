﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Text NickNameText;
    public string Nickname;
    public Image Readible;
    public static PlayerInfo instance;
    public Vector2 position;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //NickNameText = GetComponentInChildren<Text>();
            //NickNameText.text = "UserName";
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ChangeNickName(string _username)
    {
        Nickname = _username;
        GetComponentInChildren<Text>().text = Nickname;
    }

    public void ChangeReadiness(bool _isReady)
    {
        if (_isReady)
        {
            Readible.color = Color.green;
        }
        else
        {
            Readible.color = Color.red;
        }
    }

    public void ChangePosition(Vector2 _position)
    {
        position = _position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
