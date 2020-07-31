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
        public Player(int _id, string _username, int _team, Vector2 _position, bool _isReady)
        {
            id = _id;
            username = _username;
            team = _team;
            position = _position;
            isReady = _isReady;
        }

        public Player(int _id, string _username, int _team, bool _isReady)
        {
            id = _id;
            username = _username;
            team = _team;
            position = new Vector2(-1f, -1f);
            isReady = _isReady;
        }

        public int id;
        public string username;
        public int team;
        public Vector2 position;
        public bool isReady;
    }
}
