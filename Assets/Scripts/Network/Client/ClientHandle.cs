using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

//Все пакеты и методы для них
using Assets.Scripts.Network.Server;
using Assets.Scripts;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _message = _packet.ReadString();
        int _myId = _packet.ReadInt();
        int _gameStage = _packet.ReadInt();

        GameManager.AddNewLocalMessage($"Message from server: {_message}", MessageType.fromClient);
        Client.instance.myId = _myId;

        GameManager.SetLocalClientId(_myId);
        GameManager.SetGameStage(_gameStage);
        
        //Ответ серверу
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();

        GameManager.AddNewLocalMessage($"Received packet via UDP. Containsmessage: {_msg}", MessageType.fromClient);
        ClientSend.UDPTestReceived();
    }

    public static void GetPlayerInfo(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        int _team = _packet.ReadInt();
        Vector2Int _position = _packet.ReadVector2Int();
        bool _isReady = _packet.ReadBool();

        if (_id > GameManager.playersCount)
        {
            GameManager.AddNewPlayer(_id, _username, _team, _isReady);
        }
        else
        {
            GameManager.UpdateExsistingPlayer(_id, _username, _team, _isReady);
        }
    }

    public static void GetPlayerNickname(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _nickname = _packet.ReadString();

        GameManager.SetPlayerUsername(_id, _nickname);
    }

    public static void GetPlayerReady(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _isReady = _packet.ReadBool();

        GameManager.SetPlayerReady(_id, _isReady);
        if (_id == Client.instance.myId && GameManager.gameStage == 0)
        {
            GameManager.SetLocalPlayerReady(_isReady);
        }
    }

    public static void GetPlayerTeam(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _team = _packet.ReadInt();

        GameManager.SetPlayerTeam(_id, _team);
    }

    public static void GetPlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector2Int _position = _packet.ReadVector2Int();

        GameManager.SetPlayerPosition(_id, _position);
    }

    public static void GetChatMessage(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _message = _packet.ReadString();

        Chat.instance.AddNewMessage(_id, _message);
    }

    public static void GetGameStage(Packet _packet)
    {
        int _gameStage = _packet.ReadInt();

        GameManager.SetGameStage(_gameStage);
    }

    public static void GetBattleGround(Packet _packet)
    {
        CellValues[,] _battleground = _packet.ReadCellValuesArray();

        GameManager.InitializeBattlefield(_battleground);
    }

    public static void GetCell(Packet _packet)
    {
        CellValues _cellValues = _packet.ReadCellValues();

        GameManager.SetCell(_cellValues);
    }

    public static void GetSpawnHero(Packet _packet)
    {
        HeroValues _heroValues = _packet.ReadHeroValues();

        GameManager.SpawnHero(_heroValues);
    }

    public static void GetMoveHero(Packet _packet)
    {
        HeroValues _heroValues = _packet.ReadHeroValues();
        CellValues from_cellValues = _packet.ReadCellValues();
        CellValues to_cellValues = _packet.ReadCellValues();

        GameManager.MoveHero(_heroValues, from_cellValues, to_cellValues);
    }
    /*
    public static void GetActionHero(Packet _packet)
    {
        CellValues _current = _packet.ReadCellValues();
        CellValues _action = _packet.ReadCellValues();

        GameManager.ActionHero(_current, _action);
    }
    */
    public static void GetAttackHero(Packet _packet)
    {
        int _attackingHeroId = _packet.ReadInt();
        HeroValues _attackedHeroValues = _packet.ReadHeroValues();

        GameManager.AttackHero(_attackingHeroId, _attackedHeroValues);
    }

    public static void GetAvailableCells(Packet _packet)
    {
        Vector2Int[] _availableCells = _packet.ReadVector2IntArray();

        GameManager.ShowAvailableCells(_availableCells);
    }

    public static void GetHeroValues(Packet _packet)
    {
        HeroValues _heroValues = _packet.ReadHeroValues();

        GameManager.SetHeroValues(_heroValues);
    }

    public static void GetTurnNumber(Packet _packet)
    {
        int _turnNumber = _packet.ReadInt();

        GameManager.SetTurn(_turnNumber);
    }

    public static void GetCommand(Packet _packet)
    {
        int _clientId = _packet.ReadInt();
        int _messageCode = _packet.ReadInt();

        if (_messageCode == 0) // server is shutdowned
        {
            if (!GameManager.isHost) // if you not a server owner. (this can turn into infinity recursion) + if the server stopped, then the host already knows about it, because he either left the server or left the game
            {
                GameManager.YouDisconnected();
            }
        }
        else if (_messageCode == 1) // someone is disconnected
        {
            if (_clientId == GameManager.clientId) // if its you (kick or server shutdowned)
            {
                GameManager.YouDisconnected();
            }
            else
            {
                GameManager.PlayerDisconnected(_clientId);
            }
        }
    }
}
