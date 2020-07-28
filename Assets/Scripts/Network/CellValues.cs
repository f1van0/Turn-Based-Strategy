using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Network
{
    public class CellValues
    {
        public string locationName = "Hills";
        public int damagePerTurn = 1;
        public int healthPerTurn = 1;
        public int energyPerTurn = 0;

        public CellState state = CellState.empty;
        private HeroStats _heroStats;

        public CellValues()
        {
            locationName = "Hills";
            damagePerTurn = 1;
            healthPerTurn = 1;
            energyPerTurn = 0;
            state = CellState.empty;
            _heroStats = null;
        }

        public CellValues(string _locationName, int _damagePerTurn, int _healthPerTurn, int _energyPerTurn, int _state)
        {
            locationName = _locationName;
            damagePerTurn = _damagePerTurn;
            healthPerTurn = _healthPerTurn;
            energyPerTurn = _energyPerTurn;
            state = (CellState)_state;
            _heroStats = null;
        }
    }
}
