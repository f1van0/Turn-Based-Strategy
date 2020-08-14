using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Assets.Scripts.Network.Server
{
    class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;


        public static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            GameManager.AddNewLocalMessage($"Starting server.", MessageType.fromServer);

            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(UDPReceiveCallBack, null);

            GameManager.AddNewLocalMessage($"Server start on {Port}.", MessageType.fromServer);
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            GameManager.AddNewLocalMessage($"Incoming connection from {_client.Client.RemoteEndPoint} ...", MessageType.fromServer);

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            GameManager.AddNewLocalMessage($"{_client.Client.RemoteEndPoint} failed to connect: Server is full", MessageType.fromServer);
        }

        private static void UDPReceiveCallBack(IAsyncResult _result)
        {
            try
            {
                //Возвращает те байты, которые получили, а также устанавливает конечную точку IP
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallBack, null);

                if (_data.Length < 4)
                {
                    return;
                }
                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _exception)
            {
                GameManager.AddNewLocalMessage($"Error receiving UDP data: {_exception}", MessageType.fromServer);
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _exception)
            {
                GameManager.AddNewLocalMessage($"Error sending data to {_clientEndPoint} via UDP: {_exception}", MessageType.fromServer);
            }
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                //KeyValuePair<int, PacketHandler>
                //[(int)ClientPackets.welcomeReceived]=ServerHandle.WelcomeReceived
                //Значению сопоставляется 
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.udpTestReceived, ServerHandle.UDPTestReceived },
                { (int)ClientPackets.playerInfoReceived, ServerHandle.GetPlayerInfo },
                { (int)ClientPackets.playerNicknameReceived, ServerHandle.GetPlayerNickname },
                { (int)ClientPackets.playerReadinessReceived, ServerHandle.GetPlayerReady },
                { (int)ClientPackets.playerPositionReceived, ServerHandle.GetPlayerPosition },
                { (int)ClientPackets.playerTeamReceived, ServerHandle.GetPlayerTeam },
                { (int)ClientPackets.chatMessageReceived, ServerHandle.GetChatMessage },
                { (int)ClientPackets.moveHeroReceived, ServerHandle.GetMoveHero },
                { (int)ClientPackets.availableCellsReceived, ServerHandle.GetRequestToShowAvailableCells },
                { (int)ClientPackets.attackHeroReceived, ServerHandle.GetAttackHero }
            };
            GameManager.AddNewLocalMessage("Initialized packets.", MessageType.fromServer);
        }
    }
}
