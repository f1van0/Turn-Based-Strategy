using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network.Server
{
    public enum GameStage
    {
        Lobby = 0,
        Game,
        End
    }

    class ServerSideComputing
    {
        public static int cols = 10;
        public static int rows = 10;
        public static int gameStage = 0;

        //Game
        public static CellValues[,] battleground = new CellValues[cols, rows];
        public static CellValues[] presets = new CellValues[1];

        public static void StartLobby()
        {
            gameStage = 0;
            ServerSend.SendGameStageToAllExistingPlayers(gameStage);
        }

        public static void StartGame()
        {
            gameStage = 1;
            ServerSend.SendGameStageToAllExistingPlayers(gameStage);
            GenerateBattleGround();
            SpawnPlayers();
        }



        public static void GenerateBattleGround()
        {
            //creating a field while just creating cells without any kind of presets
            //map presets can be made
            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    battleground[i, j] = new CellValues();
                }
            }
            ServerSend.SendBattleGroundToAllExistingPlayers(battleground);
        }

        public static void SpawnPlayers()
        {
            //So far players are spawning in line, depending on their team
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    if (_client.player.team == 1) //team1
                    {
                        _client.player.position = new Vector2(0, _client.player.id);
                    }
                    else if (_client.player.team == 2) //team2
                    {
                        _client.player.position = new Vector2(rows - 1, _client.player.id);
                    }
                    else //spectators
                    {
                        _client.player.position = new Vector2(-1000f, -1000f);
                    }
                    ServerSend.SendPlayerPositionToAllExistingPlayers(_client.player);
                }
            }
        }

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
