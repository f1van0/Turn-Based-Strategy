using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    //General information
    public string username;
    public int team;
    public Vector2 position;
    public bool isReady;

    //Game presentation


    //Lobby presentation
    public Text usernameText;
    public Image ReadyIndicator;

    public void SetReadyInLobby(bool _isReady)
    {
        if (_isReady)
        {
            ReadyIndicator.color = Color.green;
            isReady = true;
        }
        else
        {
            ReadyIndicator.color = Color.red;
            isReady = false;
        }
    }

    public void SetUsernameInLobby(string _username)
    {
        username = _username;
        usernameText = GetComponentInChildren<Text>();
        usernameText.text = username;
    }

    //

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SetTeam(Vector2 _position)
    {
        position = _position;
    }

    public void SetPosition(Vector2 _position)
    {
        position = _position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
