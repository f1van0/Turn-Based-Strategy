using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

//Все пакеты и методы для них
using Assets.Scripts.Network.Server;

public class ClientHandle : MonoBehaviour
{
    private LobbyManager lobbyManager;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public static void Welcome(Packet _packet)
    {
        string _message = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_message}");
        Client.instance.myId = _myId;
        GameManager.GetClientId(_myId);
        //Ответ серверу
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Received packet via UDP. Containsmessage: {_msg}");
        ClientSend.UDPTestReceived();
    }

    public static void GetPlayerInfo(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        int _team = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        bool _isReady = _packet.ReadBool();

        if (_id > GameManager.playersCount)
        {
            GameManager.AddNewPlayerInLobby(_id, _username, _team, _isReady);
        }
        else
        {
            GameManager.UpdateExsistingPlayerInLobby(_id, _username, _team, _isReady);
        }
    }

    public static void GetPlayerNickname(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _nickname = _packet.ReadString();

        GameManager.GetPlayerUsername(_id, _nickname);
    }

    public static void GetPlayerReadiness(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _isReady = _packet.ReadBool();

        GameManager.GetPlayerReady(_id, _isReady);
        if (_id == Client.instance.myId)
        {
            GameManager.SetlocalPlayerReady();
        }
    }

    public static void GetPlayerTeam(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _team = _packet.ReadInt();

        GameManager.GetPlayerTeam(_id, _team);
    }

    public static void GetPlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();

        GameManager.SetPlayerPosition(_id, _position);
    }

    public static void GetChatMessage(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _nickname = _packet.ReadString();
        string _message = _packet.ReadString();

        Chat.instance.AddNewMessage(_id, _nickname, _message);
    }
}
