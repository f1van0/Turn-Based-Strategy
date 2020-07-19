using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState:int
{
    empty = 0,
    nearby,
    wall,
    hero,
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
    public string location = "Hill";
    public int damagePerTurn = 1;
    public int healthPerTurn = 1;
    public int energyPerTurn = 0;

    private GameObject cell;
    private Vector2 pos = new Vector2(0, 0);
    private CellState state = CellState.empty;
    private HeroStats _heroStats;
    private int[] index = new int[2];

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

    public void Show()
    {
        SpriteRenderer cellSprite = cell.GetComponent<SpriteRenderer>();
        switch ((int) state)
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
        SpriteRenderer cellSprite = cell.GetComponent<SpriteRenderer>();
        state = _state;
        switch ((int) state)
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
}
