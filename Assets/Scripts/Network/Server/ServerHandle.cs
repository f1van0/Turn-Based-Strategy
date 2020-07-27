using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network.Server
{
    class ServerHandle
    {


        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            //Необходмо убедится, что данные считываются в том же порядке, в котором мы написали их в пакет
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Debug.Log($"[Server] {Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}, his nickname is {_username}.");

            if (_fromClient != _clientIdCheck)
            {
                Debug.Log($"[Server] Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck}) (from ServerHandle.cs, WelcomeRecevied)");
            }

            //Сообщаем всем игрокам о его появлении.
            Server.clients[_fromClient].InitializePlayerInGameFromServer(_username, 0, new Vector2(-1f, -1f), false);
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();

            Debug.Log($"[Server] Received packet via UDP. Contains message: {_msg}");
        }

        //Принимаем информацию
        public static void GetPlayerInfo(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();
            int _team = _packet.ReadInt();
            Vector2 _position = _packet.ReadVector2();
            bool _isReady = _packet.ReadBool();

            Server.clients[_fromClient].player = new Player(_fromClient, _username, _team, _isReady);
            //отправляем полученную информацию об игроке _fromClient или _clientIdCheck (одно и то же должно быть) всем игрокам, включая него в знак, что инфа на сервере и у клиентов, а значит можно её менять и у себя на компе (у игрока) (например что-то вывести в меню)
            ServerSend.SendPlayerInfoToAllExistingPlayers(Server.clients[_fromClient].player);
        }

        public static void GetPlayerReady(int _fromClient, Packet _packet)
        {
            Server.clients[_fromClient].player.isReady = !Server.clients[_fromClient].player.isReady;
            //отправляем полученную информацию об игроке _fromClient или _clientIdCheck (одно и то же должно быть) всем игрокам, включая него в знак, что инфа на сервере и у клиентов, а значит можно её менять и у себя на компе (у игрока) (например что-то вывести в меню)
            ServerSend.SendPlayerReadinessToAllExistingPlayers(Server.clients[_fromClient].player);
        }

        public static void GetPlayerNickname(int _fromClient, Packet _packet)
        {
            string _nickname = _packet.ReadString();

            Server.clients[_fromClient].player.nickname = _nickname;
            //отправляем полученную информацию об игроке _fromClient или _clientIdCheck (одно и то же должно быть) всем игрокам, включая него в знак, что инфа на сервере и у клиентов, а значит можно её менять и у себя на компе (у игрока) (например что-то вывести в меню)
            ServerSend.SendPlayerNicknameToAllExistingPlayers(Server.clients[_fromClient].player);
        }

        public static void GetPlayerTeam(int _fromClient, Packet _packet)
        {
            int _team = _packet.ReadInt();

            Server.clients[_fromClient].player.team = _team;
            //отправляем полученную информацию об игроке _fromClient или _clientIdCheck (одно и то же должно быть) всем игрокам, включая него в знак, что инфа на сервере и у клиентов, а значит можно её менять и у себя на компе (у игрока) (например что-то вывести в меню)
            ServerSend.SendPlayerTeamToAllExistingPlayers(Server.clients[_fromClient].player);
        }

        public static void GetPlayerPosition(int _fromClient, Packet _packet)
        {
            Vector2 _position = _packet.ReadVector2();

            Server.clients[_fromClient].player.position = _position;
            //отправляем полученную информацию об игроке _fromClient или _clientIdCheck (одно и то же должно быть) всем игрокам, включая него в знак, что инфа на сервере и у клиентов, а значит можно её менять и у себя на компе (у игрока) (например что-то вывести в меню)
            ServerSend.SendPlayerPositionToAllExistingPlayers(Server.clients[_fromClient].player);
        }

        public static void GetChatMessage(int _fromClient, Packet _packet)
        {
            string _message = _packet.ReadString();

            ServerSend.SendChatMessageToAllExistingPlayers(Server.clients[_fromClient].player, _message);
        }
    }
}
