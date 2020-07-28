using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public InputField PlayerNickNameField;

    public GameObject serverPrefab;

    public GameObject ConnectionMenu;
    public GameObject LobbyMenu;
    public GameObject GameUI;

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

    public void OpenConnectionMenu()
    {
        ConnectionMenu.SetActive(true);
        LobbyMenu.SetActive(false);
        GameUI.SetActive(false);
    }

    public void OpenLobbyMenu()
    {
        ConnectionMenu.SetActive(false);
        LobbyMenu.SetActive(true);
        GameUI.SetActive(false);
    }

    public void OpenGameUI()
    {
        ConnectionMenu.SetActive(false);
        LobbyMenu.SetActive(false);
        GameUI.SetActive(true);
    }

    public void ConnectToServer()
    {
        if (PlayerNickNameField.text != "")
        {
            OpenLobbyMenu();
            Client.instance.ConnectToServer();
        }
        else
            PlayerNickNameField.GetComponent<Image>().color = Color.red;
    }

    public void RunServer()
    {
        if (PlayerNickNameField.text != "")
        {
            OpenLobbyMenu();
            ConnectToServer();
            Instantiate(serverPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

            LobbyManager.instance.ShowStartGameButton();
        }
        else
            PlayerNickNameField.GetComponent<Image>().color = Color.red;
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
