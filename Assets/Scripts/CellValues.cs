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
        public Vector2 position; // Позиция не в прямом смысле, здесь скорее индекс клетки, чтобы узнать положение этой клетки относительно других
        public bool isCellEmpty = true;
        private HeroValues heroValues;

        public CellValues()
        {
            locationName = "Hills";
            damagePerTurn = 1;
            healthPerTurn = 1;
            energyPerTurn = 0;
            position = new Vector2(0 ,0);
            isCellEmpty = true;
            heroValues = new HeroValues(null);
        }

        public CellValues(Vector2 _position)
        {
            locationName = "Hills";
            damagePerTurn = 1;
            healthPerTurn = 1;
            energyPerTurn = 0;
            position = _position;
            isCellEmpty = true;
            heroValues = new HeroValues(null);
        }

        public CellValues(string _locationName, int _damagePerTurn, int _healthPerTurn, int _energyPerTurn, Vector2 _position)
        {
            locationName = _locationName;
            damagePerTurn = _damagePerTurn;
            healthPerTurn = _healthPerTurn;
            energyPerTurn = _energyPerTurn;
            position = _position;
            isCellEmpty = true;
            heroValues = new HeroValues(null);
        }

        public CellValues(string _locationName, int _damagePerTurn, int _healthPerTurn, int _energyPerTurn, Vector2 _position, HeroValues _heroValues)
        {
            locationName = _locationName;
            damagePerTurn = _damagePerTurn;
            healthPerTurn = _healthPerTurn;
            energyPerTurn = _energyPerTurn;
            position = _position;
            isCellEmpty = true;
            heroValues = _heroValues;
        }

        public void SpawnHero(int _id, Vector2 _position, string _username, int _team)
        {
            heroValues = new HeroValues(_id, _position, _username, _team);
        }

        public void SpawnHero(HeroValues _heroValues)
        {
            //heroValues = new HeroValues(_heroValues);
        }

        public void AddHeroOnCell(HeroValues _heroStats)
        {
            heroValues = _heroStats;
        }

        public void DeleteHeroOnCell()
        {
            heroValues = null;
        }

        public HeroValues GetHeroValues()
        {
            return heroValues;
        }

        public void SetHeroValues(HeroValues _heroStats)
        {
            heroValues = _heroStats;
        }
    }
}
