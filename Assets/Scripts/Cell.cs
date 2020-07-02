using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State:int
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
    private Color emptyCellColor = Color.white;
    private Color nearbyCellColor = Color.yellow;
    private Color wallCellColor = Color.black;
    private Color heroCellColor = Color.cyan;
    private Color enemyCellColor = Color.red;
    private Color friendCellColor = Color.green;
    private Color attackCellColor = Color.magenta;

    private GameObject cell;
    private Vector2 pos = new Vector2(0, 0);
    private State state = State.empty;
    private HeroStats _heroStats;
    private int[] index = new int[2];

    public void Initialize(Vector2 pos, int[] index, State state)
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

    public void SetState(State state)
    {
        this.state = state;
    }

    public State GetState()
    {
        return state;
    }

    public bool IsEmpty()
    {
        return state == State.empty;
    }

    public bool IsEnemy()
    {
        return state == State.enemy;
    }

    public Vector2 GetPosition()
    {
        return pos;
    }

    public int[] GetIndex()
    {
        return index;
    }

    public void ShowCell()
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

    public void ShowCell(State _state)
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
        cell = this.GetComponent<GameObject>();
        //cell.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
