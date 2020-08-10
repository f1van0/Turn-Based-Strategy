using Assets.Scripts;
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

    public static int gameStage = 0;

    public static void AddNewPlayer(int _id, string _username, int _team, bool _isReady)
    {
        players.Add(_id, new PlayerManager(_username, _team, _isReady));
        LobbyManager.instance.AddNewPlayer(_id, _username, _team, _isReady);
        playersCount++;
        LobbyManager.instance.ShowPlayersCount(playersCount);
    }

    public static void SetGameStage(int _gameStage)
    {
        if (_gameStage == 0)
        {
            StartLobby();
            gameStage = 0;
        }
        else
        {
            StartGame();
            gameStage = 1;
        }
    }

    public static void StartLobby()
    {
        //TODO: start lobby
        UIManager.instance.OpenLobbyMenu();
    }

    public static void StartGame()
    {
        //TODO: start game
        UIManager.instance.OpenGameUI();
    }

    public static void InitializeBattlefield(CellValues[,] _battleground)
    {
        //TODO: initialize battleground
        BattleFieldManager.instance.SpawnBattlefield(_battleground);
    }

    //public static void SetCellInfo(CellValues _cell, bool _isCellAvailable)
    //{
    //    BattleFieldManager.instance.SetCellInfo(_cell, _isCellAvailable);
    //}

    public static void SetCell(CellValues _cell)
    {
        BattleFieldManager.instance.SetCell(_cell);
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
        //BattleFieldManager.instance.GetHero(_playerId, _position);
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
        if (gameStage == 0)
        {
            LobbyManager.instance.SetPlayerReady(_id, _isReady);
        }
        else if (gameStage == 0)
        {
            GameUI.instance.readyButton.interactable = false;
        }
    }

    public static void SpawnHero(HeroValues _heroValues)
    {
        BattleFieldManager.instance.SpawnHero(_heroValues);
    }

    public static void MoveHero(HeroValues _heroValues, CellValues from_cellValues, CellValues to_cellValues)
    {
        BattleFieldManager.instance.MoveHero(_heroValues, from_cellValues, to_cellValues);
    }

    public static void SendMoveHero(int _heroId, Vector2Int _moveToPosition)
    {
        ClientSend.SendMoveHero(_heroId, _moveToPosition);
    }

    public static void SendAttackHero(int _attackingHeroId, int _attackedHeroId)
    {
        ClientSend.SendAttackHero(_attackingHeroId, _attackedHeroId);
    }
    /*
    public static void ActionHero(CellValues _current, CellValues _action)
    {
        BattleFieldManager.instance.ActionHero(_current, _action);
    }
    */
    public static void AttackHero(int _attackingHeroId, HeroValues _attackedHeroValues)
    {
        BattleFieldManager.instance.AttackHero(_attackingHeroId, _attackedHeroValues);
    }

    public static void ShowAvailableCells( Vector2Int[] _availableCells)
    {
        BattleFieldManager.instance.ShowAvailableCells(_availableCells);
    }

    public static void SetHeroValues(HeroValues _heroValues)
    {
        BattleFieldManager.instance.SetHeroValues(_heroValues);
    }

    public static void SetTurn(int _turnNumber)
    {
        GameUI.instance.SetTurnsNumber(_turnNumber);
        BattleFieldManager.instance.ClearAvailableCells();
    }
}
