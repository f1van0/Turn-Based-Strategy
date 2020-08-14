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

            GameManager.AddNewLocalMessage($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}, his nickname is {_username}.", MessageType.fromServer);

            if (_fromClient != _clientIdCheck)
            {
                GameManager.AddNewLocalMessage($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck}) (from ServerHandle.cs, WelcomeRecevied)", MessageType.fromServer);
            }

            //Сообщаем всем игрокам о его появлении.
            Server.clients[_fromClient].InitializePlayerInGameFromServer(_username, 0, new Vector2Int(-1, -1), false);
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();

            GameManager.AddNewLocalMessage($"Received packet via UDP. Contains message: {_msg}", MessageType.fromServer);
        }

        //Принимаем информацию
        public static void GetPlayerInfo(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();
            int _team = _packet.ReadInt();
            Vector2Int _position = _packet.ReadVector2Int();
            bool _isReady = _packet.ReadBool();

            Server.clients[_fromClient].player = new Player(_fromClient, _username, _team, _isReady);
            //отправляем полученную информацию об игроке _fromClient или _clientIdCheck (одно и то же должно быть) всем игрокам, включая него в знак, что инфа на сервере и у клиентов, а значит можно её менять и у себя на компе (у игрока) (например что-то вывести в меню)
            ServerSend.SendPlayerInfoToAllExistingPlayers(Server.clients[_fromClient].player);
        }

        public static void GetPlayerReady(int _fromClient, Packet _packet)
        {
            Server.clients[_fromClient].player.isReady = !Server.clients[_fromClient].player.isReady;
            bool _isAllTeammatesAreReady = true;
            if (GameManager.gameStage == 0)
            {
                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.player != null)
                    {
                        if (!_client.player.isReady)
                        {
                            _isAllTeammatesAreReady = false;
                            break;
                        }
                    }
                }
            }
            else if (GameManager.gameStage == 1)
            {
                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.player != null)
                    {
                        //Part of code for final version, where only a specific team of players can switch the turn
                        if (((_client.player.team - ServerSideComputing.turnNumber) % 2 == 0) && (!_client.player.isReady))
                        {
                            _isAllTeammatesAreReady = false;
                            break;
                        }
                    }
                }
            }

            if (_isAllTeammatesAreReady)
            {
                if (ServerSideComputing.gameStage == 0)
                {
                    ServerSideComputing.StartGame();
                }
                else if (ServerSideComputing.gameStage == 1)
                {
                    ServerSideComputing.ToNextTurn();
                }
            }
            else
            {
                //отправляем полученную информацию об игроке _fromClient или _clientIdCheck (одно и то же должно быть) всем игрокам, включая него в знак, что инфа на сервере и у клиентов, а значит можно её менять и у себя на компе (у игрока) (например что-то вывести в меню)
                ServerSend.SendPlayerReadinessToAllExistingPlayers(Server.clients[_fromClient].player);
            }
        }

        public static void GetPlayerNickname(int _fromClient, Packet _packet)
        {
            string _nickname = _packet.ReadString();

            Server.clients[_fromClient].player.username = _nickname;
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
            Vector2Int _position = _packet.ReadVector2Int();

            ServerSideComputing.SetPlayerPosition(_fromClient, _position);
            
            //ServerSend.SendPlayerPositionToAllExistingPlayers(Server.clients[_fromClient].player);
        }

        public static void GetChatMessage(int _fromClient, Packet _packet)
        {
            string _message = _packet.ReadString();

            ServerSend.SendChatMessageToAllExistingPlayers(Server.clients[_fromClient].player, _message);
        }

        public static void GetMoveHero(int _fromClient, Packet _packet)
        {
            int _heroId = _packet.ReadInt();
            Vector2Int _moveToPosition = _packet.ReadVector2Int();

            ServerSideComputing.MoveHero(_heroId, _moveToPosition);
        }

        public static void GetAttackHero(int _fromClient, Packet _packet)
        {
            int _attackingHeroId = _packet.ReadInt();
            int _attackedHeroId = _packet.ReadInt();

            ServerSideComputing.AttackHero(_attackingHeroId, _attackedHeroId);
        }

        /*
        public static void GetHeroAction(int _fromClient, Packet _packet)
        {
            int _heroId = _packet.ReadInt();
            Vector2 _currentHeroPosition = _packet.ReadVector2();
            Vector2 _actionPosition = _packet.ReadVector2();

            ServerSideComputing.ActionHero(_heroId, _currentHeroPosition, _actionPosition);
        }
        */
        public static void GetRequestToShowAvailableCells(int _fromClient, Packet _packet)
        {
            Vector2Int _heroPosition = _packet.ReadVector2Int();

            ServerSideComputing.ShowAccesibleCellsByWave(_fromClient, _heroPosition);
        }
    }
}
