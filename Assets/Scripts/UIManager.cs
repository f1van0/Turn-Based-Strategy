using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public InputField PlayerNickNameField;
    public InputField AdressAndPortField;
    public Text AdressAndPortText;

    public GameObject serverPrefab;
    public GameObject chatScrollView;

    public GameObject ConnectionMenu;
    public GameObject LobbyMenu;
    public GameObject GameUIMenu;

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

    public void ShowAddressAndPort(string _ipAdress, int _port)
    {
        AdressAndPortText.text = "Address - " + _ipAdress + ":" + _port;
    }

    public void OpenConnectionMenu()
    {
        ConnectionMenu.SetActive(true);
        LobbyMenu.SetActive(false);
        GameUIMenu.SetActive(false);
    }

    public void OpenLobbyMenu()
    {
        ConnectionMenu.SetActive(false);
        LobbyMenu.SetActive(true);
        GameUIMenu.SetActive(false);
    }

    public void OpenGameUI()
    {
        ConnectionMenu.SetActive(false);
        LobbyMenu.SetActive(false);
        GameUIMenu.SetActive(true);
    }

    public void ConnectToServer()
    {
        if (PlayerNickNameField.text != "")
        {
            string _ipAddress = StringToAddress(AdressAndPortField.text).Item1;
            int _port = StringToAddress(AdressAndPortField.text).Item2;

            Client.instance.ConnectToServer();
        }
        else
            PlayerNickNameField.GetComponent<Image>().color = Color.red;
    }

    public (string, int) StringToAddress(string _address)
    {
        string _ip = "";
        int _port = 0;
        bool _isPortReadable = false;

        for (int i = 0; i < _address.Length; i++)
        {
            if (_address[i] == ':')
            {
                _isPortReadable = true;
            }
            else
            {
                if (!_isPortReadable)
                {
                    _ip = _ip + _address[i];
                }
                else
                {
                    _port = _port * 10 + (_address[i] - '0');
                }
            }
        }

        return (_ip, _port);
    }

    /*
    public bool isAddressCorrect()
    {
        string _address = AdressAndPortField.text;
        bool _isCorrect = true;
        if (_address.Contains(":"))
        {
            foreach (char _character in _address)
            {

            }

            if
        }
        else
        {
            return false;
        }
    }
    */

    public void RunServer()
    {
        if (PlayerNickNameField.text != "")
        {
            Instantiate(serverPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

            OpenLobbyMenu();
            ConnectToServer();

            LobbyManager.instance.ShowStartGameButton();
            GameUI.instance.ShowNextTurnButton();
        }
        else
        {
            PlayerNickNameField.GetComponent<Image>().color = Color.red;
        }
    }
    
    public void HideOrOpenChat()
    {
        chatScrollView.SetActive(!chatScrollView.active);
    }

    public void StartGame()
    {
        Assets.Scripts.Network.Server.ServerSideComputing.StartGame();
    }

    public string GetUserName()
    {
        return PlayerNickNameField.text;
    }

    // Start is called before the first frame update
    void Start()
    {
        OpenConnectionMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
