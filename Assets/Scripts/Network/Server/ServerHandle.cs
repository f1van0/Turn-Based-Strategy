using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Turn_Base_Strategy_Server
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
                Debug.Log($"[Server] Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck}) (Из ServerHandle.cs)");
            }
            //TODO: Send player into game
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();

            Debug.Log($"[Server] Received packet via UDP. Contains message: {_msg}");
        }

        public static void UPM_ReceivedReconizer(int _fromClient, Packet _packet)
        {
            int id = _packet.ReadInt();
            //Идем в свиче по существующим стейтам подбираем, читаем выполняем опред логику с этим
            switch (id)
            {
                case (int)UPM.cellState:
                    {
                        int cellState = _packet.ReadInt();
                        //Какая-нибудь функция
                        break;
                    }
                //...
                default:
                    {
                        //Что-нибудь, например функция
                        break;
                    }
            }
        }
    }
}
