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

    public static void SendPlayerInfo(string _username, int _team, Vector2Int _position, bool _isReady)
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

    public static void SendPlayerPosition(Vector2Int _position)
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

    public static void SendMoveHero(int _heroId, Vector2Int _moveToPostion)
    {
        using (Packet _packet = new Packet((int)ClientPackets.moveHeroReceived))
        {
            _packet.Write(_heroId);
            _packet.Write(_moveToPostion);

            SendTCPData(_packet);
        }
    }
    public static void SendAttackHero(int _attackingHeroId, int _attackedHeroId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.attackHeroReceived))
        {
            _packet.Write(_attackingHeroId);
            _packet.Write(_attackedHeroId);

            SendTCPData(_packet);
        }
    }

    public static void SendAvailableCells(Vector2Int _heroPosition)
    {
        using (Packet _packet = new Packet((int)ClientPackets.availableCellsReceived))
        {
            _packet.Write(_heroPosition);

            SendTCPData(_packet);
        }
    }
}
