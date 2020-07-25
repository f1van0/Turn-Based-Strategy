using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Teams
    {
        Spectators = 0,
        Team1,
        Team2
    }

    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public int playersCount = 0;
    public int clientId = 0;

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

    public void AddNewPlayerInLobby(int _id, string _username, int _team, bool _isReady)
    {
        LobbyManager.instance.AddNewPlayer(_id, _username, _team, _isReady);
    }

    public void UpdateExsistingPlayerInLobby(int _id, string _username, int _team, bool _isReady)
    {
        LobbyManager.instance.UpdateExsistingPlayer(_id, _username, _team, _isReady);
    }

    public void GetClientId(int _clientId)
    {
        clientId = _clientId;
    }

    public void GetPlayerUsername(int _id, string _username)
    {
        players[_id].username = _username;
        LobbyManager.instance.SetPlayerUsername(_id, _username);
    }

    public void SetLocalPlayerTeam(int team)
    {
        ClientSend.SendPlayerTeam(team);
    }

    public void GetPlayerTeam(int _id, int _team)
    {
        players[_id].team = _team;
        LobbyManager.instance.SetPlayerTeam(_id, _team);
    }

    public void SetPlayerPosition(int _playerId, Vector2 _position)
    {
        //TODO: pos
    }

    public void SetlocalPlayerReady()
    {
        ClientSend.SendPlayerReady();
    }

    public void GetLocalPlayerReady(int _id, bool _isReady)
    {
        LobbyManager.instance.SetReadiness_ButtonState_ForLocalPlayer(_isReady);
    }

    public void GetPlayerReady(int _id, bool _isReady)
    {
        players[_id].isReady = _isReady;
        players[_id].SetReadyInLobby(_isReady);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
