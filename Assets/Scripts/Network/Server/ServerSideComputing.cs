using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network.Server
{
    class ServerSideComputing
    {
        public void SetPlayerReadiness(ref Player _player)
        {
            _player.isReady = !_player.isReady;
        }

        public void SetplayerNickname(ref Player _player, string _nickname)
        {
            _player.nickname = _nickname;
        }

        public void SetplayerPosition(ref Player _player, Vector2 _position)
        {
            _player.position = _position;
        }
    }
}
