using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turn_Base_Strategy_Server
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
            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 0; i < Server.MaxPlayers; i++)
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
            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        public static void Welcome(int _toClient, string _message)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_message);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void Readiness(bool[] playersReadiness)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerReadiness))
            {
                _packet.Write(playersReadiness);

                SendTCPDataToAll(_packet);
            }
        }
        //Send BattleFieldInfo, StageInfo, TurnInfo, HeroesInfo

        //отправляем с свервера тестовый пакет по UDP. Причем отправление происходит при соединении в методе Connect в client.cs. То есть как только соединение с клиентом установлено, отправляется этот пакет.
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
