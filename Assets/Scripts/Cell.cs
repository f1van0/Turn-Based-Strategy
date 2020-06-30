using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State:int
{
    empty = 0,
    nearby,
    wall,
    hero,
    enemy
}

public class Cell : MonoBehaviour
{
    private GameObject cell;
    private Vector2 pos = new Vector2(0, 0);
    private State state = State.empty;
    private HeroStats _heroStats;
    private int[] index = new int[2];

    public void CreateCell(Vector2 pos, int[] index, State state)
    {
        this.pos = pos;
        this.index = index;
        this.state = state;
        cell = this.gameObject;
        cell.transform.position = pos;
    }

    public HeroStats GetCellHeroStats()
    {
        return _heroStats;
    }

    public void SetCellHeroStats(HeroStats heroStats)
    {
        _heroStats = heroStats;
    }

    public void SetCellState(State state)
    {
        this.state = state;
    }

    public State GetCellState()
    {
        return state;
    }

    public bool IsCellEmpty()
    {
        return state == State.empty;
    }

    public Vector2 GetCellPosition()
    {
        return pos;
    }

    public int[] GetCellIndex()
    {
        return index;
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
