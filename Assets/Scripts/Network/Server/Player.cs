using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network.Server
{
    public class Player
    {
        public Player(int _id, string _username, Vector2 _position, bool _isReady)
        {
            id = _id;
            nickname = _username;
            position = _position;
            isReady = _isReady;
        }

        public int id;
        public string nickname;
        public Vector2 position;
        public bool isReady;
    }
}
