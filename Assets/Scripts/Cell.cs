using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState:int
{
    empty = 0,
    nearby,
    wall,
    hero,
    controlledHero,
    enemy,
    friend,
    attack
}

public class Cell : MonoBehaviour
{
    public Color emptyColor = Color.white;
    public Color nearbyColor = Color.yellow;
    public Color wallColor = Color.black;
    public Color heroColor = Color.cyan;
    public Color enemyColor = Color.red;
    public Color friendColor = Color.green;
    public Color attackColor = Color.magenta;

    //Характеристики клетки
    public CellValues cellValues = new CellValues();

    public GameObject objectCell;
    //private Vector2 pos = new Vector2(0, 0);
    private CellState state = CellState.empty;
    //private int[] index = new int[2];

    public Cell()
    {
        cellValues = new CellValues();
        state = CellState.empty;
        //default heroVelue = null and location is "Hills" that empty
        //ShowCell
    }

    public Cell(CellValues _cellValues)
    {
        cellValues.locationName = _cellValues.locationName;
        cellValues.damagePerTurn = _cellValues.damagePerTurn;
        cellValues.healthPerTurn = _cellValues.healthPerTurn;
        cellValues.energyPerTurn = _cellValues.energyPerTurn;
        //state = _state;
        cellValues.position = _cellValues.position;
        //ShowCell
    }

    public void SetBasicCellValues(CellValues _cellValues)
    {
        cellValues.locationName = _cellValues.locationName;
        cellValues.damagePerTurn = _cellValues.damagePerTurn;
        cellValues.healthPerTurn = _cellValues.healthPerTurn;
        cellValues.energyPerTurn = _cellValues.energyPerTurn;
        //state = _state;
        cellValues.position = _cellValues.position;
        //ShowCell
    }

    public Cell(string _locationName, int _damagePerTurn, int _healthPerTurn, int _energyPerTurn, int _heroId, Vector2Int _position)
    {
        cellValues.locationName = _locationName;
        cellValues.damagePerTurn = _damagePerTurn;
        cellValues.healthPerTurn = _healthPerTurn;
        cellValues.energyPerTurn = _energyPerTurn;
        cellValues.heroId = _heroId;
        //state = _state;
        cellValues.position = _position;
    }

    public void Show()
    {
        SpriteRenderer cellSprite = objectCell.GetComponent<SpriteRenderer>();
        switch ((int)state)
        {
            case 0: { cellSprite.color = emptyColor; break; }
            case 1: { cellSprite.color = nearbyColor; break; }
            case 2: { cellSprite.color = wallColor; break; }
            case 3: { cellSprite.color = heroColor; break; }
            case 4: { cellSprite.color = enemyColor; break; }
            case 5: { cellSprite.color = friendColor; break; }
            default: { cellSprite.color = attackColor; break; }
        }
    }

    public void Show(CellState _state)
    {
        SpriteRenderer cellSprite = objectCell.GetComponent<SpriteRenderer>();
        state = _state;
        switch ((int)state)
        {
            case (int)CellState.empty: { cellSprite.color = emptyColor; break; }
            case (int)CellState.nearby: { cellSprite.color = nearbyColor; break; }
            case (int)CellState.wall: { cellSprite.color = wallColor; break; }
            case (int)CellState.hero: { cellSprite.color = heroColor; break; }
            case (int)CellState.enemy: { cellSprite.color = enemyColor; break; }
            case (int)CellState.friend: { cellSprite.color = friendColor; break; }
            default: { cellSprite.color = attackColor; break; }
        }
    }
    
    public void DefineBy_Team_Username_Available(int _localPlayerTeam, string _localPlayerUsername, bool _isAvailable, HeroValues _heroValues)
    {
        if (_heroValues.owner == _localPlayerUsername)
        {
            Show(CellState.hero);
        }
        else if (_isAvailable)
        {
            if (_heroValues.team != _localPlayerTeam && _heroValues.team != -1)
            {
                Show(CellState.attack);
            }
            else if (_heroValues.team == _localPlayerTeam)
            {
                Show(CellState.friend);
            }
        }
        else if (!_isAvailable)
        {
            if (_heroValues.team != _localPlayerTeam && _heroValues.team != -1)
            {
                Show(CellState.enemy);
            }
            else if (_heroValues.team == _localPlayerTeam)
            {
                Show(CellState.friend);
            }
        }
    }

    /*
    public void Initialize(Vector2 pos, int[] index, CellState state)
    {
        this.pos = pos;
        this.index = index;
        this.state = state;
        cell = this.gameObject;
        cell.transform.position = pos;
    }

    public HeroStats GetHeroStats()
    {
        return _heroStats;
    }

    public void SetHeroStats(HeroStats heroStats)
    {
        _heroStats = heroStats;
    }

    public void SetState(CellState state)
    {
        this.state = state;
    }

    public CellState GetState()
    {
        return state;
    }

    public bool IsEmpty()
    {
        return state == CellState.empty;
    }

    public bool IsEnemy()
    {
        return state == CellState.enemy;
    }

    public Vector2 GetPosition()
    {
        return pos;
    }

    public int[] GetIndex()
    {
        return index;
    }


    // Start is called before the first frame update
    void Start()
    {
        cell = this.gameObject;
        //cell.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
