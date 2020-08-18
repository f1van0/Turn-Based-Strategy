using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    /*
    TODO:
    Сделать отдельную отправку позиции, готовности, никнейма (смена ника после подключения), остальное потом (энергия, дамаг, лучше под это просто отправлять целый heroStats)
    */
    public static LobbyManager instance;

    public GameObject playerLobbyPrefab;

    public Transform spectatorsList;
    public Transform team1List;
    public Transform team2List;

    public Text playersCountInLobbyText;
    public Button buttonReady;
    public Button buttonJoinSpectators;
    public Button buttonJoinTeam1;
    public Button buttonJoinTeam2;
    public Button StartGameButton;

    public static Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();

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

    public void ResetLobbyUI()
    {
        foreach (GameObject _player in players.Values)
        {
            Destroy(_player.gameObject, 0);
        }

        players.Clear();

        StartGameButton.gameObject.SetActive(false);
    }

    public void ShowPlayersCount(int _playersCount)
    {
        playersCountInLobbyText.text = "Players in lobby: " + _playersCount;
    }

    public void SetPlayerTeam(int _id, int _team)
    {
        if (_team == 1)
        {
            players[_id].gameObject.transform.SetParent(team1List, false);
        }
        else if (_team == 2)
        {
            players[_id].gameObject.transform.SetParent(team2List, false);
        }
        else
        {
            players[_id].gameObject.transform.SetParent(spectatorsList, false);
        }
    }

    public void SetPlayerUsername(int _id, string _username)
    {
        players[_id].GetComponentInChildren<Text>().text = _username;
    }

    public void SetPlayerReady(int _id, bool _isReady)
    {
        if (_isReady)
        {
            players[_id].GetComponentInChildren<Image>().color = Color.green;
        }
        else
        {
            players[_id].GetComponentInChildren<Image>().color = Color.red;
        }
    }

    public void StartGame()
    {
        //TODO: start game
    }

    public void JoinSpectators()
    {
        buttonJoinSpectators.enabled = false;
        buttonJoinTeam1.enabled = true;
        buttonJoinTeam2.enabled = true;
        GameManager.SendLocalPlayerTeam(0);
    }

    public void JoinTeam1()
    {
        buttonJoinSpectators.enabled = true;
        buttonJoinTeam1.enabled = false;
        buttonJoinTeam2.enabled = true;
        GameManager.SendLocalPlayerTeam(1);
    }

    public void JoinTeam2()
    {
        buttonJoinSpectators.enabled = true;
        buttonJoinTeam1.enabled = true;
        buttonJoinTeam2.enabled = false;
        GameManager.SendLocalPlayerTeam(2);
    }

    public void SendReadiness_ForLocalPlayer()
    {
        GameManager.SendlocalPlayerReady();
    }

    public void SetReadiness_ButtonState_ForLocalPlayer(bool _isReady)
    {
        if (_isReady)
        {
            buttonReady.GetComponentInChildren<Text>().text = "Unready";
        }
        else
        {
            buttonReady.GetComponentInChildren<Text>().text = "Ready";
        }
    }

    public void AddNewPlayer(int _id, string _username, int _team, bool _isReady)
    {
        players[_id] = Instantiate(playerLobbyPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        UpdateExsistingPlayer(_id, _username, _team, _isReady);
    }

    public void UpdateExsistingPlayer(int _id, string _username, int _team, bool _isReady)
    {
        SetPlayerUsername(_id, _username);
        SetPlayerReady(_id, _isReady);
        SetPlayerTeam(_id, _team);
    }

    public void RemovePlayer(int _id)
    {
        Destroy(players[_id].gameObject, 0);
        players.Remove(_id);
    }

    public void ShowStartGameButton()
    {
        StartGameButton.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonJoinSpectators.enabled = false;

        StartGameButton.gameObject.SetActive(false);
        /*
        //Только хост может запустить игру
        ServerRunner _serverRunner;
        if (TryGetComponent<ServerRunner>(out _serverRunner))
            StartGameButton.gameObject.SetActive(true);
        else
            StartGameButton.gameObject.SetActive(false);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
