using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Turn
{
    protected Team Team;
    protected TurnCycle TurnCycle;

    public Turn(Team team, TurnCycle turnCycle)
    {
        Team = team;
        TurnCycle = turnCycle;
    }

    public abstract IEnumerator Start(); // происходит на старте состояния
    public abstract IEnumerator SelectHero(HeroBehaviour hero); // выбрать героя
    public abstract IEnumerator UseHero(string action); // походить героем
    public abstract IEnumerator FinishTurn(); // заканчиваем ход
}

public class TeamTurn: Turn
{
    public TeamTurn(Team team, TurnCycle turnCycle) : base(team, turnCycle)
    {
    }

    public override IEnumerator FinishTurn()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator SelectHero(HeroBehaviour hero)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator Start()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator UseHero(string action)
    {
        throw new NotImplementedException();
    }
}


public class NextTurn : Turn
{
    public NextTurn(Team team, TurnCycle turnCycle) : base(team, turnCycle)
    {
    }

    public override IEnumerator FinishTurn()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator SelectHero(HeroBehaviour hero)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator Start()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator UseHero(string action)
    {
        throw new NotImplementedException();
    }
}


public class Yielding : Turn
{
    public Yielding(Team team, TurnCycle turnCycle) : base(team, turnCycle)
    {
    }

    public override IEnumerator FinishTurn()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator SelectHero(HeroBehaviour hero)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator Start()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator UseHero(string action)
    {
        throw new NotImplementedException();
    }
}
