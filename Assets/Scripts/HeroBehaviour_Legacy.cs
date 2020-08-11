using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroBehaviour : MonoBehaviour
{
    //private HeroStats heroStats;
    private GameObject hero;
    private Canvas hero_canvas;
    private Text hp_text;
    /*
    public void TakeDamage(int damage)
    {
        heroStats.health -= damage;
        hp_text.text = heroStats.health+"HP";
    }
    /*
    public void InitializeHero(Cell cell, int teamNumber, int id)
    {
        heroStats = new HeroStats();
        hero = this.gameObject;
        hero_canvas = hero.GetComponentInChildren<Canvas>();
        hp_text = hero_canvas.GetComponentInChildren<Text>();
        hp_text.text = heroStats.health + "HP";
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

    public void SetHeroStatsAfterTurn()
    {
        heroStats.defaultDamage += heroStats.GetCell().damagePerTurn;
        heroStats.defaultHealth += heroStats.GetCell().healthPerTurn;
        heroStats.defaultEnergy += heroStats.GetCell().energyPerTurn;

        heroStats.damage += heroStats.GetCell().damagePerTurn;
        heroStats.health += heroStats.GetCell().healthPerTurn;
        heroStats.energy += heroStats.GetCell().energyPerTurn;
    }
    
    public void MoveToCell(Cell _cell)
    {
        heroStats.GetCell().Show(CellState.empty);
        heroStats.GetCell().SetHeroStats(null);
        heroStats.SetEnergyCount(heroStats.GetHeroEnergy() - Mathf.Abs(Mathf.RoundToInt(Mathf.Abs(heroStats.GetCell().GetIndex()[0]) - Mathf.Abs(_cell.GetIndex()[0]))) - Mathf.Abs(Mathf.RoundToInt(Mathf.Abs(heroStats.GetCell().GetIndex()[1]) - Mathf.Abs(_cell.GetIndex()[1]))));
        heroStats.SetCell(_cell);
        hero.transform.position = new Vector3(_cell.GetPosition().x, _cell.GetPosition().y, -1);
        heroStats.targetID = -1;
        heroStats.GetCell().Show(CellState.hero);
        _cell.SetHeroStats(heroStats);
    }
    */
    /*
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
        return heroStats.health > 0;
    }
    */
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
