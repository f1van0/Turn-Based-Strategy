using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    internal int targetID { get; set; }
    internal int ID { get; set; }

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
        team = 0;
        ID = 0;
        targetID = -1;
        this._cell = null;
    }

    public void Initialize(Cell cell, int teamNumber, int id)
    {
        hp = 100;
        mana = 120;
        damage = 30;
        stepsCount = 2;
        defaultStepsCount = 2;
        this._cell = cell; 
        team = teamNumber;
        ID = id;
        targetID = -1;
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
    private HeroStats heroStats;
    private GameObject hero;
    private Canvas hero_canvas;
    private Text hp_text;

    public void TakeDamage(int damage)
    {
        heroStats.hp -= damage;
        hp_text.text = heroStats.hp+"HP";
    }

    public void InitializeHero(Cell cell, int teamNumber, int id)
    {
        heroStats = new HeroStats();
        hero = this.gameObject;
        hero_canvas = hero.GetComponentInChildren<Canvas>();
        hp_text = hero_canvas.GetComponentInChildren<Text>();
        hp_text.text = heroStats.hp + "HP";
        heroStats.Initialize(cell, teamNumber, id);
        cell.SetState(CellState.hero);
        cell.SetHeroStats(heroStats);
        heroStats.SetCell(cell);
        hero.transform.position = new Vector3(cell.GetPosition().x, cell.GetPosition().y, -1);
    }

    public HeroStats GetHeroStats()
    {
        return heroStats;
    }
    
    public void MoveToCell(Cell _cell)
    {
        heroStats.GetCell().Show(CellState.empty);
        heroStats.GetCell().SetHeroStats(null);
        heroStats.SetStepsCount(heroStats.GetHeroStepsCount() - Mathf.Abs(Mathf.RoundToInt(Mathf.Abs(heroStats.GetCell().GetIndex()[0]) - Mathf.Abs(_cell.GetIndex()[0]))) - Mathf.Abs(Mathf.RoundToInt(Mathf.Abs(heroStats.GetCell().GetIndex()[1]) - Mathf.Abs(_cell.GetIndex()[1]))));
        heroStats.SetCell(_cell);
        hero.transform.position = new Vector3(_cell.GetPosition().x, _cell.GetPosition().y, -1);
        heroStats.targetID = -1;
        heroStats.GetCell().Show(CellState.hero);
        _cell.SetHeroStats(heroStats);
    }

    public void SetTargetID(int targetID)
    {
        heroStats.targetID = targetID;
    }

    public int GetTargetID()
    {
        return heroStats.targetID;
    }

    public int GetID()
    {
        return heroStats.ID;
    }

    public bool isAlive()
    {
        return heroStats.hp > 0;
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
