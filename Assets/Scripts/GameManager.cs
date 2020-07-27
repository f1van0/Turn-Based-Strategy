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

    public static void AddNewPlayer(int _id, string _username, int _team, bool _isReady)
    {
        players.Add(_id, new PlayerManager(_username, _team, _isReady));
        LobbyManager.instance.AddNewPlayer(_id, _username, _team, _isReady);
        playersCount++;
        LobbyManager.instance.ShowPlayersCount(playersCount);
    }

    public static void StartGame()
    {
        //TODO: start game
    }

    public static void UpdateExsistingPlayer(int _id, string _username, int _team, bool _isReady)
    {
        players[_id] = new PlayerManager(_username, _team, _isReady);
        LobbyManager.instance.UpdateExsistingPlayer(_id, _username, _team, _isReady);
    }

    public static void SetLocalClientId(int _clientId)
    {
        clientId = _clientId;
    }

    public static void SetPlayerUsername(int _id, string _username)
    {
        players[_id].username = _username;
        LobbyManager.instance.SetPlayerUsername(_id, _username);
    }

    public static void SendLocalPlayerTeam(int team)
    {
        ClientSend.SendPlayerTeam(team);
    }

    public static void SetPlayerTeam(int _id, int _team)
    {
        players[_id].team = _team;
        LobbyManager.instance.SetPlayerTeam(_id, _team);
    }

    public static void SetPlayerPosition(int _playerId, Vector2 _position)
    {
        //TODO: pos
    }

    public static void SendlocalPlayerReady()
    {
        ClientSend.SendPlayerReady();
    }

    public static void SetLocalPlayerReady(bool _isReady)
    {
        LobbyManager.instance.SetReadiness_ButtonState_ForLocalPlayer(_isReady);
    }

    public static void SetPlayerReady(int _id, bool _isReady)
    {
        players[_id].isReady = _isReady;
        LobbyManager.instance.SetPlayerReady(_id, _isReady);
    }
}
