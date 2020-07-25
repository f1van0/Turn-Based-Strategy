using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network.Server
{
    public enum Team
    {
        Spectators = 0,
        Team1,
        Team2
    }

    public class Player
    {
        public Player(int _id, string _username, int _team, Vector2 _position, bool _isReady)
        {
            id = _id;
            nickname = _username;
            team = _team;
            position = _position;
            isReady = _isReady;
        }



        public int id;
        public string nickname;
        public int team;
        public Vector2 position;
        public bool isReady;
    }
}
