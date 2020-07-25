using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Все пакеты и методы для них
using Assets.Scripts.Network.Server;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.GetUserName());

            SendTCPData(_packet);
        }
    }
    
    public static void UDPTestReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.udpTestReceived))
        {
            _packet.Write("Received a UDP packet.");

            SendUDPData(_packet);
        }
    }

    public static void SendPlayerInfo(string _username, int _team, Vector2 _position, bool _isReady)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerInfoReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(_username);
            _packet.Write(_team);
            _packet.Write(_position);
            _packet.Write(_isReady);

            SendTCPData(_packet);
        }
    }

    public static void SendPlayerUsername(string _username)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerNicknameReceived))
        {
            _packet.Write(_username);

            SendTCPData(_packet);
        }
    }

    public static void SendPlayerReady()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerReadinessReceived))
        {
            SendTCPData(_packet);
        }
    }

    public static void SendPlayerTeam(int _team)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerTeamReceived))
        {
            _packet.Write(_team);

            SendTCPData(_packet);
        }
    }

    public static void SendPlayerPosition(Vector2 _position)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerPositionReceived))
        {
            _packet.Write(_position);

            SendTCPData(_packet);
        }
    }

    public static void SendChatMessage(string _message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.chatMessageReceived))
        {
            _packet.Write(_message);

            SendUDPData(_packet);
        }
    }

}
