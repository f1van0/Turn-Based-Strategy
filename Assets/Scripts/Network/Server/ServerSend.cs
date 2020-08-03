using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network.Server
{
    class ServerSend
    {
        //Готовит пакет к отправке
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        private static void SendTCPDataToAllExistingPlayers(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (Server.clients[i].player != null)
                    Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAllExistingPlayers(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (Server.clients[i].player != null)
                    Server.clients[i].udp.SendData(_packet);
            }
        }

        public static void Welcome(int _toClient, string _message, int _gameStage)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_message);
                _packet.Write(_toClient);
                _packet.Write(_gameStage);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SendPlayerInfoToAllExistingPlayers(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerInfo))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.team);
                _packet.Write(_player.position);
                _packet.Write(_player.isReady);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendPlayerInfo(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerInfo))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.team);
                _packet.Write(_player.position);
                _packet.Write(_player.isReady);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SendPlayerNicknameToAllExistingPlayers(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerNickname))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendPlayerReadinessToAllExistingPlayers(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerReady))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.isReady);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendPlayerTeamToAllExistingPlayers(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerTeam))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.team);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendPlayerPositionToAllExistingPlayers(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.position);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendChatMessageToAllExistingPlayers(Player _player, string _message)
        {
            using (Packet _packet = new Packet((int)ServerPackets.chatMessage))
            {
                _packet.Write(_player.id);
                _packet.Write(_message);

                SendUDPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendGameStageToAllExistingPlayers(int gameStage)
        {
            using (Packet _packet = new Packet((int)ServerPackets.gameStage))
            {
                _packet.Write(gameStage);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendCellToAllExistingPlayers(CellValues _cell, bool isCellAvailable)
        {
            using (Packet _packet = new Packet((int)ServerPackets.cell))
            {
                _packet.Write(_cell);
                _packet.Write(isCellAvailable);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendBattleFieldToAllExistingPlayers(CellValues[,] battleground)
        {
            using (Packet _packet = new Packet((int)ServerPackets.battleground))
            {
                _packet.Write(battleground);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendSpawnHero(HeroValues _heroValues)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnHero))
            {
                _packet.Write(_heroValues);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendMoveHero(CellValues from_cellValues, CellValues to_cellValues)
        {
            using (Packet _packet = new Packet((int)ServerPackets.moveHero))
            {
                _packet.Write(from_cellValues);
                _packet.Write(to_cellValues);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void SendAvailableCells(int _toClient, Vector2[] _availableCells)
        {
            using (Packet _packet = new Packet((int)ServerPackets.availableCells))
            {
                _packet.Write(_availableCells);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SendTurnNumber(int _turnNumber)
        {
            using (Packet _packet = new Packet((int)ServerPackets.turnNumber))
            {
                _packet.Write(_turnNumber);

                SendTCPDataToAllExistingPlayers(_packet);
            }
        }

        public static void UDPTest(int _toClient)
        {
            //Формируем и отправляем пакет, создавая новый с ServerPackets, который соответствует типу пакета, в нашем случае udpTest
            using (Packet _packet = new Packet((int)ServerPackets.udpTest))
            {
                _packet.Write("[From_server_to_client] A test packet for UDP");

                SendUDPData(_toClient, _packet);
            }
        }
    }
}
