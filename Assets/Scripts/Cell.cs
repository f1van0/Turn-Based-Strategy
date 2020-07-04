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
    public Color emptyCellColor = Color.white;
    public Color nearbyCellColor = Color.yellow;
    public Color wallCellColor = Color.black;
    public Color heroCellColor = Color.cyan;
    public Color enemyCellColor = Color.red;
    public Color friendCellColor = Color.green;
    public Color attackCellColor = Color.magenta;

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
            case 0: { cellSprite.color = emptyCellColor; break; }
            case 1: { cellSprite.color = nearbyCellColor; break; }
            case 2: { cellSprite.color = wallCellColor; break; }
            case 3: { cellSprite.color = heroCellColor; break; }
            case 4: { cellSprite.color = enemyCellColor; break; }
            case 5: { cellSprite.color = friendCellColor; break; }
            default: { cellSprite.color = attackCellColor; break; }
        }
    }

    public void Show(CellState _state)
    {
        SpriteRenderer cellSprite = cell.GetComponent<SpriteRenderer>();
        state = _state;
        switch ((int) state)
        {
            case 0: { cellSprite.color = emptyCellColor; break; }
            case 1: { cellSprite.color = nearbyCellColor; break; }
            case 2: { cellSprite.color = wallCellColor; break; }
            case 3: { cellSprite.color = heroCellColor; break; }
            case 4: { cellSprite.color = enemyCellColor; break; }
            case 5: { cellSprite.color = friendCellColor; break; }
            default: { cellSprite.color = attackCellColor; break; }
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
