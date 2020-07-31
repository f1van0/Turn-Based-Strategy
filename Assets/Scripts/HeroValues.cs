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
        /*
        internal int health { get; set; }
        internal int mana { get; set; }
        internal int damage { get; set; }
        internal int energy { get; set; }

        internal int defaultHealth = 100;
        internal int defaultMana = 120;
        internal int defaultDamage = 30;
        internal int defaultEnergy = 2;

        internal int targetID { get; set; }
        */
        internal int ID { get; set; }

        internal Vector2 position;
        internal string owner;
        internal int team;


        //Клетка на поле в которой находится герой
        //private Cell _cell;

        public HeroValues(HeroValues _heroValues)
        {
            if (_heroValues == null)
            {
                ID = -1;
                position = new Vector2(-1f, -1f);
                owner = "None";
                team = -1;
            }
            else
            {
                ID = _heroValues.ID;
                position = _heroValues.position;
                owner = _heroValues.owner;
                team = _heroValues.team;
            }
        }

        public HeroValues(int _id, Vector2 _position, string _owner, int _team)
        {
            /*
            health = 100;
            mana = 120;
            damage = 30;
            energy = 2;
            defaultEnergy = 2;
            */
            ID = _id;
            position = _position;
            owner = _owner;
            team = _team;
            //targetID = -1;
            //this._cell = null;
        }

        public HeroValues()
        {
            /*
            health = 100;
            mana = 120;
            damage = 30;
            energy = 2;
            defaultEnergy = 2;
            */
            team = 0;
            ID = 0;
            position = new Vector2(0, 0);
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
    }
}
