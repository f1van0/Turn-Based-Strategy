using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CellValues
    {
        public string locationName = "Hills";
        public int damagePerTurn = 1;
        public int healthPerTurn = 1;
        public int energyPerTurn = 0;
        public Vector2Int position; // Позиция не в прямом смысле, здесь скорее индекс клетки, чтобы узнать положение этой клетки относительно других
        public bool isCellEmpty = true;
        public int heroId = -1;

        public CellValues()
        {
            locationName = "Hills";
            damagePerTurn = 1;
            healthPerTurn = 1;
            energyPerTurn = 0;
            position = new Vector2Int(0 ,0);
            isCellEmpty = true;
            heroId = -1;
        }

        public CellValues(Vector2Int _position)
        {
            locationName = "Hills";
            damagePerTurn = 1;
            healthPerTurn = 1;
            energyPerTurn = 0;
            position = _position;
            isCellEmpty = true;
            heroId = -1;
        }

        public CellValues(string _locationName, int _damagePerTurn, int _healthPerTurn, int _energyPerTurn, Vector2Int _position)
        {
            locationName = _locationName;
            damagePerTurn = _damagePerTurn;
            healthPerTurn = _healthPerTurn;
            energyPerTurn = _energyPerTurn;
            position = _position;
            isCellEmpty = true;
            heroId = -1;
        }

        public CellValues(string _locationName, int _damagePerTurn, int _healthPerTurn, int _energyPerTurn, Vector2Int _position, int _heroId)
        {
            locationName = _locationName;
            damagePerTurn = _damagePerTurn;
            healthPerTurn = _healthPerTurn;
            energyPerTurn = _energyPerTurn;
            position = _position;
            isCellEmpty = true;
            heroId = _heroId;
        }
    }
}
