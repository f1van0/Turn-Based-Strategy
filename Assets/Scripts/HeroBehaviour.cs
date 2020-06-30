using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStats
{
    internal int hp { get; set; }
    internal int mana { get; set; }
    internal int damage { get; set; }
    internal int stepsCount { get; set; }

    private int defaultHP = 100;
    private int defaultMana = 120;
    private int defaultDamage = 30;
    private int defaultStepsCount = 2;


    private string owner;
    private int team;


    //Клетка на поле в которой находится герой
    private Cell _cell;

    public HeroStats()
    {
        hp = 100;
        mana = 120;
        damage = 30;
        stepsCount = 2;
        defaultStepsCount = 2;
        team = 1;
        this._cell = null;
    }

    public void Initialize(Cell cell, int teamNumber)
    {
        hp = 100;
        mana = 120;
        damage = 30;
        stepsCount = 2;
        defaultStepsCount = 2;
        this._cell = cell;
        team = teamNumber;
    }


    public Cell GetCell()
    {
        return _cell;
    }

    //Изменить положение игрока, переместив его в клетку
    public void SetCell(Cell cell)
    {
        this._cell = cell;
    }

    //Узнать количество доступных шагов
    public int GetHeroStepsCount()
    {
        return stepsCount;
    }

    //Изменить количество доступных шагов
    public void SetStepsCount(int stepsCount)
    {
        this.stepsCount = stepsCount;
    }

    public void RestoreStepsCount()
    {
        stepsCount = defaultStepsCount;
    }

    public int GetTeam()
    {
        return team;
    }

    public void SetTeam(int teamNumber)
    {
        team = teamNumber;
    }
}

public class HeroBehaviour : MonoBehaviour
{
    private HeroStats _heroStats;
    private GameObject hero;

    public void TakeDamage(int damage)
    {
        _heroStats.hp -= damage;
    }

    public void InitializeHero(Cell cell, int teamNumber)
    {
        _heroStats = new HeroStats();
        hero = this.gameObject;
        _heroStats.Initialize(cell, teamNumber);
        cell.SetState(State.hero);
        _heroStats.SetCell(cell);
        hero.transform.position = new Vector3(cell.GetPosition().x, cell.GetPosition().y, -1);
    }

    public HeroStats GetHeroStats()
    {
        return _heroStats;
    }
    
    public void MoveToCell(Cell cell)
    {
        _heroStats.GetCell().SetState(State.empty);
        _heroStats.SetStepsCount(_heroStats.GetHeroStepsCount() - Mathf.Abs(Mathf.RoundToInt(Mathf.Abs(_heroStats.GetCell().GetIndex()[0]) - Mathf.Abs(cell.GetIndex()[0]))) - Mathf.Abs(Mathf.RoundToInt(Mathf.Abs(_heroStats.GetCell().GetIndex()[1]) - Mathf.Abs(cell.GetIndex()[1]))));
        _heroStats.SetCell(cell);
        hero.transform.position = new Vector3(cell.GetPosition().x, cell.GetPosition().y, -1);
        _heroStats.GetCell().SetState(State.hero);
    }
/*
    public void CreateHero(Cell cell)
    {
        _hero = new HeroStats();
        _hero.SetHeroPosition(cell.GetCellPosition());
        hero.transform.position = _hero.GetHeroPosition();
    }




    public void CreateHero(Cell cell)
    {
        pos = cell.GetCellPosition();
        hero.transform.position = pos;
        _hero = HeroStats();
    }

    public void SetHeroPosition(int i, int j, Vector2 pos)
    {
        _hero.SetHeroCell(i, j);
        //здесь вместо присвоений должна быть плавная анимация
        this.pos = pos;
        hero.transform.position = pos;
    }
*/
    // Start is called before the first frame update
    void Start()
    {
        //hero = GetComponent<GameObject>();
        //hero.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
