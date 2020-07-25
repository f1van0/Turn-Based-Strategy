using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public enum Teams
    {
        Spectators = 0,
        Team1,
        Team2
    }
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static int playersCount = 0;
    public static int clientId = 0;

    public static void AddNewPlayerInLobby(int _id, string _username, int _team, bool _isReady)
    {
        LobbyManager.instance.AddNewPlayer(_id, _username, _team, _isReady);
    }

    public static void UpdateExsistingPlayerInLobby(int _id, string _username, int _team, bool _isReady)
    {
        LobbyManager.instance.UpdateExsistingPlayer(_id, _username, _team, _isReady);
    }

    public static void GetClientId(int _clientId)
    {
        clientId = _clientId;
    }

    public static void GetPlayerUsername(int _id, string _username)
    {
        players[_id].username = _username;
        LobbyManager.instance.SetPlayerUsername(_id, _username);
    }

    public static void SetLocalPlayerTeam(int team)
    {
        ClientSend.SendPlayerTeam(team);
    }

    public static void GetPlayerTeam(int _id, int _team)
    {
        players[_id].team = _team;
        LobbyManager.instance.SetPlayerTeam(_id, _team);
    }

    public static void SetPlayerPosition(int _playerId, Vector2 _position)
    {
        //TODO: pos
    }

    public static void SetlocalPlayerReady()
    {
        ClientSend.SendPlayerReady();
    }

    public static void GetLocalPlayerReady(int _id, bool _isReady)
    {
        LobbyManager.instance.SetReadiness_ButtonState_ForLocalPlayer(_isReady);
    }

    public static void GetPlayerReady(int _id, bool _isReady)
    {
        players[_id].isReady = _isReady;
        players[_id].SetReadyInLobby(_isReady);
    }
}
