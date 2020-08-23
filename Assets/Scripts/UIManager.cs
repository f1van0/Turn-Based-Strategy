using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public InputField PlayerNickNameField;
    public InputField ipAddressField;
    public InputField portField;
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

    public void ShowAddressAndPort(string _ipAddress, int _port)
    {
        AdressAndPortText.gameObject.SetActive(true);
        AdressAndPortText.text = "Address - " + _ipAddress + ":" + _port;
    }

    public void OpenConnectionMenu()
    {
        ConnectionMenu.SetActive(true);
        LobbyMenu.SetActive(false);
        GameUIMenu.SetActive(false);

        AdressAndPortText.gameObject.SetActive(false);
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
            PlayerNickNameField.GetComponent<Image>().color = Color.white;

            string _ipAddress = ipAddressField.text;

            if (isIpAddressCorrect(_ipAddress))
            {
                ipAddressField.GetComponent<Image>().color = Color.white;

                int _port = StringToPort(portField.text);

                if (_port != 0)
                {
                    portField.GetComponent<Image>().color = Color.white;

                    Client.instance.UpdateAddressConnection(_ipAddress, _port);
                    Client.instance.ConnectToServer();
                }
                else
                {
                    portField.GetComponent<Image>().color = Color.red;
                }
            }
            else
            {
                ipAddressField.GetComponent<Image>().color = Color.red;
            }
        }
        else
        {
            PlayerNickNameField.GetComponent<Image>().color = Color.red;
        }
    }

    public bool isIpAddressCorrect(string _ipAddress)
    {
        int separatorsCount = 0;
        int numeralsInSeparators = 0;

        for (int i = 0; i < _ipAddress.Length; i++)
        {
            if (_ipAddress[i] >= '0' && _ipAddress[i] <= '9')
            {
                if (numeralsInSeparators > 3)
                {
                    return false;
                }
                else
                {
                    numeralsInSeparators++;
                }
            }
            else if (_ipAddress[i] == '.')
            {
                if (numeralsInSeparators == 0)
                {
                    return false;
                }
                else
                {
                    numeralsInSeparators = 0;
                    separatorsCount++;
                }
            }
            else
            {
                return false;
            }
        }
        
        if (separatorsCount == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int StringToPort(string _portString)
    {
        int _port = 0;

        for (int i = 0; i < _portString.Length; i++)
        {
            if (_portString[i] >= '0' && _portString[i] <= '9')
            {
                _port = _port * 10 + _portString[i] - '0';
            }
            else
            {
                return 0;
            }
        }

        if (_port >= 10000)
        {
            return _port;
        }
        else
        {
            return 0;
        }
    }

    public void ResetData()
    {
        GameManager.ResetGameManagerData();
        LobbyManager.instance.ResetLobbyUI();
        GameUI.instance.ResetGameUI();
        OpenConnectionMenu();
    }

    public void ExitFromTheServer()
    {
        GameManager.YouDisconnected();
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
            PlayerNickNameField.GetComponent<Image>().color = Color.white;

            int _port = StringToPort(portField.text);

            if (_port != 0)
            {
                portField.GetComponent<Image>().color = Color.white;

                ServerRunner _serverRunner = Instantiate(serverPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)).GetComponent<ServerRunner>();

                _serverRunner.port = _port;

                StartCoroutine(ConnectToServerAfterServerStart(_port));

                GameManager.isHost = true;

                AdressAndPortText.gameObject.SetActive(true);

                LobbyManager.instance.ShowStartGameButton();
                GameUI.instance.ShowNextTurnButton();
            }
            else
            {
                portField.GetComponent<Image>().color = Color.red;
            }
        }
        else
        {
            PlayerNickNameField.GetComponent<Image>().color = Color.red;
        }
    }

    IEnumerator ConnectToServerAfterServerStart(int _port)
    {
        yield return new WaitForSeconds(0.1f);
        if (ServerRunner.GetServerStatus())
        {
            Client.instance.UpdateAddressConnection("127.0.0.1", _port);
            Client.instance.ConnectToServer();

            StopCoroutine(ConnectToServerAfterServerStart(_port));
        }
        else
        {
            ConnectToServerAfterServerStart(_port);
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
