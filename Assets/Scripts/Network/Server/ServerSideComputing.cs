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

        // - Cells
        public static CellValues[,] battlefield = new CellValues[cols, rows];
        public static CellValues[] cellPresets = new CellValues[1];

        // - HeroValues
        public static Dictionary<int, HeroValues> heroes = new Dictionary<int, HeroValues>();
        public static HeroValues[] heroPresets = new HeroValues[1];

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

            foreach (HeroValues _hero in heroes.Values)
            {
                if ((_hero.team - turnNumber) % 2 == 0)
                {
                    _hero.energy = _hero.defaultEnergy;
                }
                else
                {
                    _hero.energy = 0;
                }
                ServerSend.SendHeroValues(_hero);
            }

            SetAllPlayersReady(false);

            ServerSend.SendTurnNumber(turnNumber);
        }

        //TODO: part of refactoring. spawn battlefield based on cellValues presets
        public static void GenerateBattleField()
        {
            //creating a field while just creating cells without any kind of presets
            //presets for a whole map can be made
            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    battlefield[i, j] = new CellValues(new Vector2Int(i, j));
                }
            }
            ServerSend.SendBattleFieldToAllExistingPlayers(battlefield);
        }

        //TODO: part of refactoring. make more func, based on new CellValues and HeroValues
        public static void ShowAccesibleCellsByWave(int _toClient, Vector2Int _heroPosition)
        {
            int CountOfCurrentNodes = 1;
            int CountOfNewNodes = 0;
            int CountOfAllNodes = 1;
            int start = CountOfAllNodes - CountOfCurrentNodes;
            int io = _heroPosition.x;
            int jo = _heroPosition.y;
            int stepsCount = heroes[battlefield[_heroPosition.x, _heroPosition.y].heroId].energy;

            List<Vector2Int> nodes = new List<Vector2Int>();
            nodes.Add(_heroPosition);

            Vector2Int[] currentNodes = new Vector2Int[cols * rows];
            currentNodes[0] = new Vector2Int(io, jo);

            for (int len = 0; len < stepsCount; len++)
            {
                CountOfNewNodes = 0;
                for (int t = start; t < CountOfAllNodes; t++)
                {
                    int i = nodes[t].x;
                    int j = nodes[t].y;

                    CheckCellInAllDirections(i, j, t, ref nodes, CountOfAllNodes, ref CountOfNewNodes);
                }
                start = CountOfAllNodes;
                CountOfAllNodes += CountOfNewNodes;
                CountOfCurrentNodes = CountOfNewNodes;
            }

            ServerSend.SendAvailableCells(_toClient, nodes.ToArray());
        }

        public static void CheckCellInAllDirections(int x, int y, int t, ref List<Vector2Int> _nodes, int _countOfAllNodes, ref int _countOfNewNodes)
        {
            if (battlefield[x, y].heroId == -1 || t == 0)
            {
                if (x < cols - 1)
                {
                    CheckCellAvailability(x + 1, y, ref _nodes, _countOfAllNodes, ref _countOfNewNodes);
                }
                if (x > 0)
                {
                    CheckCellAvailability(x - 1, y, ref _nodes, _countOfAllNodes, ref _countOfNewNodes);
                }
                if (y < rows - 1)
                {
                    CheckCellAvailability(x, y + 1, ref _nodes, _countOfAllNodes, ref _countOfNewNodes);
                }
                if (y > 0)
                {
                    CheckCellAvailability(x, y - 1, ref _nodes, _countOfAllNodes, ref _countOfNewNodes);
                }
            }
        }
        
        public static void CheckCellAvailability(int x, int y, ref List<Vector2Int> _nodes, int _countOfAllNodes, ref int _countOfNewNodes)
        {
            if (battlefield[x, y].isCellEmpty && !_nodes.Contains(new Vector2Int(x, y)))
            {
                _nodes.Add(new Vector2Int(x, y));
                _countOfNewNodes++;
            }
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
                    if (_client.player.team == -1) // spectator
                    {

                    }
                    else
                    {
                        Vector2Int _spawnPosition;
                        if (_client.player.team == 1) //team1
                        {
                            _spawnPosition = new Vector2Int(rows / 2, _client.player.id);
                        }
                        else //team2
                        {
                            _spawnPosition = new Vector2Int(rows - 1, _client.player.id);
                        }

                        int _heroId = _client.player.id;

                        heroes.Add(_heroId, new HeroValues(_heroId, _spawnPosition, _client.player.username, _client.player.team));

                        battlefield[_spawnPosition.x, _spawnPosition.y].isCellEmpty = false;
                        battlefield[_spawnPosition.x, _spawnPosition.y].heroId = _heroId;

                        ServerSend.SendSpawnHero(heroes[_heroId]);
                    }
                }
            }
        }

        public static void MoveHero(int _heroId, Vector2Int _moveToPosition)
        {
            HeroValues _hero = heroes[_heroId];
            CellValues from_cellValues = battlefield[heroes[_heroId].position.x, heroes[_heroId].position.y];
            CellValues to_cellValues = battlefield[_moveToPosition.x, _moveToPosition.y];

            from_cellValues.isCellEmpty = true;
            from_cellValues.heroId = -1;

            battlefield[_moveToPosition.x, _moveToPosition.y].isCellEmpty = false;
            to_cellValues.heroId = _heroId;

            heroes[_heroId].position = _moveToPosition;

            ServerSend.SendMoveHero(_hero, from_cellValues, to_cellValues);
        }

        /* Removed by refactoring
        public static void MoveHero(int _heroId, CellValues _moveFrom, CellValues _moveTo)
        {

            _moveTo.SetHeroValues(_moveFrom.GetHeroValues());
            _moveTo.GetHeroValues().position = _moveTo.position;
            _moveFrom.SetHeroValues(new HeroValues(null));
        }
        */

        public static void AttackHero(int _attackingHeroId, int _attackedHeroId)
        {
            heroes[_attackingHeroId].GetDamage(heroes[_attackedHeroId]);
            ServerSend.SendAttackHero(_attackingHeroId, heroes[_attackedHeroId]);
            //_AttackedHero.GetHeroValues().GetDamage(_AttackingHero.GetHeroValues());
        }

        /* Removed by refactoring
        public static void AttackHero(int _heroId, CellValues _AttackingHero, CellValues _AttackedHero)
        {
            _AttackedHero.GetHeroValues().GetDamage(_AttackingHero.GetHeroValues());
        }
        */

        /*
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
        */

        public static void SetPlayerReadiness(ref Player _player)
        {
            _player.isReady = !_player.isReady;
        }

        public static void SetplayerUsername(ref Player _player, string _username)
        {
            _player.username = _username;
        }

        public static void SetPlayerPosition(int _fromClient, Vector2Int _position)
        {
            if (Server.clients[_fromClient].player.position == _position)
            {
                //ShowAccesibleCellsByWave(_position);
            }
            else
            {
                Vector2Int previous_position = Server.clients[_fromClient].player.position;
                battlefield[previous_position.x, previous_position.y].isCellEmpty = true;
                battlefield[_position.x, _position.y].isCellEmpty = false;
                ServerSend.SendPlayerPositionToAllExistingPlayers(Server.clients[_fromClient].player);
            }
            //Soon HeroStats, send Cell with HeroStats
            //battlefield[(int)_position.x, (int)_position.y].AddHeroOnCell(battlefield[(int)previous_position.x, (int)previous_position.y].GetHeroStats());
            //battlefield[(int)previous_position.x, (int)previous_position.y].DeleteHeroOnCell();
        }
    }
}
