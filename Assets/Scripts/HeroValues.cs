using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class HeroValues
    {
        internal int health { get; set; }
        internal int defaultHealth { get; set; }

        internal int damage { get; set; }
        internal int defaultDamage { get; set; }

        internal int energy { get; set; }
        internal int defaultEnergy { get; set; }


        internal int targetID { get; set; }
        internal int ID { get; set; }

        internal Vector2Int position;
        internal string owner;
        internal int team;


        //Клетка на поле в которой находится герой
        //private Cell _cell;

        public HeroValues(HeroValues _heroValues)
        {
            if (_heroValues == null)
            {
                ID = -1;
                position = new Vector2Int(-1, -1);
                owner = "None";
                team = -1;

                defaultHealth = 0;
                health = defaultHealth;

                defaultDamage = 0;
                damage = defaultDamage;

                defaultEnergy = 0;
                energy = defaultEnergy;
            }
            else
            {
                ID = _heroValues.ID;
                position = _heroValues.position;
                owner = _heroValues.owner;
                team = _heroValues.team;

                defaultHealth = _heroValues.defaultHealth;
                health = _heroValues.health;

                defaultDamage = _heroValues.defaultDamage;
                damage = _heroValues.damage;

                defaultEnergy = _heroValues.defaultEnergy;
                energy = _heroValues.energy;
            }
        }

        public HeroValues(int _id, Vector2Int _position, string _owner, int _team, int _defaultHealth, int _health, int _defaultDamage, int _damage, int _defaultEnergy, int _energy)
        {
            ID = _id;
            position = _position;
            owner = _owner;
            team = _team;

            defaultHealth = _defaultHealth;
            health = _health;

            defaultDamage = _defaultDamage;
            damage = _damage;

            defaultEnergy = _defaultEnergy;
            energy = _energy;
        }

        public HeroValues(int _id, Vector2Int _position, string _owner, int _team)
        {
            ID = _id;
            position = _position;
            owner = _owner;
            team = _team;

            defaultHealth = 100;
            health = defaultHealth;

            defaultDamage = 20;
            damage = defaultDamage;

            defaultEnergy = 2;
            energy = defaultEnergy;
        }

        public HeroValues()
        {
            defaultHealth = 100;
            health = defaultHealth;

            defaultDamage = 20;
            damage = defaultDamage;

            defaultEnergy = 2;
            energy = defaultEnergy;

            team = 0;
            ID = 0;
            position = new Vector2Int(0, 0);
            //targetID = -1;
            owner = "AI";
            //this._cell = null;
        }

        public void Initialize(Cell cell, int teamNumber, int id)
        {
            /*
            health = 100;
            mana = 120;
            damage = 30;
            energy = 2;
            defaultEnergy = 2;
            */
            //this._cell = cell; 
            team = teamNumber;
            ID = id;
            //targetID = -1;
        }
        /*
        //Узнать количество доступных шагов
        public int GetHeroEnergy()
        {
            return energy;
        }

        //Изменить количество доступных шагов
        public void SetEnergyCount(int _energy)
        {
            this.energy = _energy;
        }

        public void RestoreEnergy()
        {
            energy = defaultEnergy;
        }
        */
        public int GetTeam()
        {
            return team;
        }

        public string GetOwner()
        {
            return owner;
        }

        public void SetOwner(string _owner)
        {
            owner = _owner;
        }

        public void SetTeam(int teamNumber)
        {
            team = teamNumber;
        }

        public void GetDamage(HeroValues _attakingHero)
        {
            health -= _attakingHero.damage;
        }
    }
}
