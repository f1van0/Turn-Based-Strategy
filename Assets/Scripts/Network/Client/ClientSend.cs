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

    public static void SendPlayerInfo()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerInfoReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.GetUserName());
            _packet.Write(LobbyManager.instance.position);
            _packet.Write(LobbyManager.instance.isLocalPlayerReady);

            SendTCPData(_packet);
        }
    }

    public static void SendPlayerNickname()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerNicknameReceived))
        {
            _packet.Write(UIManager.instance.GetUserName());

            SendTCPData(_packet);
        }
    }

    public static void SendPlayerReadiness()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerReadyReceived))
        {
            _packet.Write(LobbyManager.instance.isLocalPlayerReady);

            SendTCPData(_packet);
        }
    }

    public static void SendPlayerPosition()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerPositionReceived))
        {
            _packet.Write(LobbyManager.instance.position);

            SendTCPData(_packet);
        }
    }
}
