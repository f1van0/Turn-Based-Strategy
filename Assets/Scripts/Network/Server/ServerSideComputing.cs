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
        public static CellValues[,] battlefield = new CellValues[cols, rows];
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
            GenerateBattleField();
            SpawnHeroes();
        }



        public static void GenerateBattleField()
        {
            //creating a field while just creating cells without any kind of presets
            //map presets can be made
            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    battlefield[i, j] = new CellValues(new Vector2(i, j));
                }
            }
            ServerSend.SendBattleFieldToAllExistingPlayers(battlefield);
        }

        public static void ShowAccesibleCellsByWave(int _toClient, Vector2 _heroPosition)
        {
            int CountOfCurrentNodes = 1;
            int CountOfNewNodes = 0;
            int CountOfAllNodes = 1;
            int newCount = 0;
            int start = CountOfAllNodes - CountOfCurrentNodes;
            int io = (int)_heroPosition.x;
            int jo = (int)_heroPosition.y;
            int stepsCount = 2;
            //Soon
            //int stepsCount = battlefield[io, jo].GetHeroStats().GetHeroEnergy();

            Vector2[] currentNodes = new Vector2[cols * rows];
            currentNodes[0] = new Vector2(io, jo);

            for (int len = 0; len < stepsCount; len++)
            {
                CountOfNewNodes = 0;
                for (int t = start; t < CountOfAllNodes; t++)
                {
                    newCount = 0;
                    int i = Mathf.RoundToInt(currentNodes[t].x);
                    int j = Mathf.RoundToInt(currentNodes[t].y);

                    if (i < cols - 1)
                    {
                        if (battlefield[i + 1, j].isCellEmpty)
                        {
                            currentNodes[CountOfAllNodes + CountOfNewNodes] = new Vector2(i + 1, j);
                            //ServerSend.SendCellToAllExistingPlayers(battlefield[i + 1, j], true);
                            newCount++;
                            CountOfNewNodes++;
                        }
                        else
                        {
                            //ServerSend.SendCellToAllExistingPlayers(battlefield[i + 1, j], false);
                        }
                    }
                    if (i > 0)
                    {
                        if (battlefield[i - 1, j].isCellEmpty)
                        {
                            currentNodes[CountOfAllNodes + CountOfNewNodes] = new Vector2(i - 1, j);
                            //ServerSend.SendCellToAllExistingPlayers(battlefield[i - 1, j], true);
                            newCount++;
                            CountOfNewNodes++;
                        }
                        else
                        {
                            //ServerSend.SendCellToAllExistingPlayers(battlefield[i - 1, j], false);
                        }
                    }
                    if (j < rows - 1)
                    {
                        if (battlefield[i, j + 1].isCellEmpty)
                        {
                            currentNodes[CountOfAllNodes + CountOfNewNodes] = new Vector2(i, j + 1);
                            //ServerSend.SendCellToAllExistingPlayers(battlefield[i, j + 1], true);
                            newCount++;
                            CountOfNewNodes++;
                        }
                        else
                        {
                            //ServerSend.SendCellToAllExistingPlayers(battlefield[i, j + 1], false);
                        }
                    }
                    if (j > 0)
                    {
                        if (battlefield[i, j - 1].isCellEmpty)
                        {
                            currentNodes[CountOfAllNodes + CountOfNewNodes] = new Vector2(i, j - 1);
                            //ServerSend.SendCellToAllExistingPlayers(battlefield[i, j - 1], true);
                            newCount++;
                            CountOfNewNodes++;
                        }
                        else
                        {
                            //ServerSend.SendCellToAllExistingPlayers(battlefield[i, j - 1], false);
                        }
                    }
                    //сделать шаги в стороны
                    //очистить текущие
                    //проверить были ли они в истории
                    //добавить полученные в текущие
                }
                start = CountOfAllNodes;
                CountOfAllNodes += CountOfNewNodes;
                CountOfCurrentNodes = CountOfNewNodes;
            }

            Vector2[] nodes = new Vector2[CountOfAllNodes];
            for (int i = 0; i < CountOfAllNodes; i++)
            {
                nodes[i] = currentNodes[i];
            }

            ServerSend.SendAvailableCells(_toClient, nodes);
        }

        public static void SpawnHeroes()
        {
            //So far players are spawning in line, depending on their team
            //Hero's id equals player's id. Hero's owner = player's username
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    Vector2 _spawnPosition;
                    if (_client.player.team == 1) //team1
                    {
                        _spawnPosition = new Vector2(0, _client.player.id);
                        battlefield[(int)_spawnPosition.x, (int)_spawnPosition.y].SpawnHero(_client.player.id, _spawnPosition, _client.player.username, _client.player.team);
                        //_client.player.position = new Vector2(0, _client.player.id);
                        ServerSend.SendSpawnHero(battlefield[(int)_spawnPosition.x, (int)_spawnPosition.y].GetHeroValues());
                    }
                    else if (_client.player.team == 2) //team2
                    {
                        _spawnPosition = new Vector2(rows - 1, _client.player.id);
                        battlefield[(int)_spawnPosition.x, (int)_spawnPosition.y].SpawnHero(_client.player.id, _spawnPosition, _client.player.username, _client.player.team);
                        //_client.player.position = new Vector2(rows - 1, _client.player.id);
                        ServerSend.SendSpawnHero(battlefield[(int)_spawnPosition.x, (int)_spawnPosition.y].GetHeroValues());
                    }
                    else //spectators
                    {
                        
                    }
                }
            }
        }

        public static void MoveHero(int _heroId, Vector2 _moveFromPosition, Vector2 _moveToPosition)
        {
            ref CellValues from_cellValues = ref battlefield[(int)_moveFromPosition.x, (int)_moveFromPosition.y];
            ref CellValues to_cellValues = ref battlefield[(int)_moveToPosition.x, (int)_moveToPosition.y];

            to_cellValues.SetHeroValues(from_cellValues.GetHeroValues());
            to_cellValues.GetHeroValues().position = _moveToPosition;
            from_cellValues.SetHeroValues(new HeroValues(null));

            ServerSend.SendMoveHero(from_cellValues, to_cellValues);
        }

        public static void SetPlayerReadiness(ref Player _player)
        {
            _player.isReady = !_player.isReady;
        }

        public static void SetplayerNickname(ref Player _player, string _nickname)
        {
            _player.username = _nickname;
        }

        public static void SetPlayerPosition(int _fromClient, Vector2 _position)
        {
            if (Server.clients[_fromClient].player.position == _position)
            {
                //ShowAccesibleCellsByWave(_position);
            }
            else
            {
                Vector2 previous_position = Server.clients[_fromClient].player.position;
                battlefield[(int)previous_position.x, (int)previous_position.y].isCellEmpty = true;
                battlefield[(int)_position.x, (int)_position.y].isCellEmpty = false;
                ServerSend.SendPlayerPositionToAllExistingPlayers(Server.clients[_fromClient].player);
            }
            //Soon HeroStats, send Cell with HeroStats
            //battlefield[(int)_position.x, (int)_position.y].AddHeroOnCell(battlefield[(int)previous_position.x, (int)previous_position.y].GetHeroStats());
            //battlefield[(int)previous_position.x, (int)previous_position.y].DeleteHeroOnCell();
        }
    }
}
