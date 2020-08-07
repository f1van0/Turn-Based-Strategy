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
        public static int turnNumber = 0;
        //Game
        public static CellValues[,] battlefield = new CellValues[cols, rows];
        public static CellValues[] presets = new CellValues[1];

        //TODO: part of refactoring. make it so that the player can connect not only during lobby gameStage
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
            ToNextTurn();
        }

        public static void SetAllPlayersReady(bool _isReady)
        {
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    _client.player.isReady = _isReady;
                    ServerSend.SendPlayerReadinessToAllExistingPlayers(_client.player);
                }
            }
        }

        public static void ToNextTurn()
        {
            turnNumber++;

            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    if (battlefield[i, j].GetHeroValues().ID != -1)
                    {
                        if ((battlefield[i, j].GetHeroValues().team - turnNumber) % 2 == 0)
                        {
                            battlefield[i, j].GetHeroValues().energy = battlefield[i, j].GetHeroValues().defaultEnergy;
                        }
                        else
                        {
                            battlefield[i, j].GetHeroValues().energy = 0;
                        }
                        ServerSend.SendCellToAllExistingPlayers(battlefield[i, j]);
                    }
                }
            }

            SetAllPlayersReady(false);

            ServerSend.SendTurnNumber(turnNumber);
        }

        //TODO: part of refactoring. spawn battlefield based on cellValues presets
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

        //TODO: part of refactoring. make more func, based on new CellValues and HeroValues
        public static void ShowAccesibleCellsByWave(int _toClient, Vector2 _heroPosition)
        {
            int CountOfCurrentNodes = 1;
            int CountOfNewNodes = 0;
            int CountOfAllNodes = 1;
            int newCount = 0;
            int start = CountOfAllNodes - CountOfCurrentNodes;
            int io = (int)_heroPosition.x;
            int jo = (int)_heroPosition.y;
            int stepsCount = battlefield[(int)_heroPosition.x, (int)_heroPosition.y].GetHeroValues().energy;
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
                    if (battlefield[i,j].GetHeroValues().ID == -1 || t == 0)
                    {
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

        //TODO: part of refactoring. Add hero presets, hero position
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

        public static void MoveHero(int _heroId, CellValues _moveFrom, CellValues _moveTo)
        {

            _moveTo.SetHeroValues(_moveFrom.GetHeroValues());
            _moveTo.GetHeroValues().position = _moveTo.position;
            _moveFrom.SetHeroValues(new HeroValues(null));
        }

        public static void AttackHero(int _heroId, CellValues _AttackingHero, CellValues _AttackedHero)
        {
            _AttackedHero.GetHeroValues().GetDamage(_AttackingHero.GetHeroValues());
        }

        public static void ActionHero(int _heroId, Vector2 _currentHeroPosition, Vector2 _actionPosition)
        {
            ref CellValues current_cellValues = ref battlefield[(int)_currentHeroPosition.x, (int)_currentHeroPosition.y];
            ref CellValues action_cellValues = ref battlefield[(int)_actionPosition.x, (int)_actionPosition.y];

            int _distanceOx = (int) Mathf.Abs(current_cellValues.position.x - action_cellValues.position.x);
            int _distanceOy = (int) Mathf.Abs(current_cellValues.position.y - action_cellValues.position.y);
            int _distance = (int)(_distanceOx + _distanceOy);

            //Уменьшаем энергию героя
            current_cellValues.GetHeroValues().energy = current_cellValues.GetHeroValues().energy - _distance;

            if (action_cellValues.GetHeroValues().ID == -1)
            {
                //MoveHero
                MoveHero(_heroId, current_cellValues, action_cellValues);
            }
            else
            {
                //AttackHero
                AttackHero(_heroId, current_cellValues, action_cellValues);
            }

            CellValues _currentHeroCell = battlefield[(int)_currentHeroPosition.x, (int)_currentHeroPosition.y];
            CellValues _actionHeroCell = battlefield[(int)_actionPosition.x, (int)_actionPosition.y];

            ServerSend.SendActionHero(_currentHeroCell, _actionHeroCell);
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
