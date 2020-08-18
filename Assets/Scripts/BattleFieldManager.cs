using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class BattleFieldManager : MonoBehaviour//, IDisposable
{
    public static BattleFieldManager instance;

    public Camera mainCamera;

    public GameObject cellPrefab;
    public GameObject heroPrefab;

    public Cell[,] battlefield = null;
    public Dictionary<int, Hero> heroes = new Dictionary<int, Hero>();

    public Vector2Int[] availableCells = null;

    private const float distanceBetweenCells = 1.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnBattlefield(CellValues[,] _battlefield)
    {
        battlefield = new Cell[_battlefield.GetLength(0), _battlefield.GetLength(1)];
        for (int j = 0; j < _battlefield.GetLength(1); j++)
        {
            for (int i = 0; i < _battlefield.GetLength(0); i++)
            {
                GameObject _cellGameObject = Instantiate(cellPrefab, new Vector3((i - _battlefield.GetLength(0) / 2) * 1.5f, (j - _battlefield.GetLength(1) / 2) * 1.5f, 0), new Quaternion(0, 0, 0, 0));
                battlefield[i, j] = _cellGameObject.GetComponent<Cell>();
                battlefield[i, j].SetBasicCellValues(_battlefield[i, j]);
                battlefield[i, j].objectCell = _cellGameObject;
            }
        }

        availableCells = null;
    }

    public void ResetBattlefield()
    {
        if (battlefield != null)
        {
            for (int j = 0; j < battlefield.GetLength(1); j++)
            {
                for (int i = 0; i < battlefield.GetLength(0); i++)
                {
                    Destroy(battlefield[i, j].gameObject, 0);
                }
            }

            battlefield = null;
        }

        availableCells = null;

        foreach(Hero _hero in heroes.Values)
        {
            Destroy(_hero.gameObject, 0);
        }

        heroes.Clear();
    }

    public void SpawnHero(HeroValues _heroValues)
    {
        int _heroId = _heroValues.ID;
        Vector2Int _heroPosition = _heroValues.position;

        //hero initialization
        GameObject _heroGameObject = Instantiate(heroPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        heroes.Add(_heroId, _heroGameObject.GetComponent<Hero>());
        heroes[_heroId].Initialize(_heroValues);

        //hero's cell
        ref Cell _cell = ref battlefield[_heroPosition.x, _heroPosition.y];
        _cell.cellValues.heroId = _heroId;

        //highlight hero's cell
        _cell.Show(GameManager.players[GameManager.clientId].team, GameManager.players[GameManager.clientId].username, false, _heroValues);

        Vector3 cellPosition = _cell.objectCell.transform.position;

        //move hero to the cell
        heroes[_heroId].transform.position = cellPosition;
    }

    public void SetCell(CellValues _cell)
    {
        battlefield[_cell.position.x, _cell.position.y].cellValues = _cell;
    }

    public void MoveHero(HeroValues _heroValues, CellValues from_cellValues, CellValues to_cellValues)
    {
        Vector2Int _previousHeroPosition = from_cellValues.position;
        Vector2Int _nextHeroPosition = to_cellValues.position;

        //Меняем информацию у клеткок и у героя
        battlefield[_previousHeroPosition.x, _previousHeroPosition.y].cellValues = from_cellValues;
        battlefield[_nextHeroPosition.x, _nextHeroPosition.y].cellValues = to_cellValues;
        heroes[_heroValues.ID].heroValues = _heroValues;

        //Move hero object to a new cell (can add animation or something like it)
        heroes[_heroValues.ID].gameObject.transform.position = battlefield[_nextHeroPosition.x, _nextHeroPosition.y].gameObject.transform.position;

        //Update cells highlighting
        battlefield[_previousHeroPosition.x, _previousHeroPosition.y].Show(CellState.empty);
        battlefield[_nextHeroPosition.x, _nextHeroPosition.y].Show(GameManager.players[GameManager.clientId].team, GameManager.players[GameManager.clientId].username, false, _heroValues);
        ClearAvailableCells();
    }

    public void AttackHero(int _attackingHeroId, HeroValues _attackedHeroValues)
    {
        int _attackedHeroId = _attackedHeroValues.ID;
        heroes[_attackedHeroId].heroValues = _attackedHeroValues;

        //play animation or something
        heroes[_attackingHeroId].GetComponent<SpriteRenderer>().color = Color.yellow;
        heroes[_attackedHeroId].GetComponent<SpriteRenderer>().color = Color.red;
        heroes[_attackedHeroId].GetComponentInChildren<Text>().text = heroes[_attackedHeroId].heroValues.health.ToString();

        ClearAvailableCells();
    }

    public void SelectHero(int _heroId)
    {
        HeroValues _heroValues = heroes[_heroId].heroValues;
        if (_heroValues.ID != -1)
        {
            if (_heroValues.owner == GameManager.players[GameManager.clientId].username && _heroValues.health >= 0)
            {
                ClientSend.SendAvailableCells(_heroValues.position);
            }
            else
            {
                ClearAvailableCells();
            }
        }
    }

    public void ShowAvailableCells(Vector2Int[] _availableCells)
    {
        for (int i = 0; i < _availableCells.Length; i++)
        {
            if (battlefield[_availableCells[i].x, _availableCells[i].y].cellValues.heroId != -1)
            {
                HeroValues _heroValues = heroes[battlefield[_availableCells[i].x, _availableCells[i].y].cellValues.heroId].heroValues;
                battlefield[_availableCells[i].x, _availableCells[i].y].Show(GameManager.players[GameManager.clientId].team, GameManager.players[GameManager.clientId].username, true, _heroValues);
            }
            else
            {
                battlefield[_availableCells[i].x, _availableCells[i].y].Show(CellState.nearby);
            }
        }
        battlefield[_availableCells[0].x, _availableCells[0].y].Show(CellState.hero);

        availableCells = _availableCells;
    }

    public void HideAvailableCells()
    {
        if (availableCells != null)
        {
            for (int i = 0; i < availableCells.Length; i++)
            {
                if (battlefield[availableCells[i].x, availableCells[i].y].cellValues.heroId != -1)
                {
                    HeroValues _heroValues = heroes[battlefield[availableCells[i].x, availableCells[i].y].cellValues.heroId].heroValues;
                    battlefield[availableCells[i].x, availableCells[i].y].Show(GameManager.players[GameManager.clientId].team, GameManager.players[GameManager.clientId].username, false, _heroValues);
                }
                else
                {
                    battlefield[availableCells[i].x, availableCells[i].y].Show(CellState.empty);
                }
            }
        }
    }

    public void ClearAvailableCells()
    {
        HideAvailableCells();

        if (availableCells != null)
        {
            availableCells = null;
        }
    }

    public bool isAvailableCellSelected(Vector2 _position)
    {
        bool isMatchFound = false;
        if (availableCells != null)
        {
            for (int i = 0; i < availableCells.Length; i++)
            {
                if (availableCells[i] == _position)
                {
                    isMatchFound = true;
                    break;
                }
            }
        }

        return isMatchFound;
    }

    public void SetHeroValues(HeroValues _heroValues)
    {
        heroes[_heroValues.ID].heroValues = _heroValues;
    }

    public HeroValues GetHeroValuesById(int _heroId)
    {
        return heroes[_heroId].heroValues;
    }
}
