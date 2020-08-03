using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class BattleFieldManager : MonoBehaviour//, IDisposable
{
    public static BattleFieldManager instance;

    public GameObject cellPrefab;
    public GameObject heroPrefab;
    public Camera mainCamera;

    private GameObject hero;
    private HeroBehaviour[] heroBehaviours = new HeroBehaviour[cols];
    private Cell[,] cell = new Cell[cols, rows];
    public Cell[,] battlefield;
    public Dictionary<int, GameObject> heroes = new Dictionary<int, GameObject>();

    public Vector2[] availableCells;

    private const int cols = 8;
    private const int rows = 6;
    private const int heroesCount = 4;
    private const int teamCount = 2;
    private const float distanceBetweenCells = 1.5f;
    private Vector2 cameraCenter;

    InputController inputController;
    //private int turn = 0;

    //private GameObject[,] cells = new GameObject[n, m];

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
    }

    public void SetCellInfo(CellValues _cell, bool _isCellAvailable)
    {
        /*
        if (_isCellAvailable)
            battlefield[(int)_cell.position.x, (int)_cell.position.y] = new Cell(_cell.locationName, _cell.damagePerTurn, _cell.healthPerTurn, _cell.energyPerTurn, CellState.nearby, new Vector2(_cell.position.x, _cell.position.y));
        else
            battlefield[(int)_cell.position.x, (int)_cell.position.y] = new Cell(_cell.locationName, _cell.damagePerTurn, _cell.healthPerTurn, _cell.energyPerTurn, CellState.wall, new Vector2(_cell.position.x, _cell.position.y));

        battlefield[(int)_cell.position.x, (int)_cell.position.y].Show();
        */
    }

    /*
    public void SpawnHero(int _id, Vector2 _position)
    {
        heroes.Add(_id, Instantiate(heroPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)));
        heroes[_id].transform.SetParent(battlefield[(int)_position.x, (int)_position.y].gameObject.transform, false);
    }
    */

    public void SpawnHero(HeroValues _heroValues)
    {
        int _heroId = _heroValues.ID;
        Vector2 _heroPosition = _heroValues.position;

        battlefield[(int)_heroPosition.x, (int)_heroPosition.y].SetHeroValues(_heroValues);

        heroes.Add(_heroId, Instantiate(heroPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)));
        Transform cellParent = battlefield[(int)_heroPosition.x, (int)_heroPosition.y].objectCell.transform;

        heroes[_heroId].transform.SetParent(cellParent, false);

        //Display cell in Unity scene (by changing cell color).
        battlefield[(int)_heroPosition.x, (int)_heroPosition.y].DefineBy_Team_Username_Available(GameManager.players[GameManager.clientId].team, GameManager.players[GameManager.clientId].username, false);
    }

    /*
    public void MoveHero(int _id, Vector2 _position)
    {
        heroes[_id].gameObject.transform.position = _position;
        heroes[_id].transform.SetParent(battlefield[(int)_position.x, (int)_position.y].gameObject.transform, false);
    }
    */

    public void MoveHero(CellValues from_cellValues, CellValues to_cellValues)
    {
        int _heroId = to_cellValues.GetHeroValues().ID;
        Vector2 _previousHeroPosition = from_cellValues.position;
        Vector2 _nextHeroPosition = to_cellValues.position;
        //Удаляем героя из прошлой клетки
        battlefield[(int)_previousHeroPosition.x, (int)_previousHeroPosition.y].SetHeroValues(new HeroValues(null));
        battlefield[(int)_previousHeroPosition.x, (int)_previousHeroPosition.y].Show(CellState.empty);
        //Добавляем героя, меняя информацию в клетке, в новую клетку
        battlefield[(int)_nextHeroPosition.x, (int)_nextHeroPosition.y].SetHeroValues(to_cellValues.GetHeroValues());
        heroes[_heroId].transform.SetParent(battlefield[(int)_nextHeroPosition.x, (int)_nextHeroPosition.y].gameObject.transform, false);

        battlefield[(int)_nextHeroPosition.x, (int)_nextHeroPosition.y].DefineBy_Team_Username_Available(GameManager.players[GameManager.clientId].team, GameManager.players[GameManager.clientId].username, false);
    }

    public void SelectHero(Vector2 _heroPosition)
    {
        Cell _cellWithHero = battlefield[(int)_heroPosition.x, (int)_heroPosition.y].GetComponent<Cell>();
        HeroValues _heroValues = _cellWithHero.cellValues.GetHeroValues();
        if (_heroValues.ID != -1)
        {
            if (_heroValues.owner == GameManager.players[GameManager.clientId].username)
            {
                ClientSend.SendAvailableCells(_heroPosition);
            }
            else
            {
                HideAvailableCells();
            }
        }
    }

    public void ShowAvailableCells(Vector2[] _availableCells)
    {
        for (int i = 0; i < _availableCells.Length; i++)
        {
            if (battlefield[(int)_availableCells[i].x, (int)_availableCells[i].y].cellValues.GetHeroValues().ID != -1)
            {
                battlefield[(int)_availableCells[i].x, (int)_availableCells[i].y].DefineBy_Team_Username_Available(GameManager.players[GameManager.clientId].team, GameManager.players[GameManager.clientId].username, true);
            }
            else
            {
                battlefield[(int)_availableCells[i].x, (int)_availableCells[i].y].Show(CellState.nearby);
            }
        }
        battlefield[(int)_availableCells[0].x, (int)_availableCells[0].y].Show(CellState.hero);

        availableCells = _availableCells;
    }

    public void HideAvailableCells()
    {
        if (availableCells != null)
        {
            for (int i = 0; i < availableCells.Length; i++)
            {
                battlefield[(int)availableCells[i].x, (int)availableCells[i].y].Show(CellState.empty);
            }
            battlefield[(int)availableCells[0].x, (int)availableCells[0].y].DefineBy_Team_Username_Available(GameManager.players[GameManager.clientId].team, GameManager.players[GameManager.clientId].username, false);
        }

        availableCells = null;
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

    /*
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
        inputController = FindObjectOfType<InputController>();
        inputController.SelectHero += OnSelectedHero;
        inputController.SelectCell += OnSelectCell;
        inputController.DefocusHero += HideAccesibleCells;
        inputController.ChangeTurn += ChangeTurn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialization()
    {
        cameraCenter = new Vector2(mainCamera.pixelWidth / 180, mainCamera.pixelHeight / 180);
        Vector2 pos;

        //Создаем поле
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                pos = new Vector2(i * distanceBetweenCells, j * distanceBetweenCells) - cameraCenter;
                cell[i, j] = Instantiate(cellPrefab, pos, new Quaternion(0, 0, 0, 0)).GetComponent<Cell>();
                cell[i, j].Initialize(pos, new int[2] { i, j }, CellState.empty);
            }
        }

        //Создаем игрока, к примеру в клетке (0, 0) левый нижний угол
        for (int i = 0; i < heroesCount; i++)
        {
            heroBehaviours[i] = Instantiate(heroPrefab, new Vector3(0, 0, -1), new Quaternion(0, 0, 0, 0)).GetComponent<HeroBehaviour>();
            heroBehaviours[i].InitializeHero(cell[i, i], i % 2, i);
        }
        //hero = Instantiate(heroPrefab, new Vector3(0, 0, -1), new Quaternion(0, 0, 0, 0));
        //heroBehaviours = hero.GetComponent<HeroBehaviour>();

        //Подсвечиваем нужное
        ChangeTurn(0);
        //RebootField();
    }

    public void OnSelectedHero(HeroBehaviour _heroBehaviour)
    {
        //Баг из-за которого можно было ходить бесконечно, нажимая на своего героя, потом на другого, потом опять на своего, из-за чего не скрывались Accessible Cells, т.е. доступные клетки, из-за чего на них можно было нажать
        HideAccesibleCells();

        ShowHeroes(_heroBehaviour);
        HighlightCellWithSelectedHero(_heroBehaviour);
        ShowAccesibleCellsByWave(_heroBehaviour);
    }

    private void OnSelectCell(GameObject hit, HeroBehaviour selectedHero)
    {
        HeroBehaviour _heroBehaviour;
        Cell _cell;

        if (hit.TryGetComponent<HeroBehaviour>(out _heroBehaviour) && hit.GetComponent<HeroBehaviour>().GetHeroStats().GetCell().GetState() == CellState.attack)
        {
            selectedHero.SetTargetID(_heroBehaviour.GetHeroStats().ID);
            selectedHero.GetHeroStats().SetEnergyCount(0);
            //Повторяющийся код
            HideAccesibleCells();
            ShowHeroes(selectedHero);
            HighlightCellWithSelectedHero(selectedHero);
            ShowAccesibleCellsByWave(selectedHero);
        }
        else if (hit.TryGetComponent<Cell>(out _cell))
        {
            if (_cell.GetState() == CellState.nearby)
            {
                selectedHero.MoveToCell(_cell);
                //Повторяющийся код
                HideAccesibleCells();
                ShowHeroes(selectedHero);
                HighlightCellWithSelectedHero(selectedHero);
                ShowAccesibleCellsByWave(selectedHero);
            }
            else if (_cell.GetState() == CellState.attack)
            {
                selectedHero.SetTargetID(_cell.GetHeroStats().ID);
                selectedHero.GetHeroStats().SetEnergyCount(0);
                //Повторяющийся код
                HideAccesibleCells();
                ShowHeroes(selectedHero);
                HighlightCellWithSelectedHero(selectedHero);
                ShowAccesibleCellsByWave(selectedHero);
            }
            else
            {
                HideAccesibleCells();
            }
        }
    }

    public void RewriteField()
    {
        for (int i = 0; i < cols; i++)
            for (int j = 0; j < rows; j++)
                cell[i, j].Show();
    }

    public void HideAccesibleCells()
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (cell[i, j].GetState() == CellState.nearby)
                    cell[i, j].Show(CellState.empty);
                //Возможно здесь стоило бы друзей и врагов превратить в просто нейтральных героев
                else if (cell[i, j].GetState() == CellState.attack)
                    cell[i, j].Show(CellState.enemy);
            }
        }
    }

    public void ShowAccesibleCellsByWave(HeroBehaviour _heroBehaviour)
    {
        HeroStats _heroStats = _heroBehaviour.GetHeroStats();
        int CountOfCurrentNodes = 1;
        int CountOfNewNodes = 0;
        int CountOfAllNodes = 1;
        int newCount = 0;
        int start = CountOfAllNodes - CountOfCurrentNodes;
        int io = _heroStats.GetCell().GetIndex()[0];
        int jo = _heroStats.GetCell().GetIndex()[1];
        int stepsCount = _heroStats.GetHeroEnergy();

        Vector2[] currentNodes = new Vector2[1000];
        currentNodes[0] = new Vector2(io, jo);

        for (int len = 0; len < stepsCount; len++)
        {
            CountOfNewNodes = 0;
            for (int t = start; t < CountOfAllNodes; t++)
            {
                newCount = 0;
                int i = Mathf.RoundToInt(currentNodes[t].x);
                int j = Mathf.RoundToInt(currentNodes[t].y);

                if (i < cols - 1)
                {
                    if (cell[i + 1, j].IsEmpty())
                    {
                        currentNodes[CountOfAllNodes + CountOfNewNodes] = SetCell(i + 1, j, CellState.nearby);
                        cell[i + 1, j].Show();
                        newCount++;
                        CountOfNewNodes++;
                    }
                    else if (cell[i + 1, j].IsEnemy())
                    {
                        cell[i + 1, j].Show(CellState.attack);
                    }
                }
                if (i > 0)
                {
                    if (cell[i - 1, j].IsEmpty())
                    {
                        currentNodes[CountOfAllNodes + CountOfNewNodes] = SetCell(i - 1, j, CellState.nearby);
                        cell[i - 1, j].Show();
                        newCount++;
                        CountOfNewNodes++;
                    }
                    else if (cell[i - 1, j].IsEnemy())
                    {
                        cell[i - 1, j].Show(CellState.attack);
                    }
                }
                if (j < rows - 1)
                {
                    if (cell[i, j + 1].IsEmpty())
                    {
                        currentNodes[CountOfAllNodes + CountOfNewNodes] = SetCell(i, j + 1, CellState.nearby);
                        cell[i, j + 1].Show();
                        newCount++;
                        CountOfNewNodes++;
                    }
                    else if (cell[i, j + 1].IsEnemy())
                    {
                        cell[i, j + 1].Show(CellState.attack);
                    }
                }
                if (j > 0)
                {
                    if (cell[i, j - 1].IsEmpty())
                    {
                        currentNodes[CountOfAllNodes + CountOfNewNodes] = SetCell(i, j - 1, CellState.nearby);
                        cell[i, j - 1].Show(); ;
                        newCount++;
                        CountOfNewNodes++;
                    }
                    else if (cell[i, j - 1].IsEnemy())
                    {
                        cell[i, j - 1].Show(CellState.attack);
                    }
                }
                //сделать шаги в стороны
                //очистить текущие
                //проверить были ли они в истории
                //добавить полученные в текущие
            }
            start = CountOfAllNodes;
            CountOfAllNodes += CountOfNewNodes;
            CountOfCurrentNodes = CountOfNewNodes;
        }
    }

    public void HighlightCellWithSelectedHero(HeroBehaviour _heroBehaviour)
    {
        Cell _cell = _heroBehaviour.GetHeroStats().GetCell();
        _cell.Show(CellState.hero);
    }

    public Vector2 SetCell(int i, int j, CellState state)
    {
        Vector2 node;
        cell[i, j].Show(state);
        node = new Vector2(i, j);
        return node;
    }

    public void ShowHeroes(HeroBehaviour heroBehaviour)
    {
        //Проходим по каждому из героев
        for (int i = 0; i < heroesCount; i++)
        {
            if (heroBehaviours[i].GetHeroStats().GetTeam() != heroBehaviour.GetHeroStats().GetTeam())
                heroBehaviours[i].GetHeroStats().GetCell().Show(CellState.enemy);
            else
                heroBehaviours[i].GetHeroStats().GetCell().Show(CellState.friend);
        }
    }

    public void ChangeTurn(int _turn)
    {
        int target_id;
        int damage;
        //Проходимся по каждому герою
        for (int i = 0; i < heroesCount; i++)
        {
            //Наносим урон героям, которые были выбраны в качестве цели (targetID) для атаки. Причем происходит проход только по тем героям, которые выбрали цель
            if (heroBehaviours[i].GetTargetID() != -1)
            {
                target_id = heroBehaviours[i].GetTargetID();
                damage = heroBehaviours[i].GetHeroStats().damage;
                //Обращаемся к герою, который был выбран и сбавляем ему хп на наносимый противником урон (damage)
                heroBehaviours[target_id].TakeDamage(damage);
                heroBehaviours[i].SetTargetID(-1);
            }

            //Возвращаем игрокам их число доступных шагов, если они находятся в команде, которая производит ход
            if (heroBehaviours[i].isAlive())
            {
                //Если герой жив, меняем его heroStats в соответствии с клеткой восстанавливаем ему шаги
                heroBehaviours[i].SetHeroStatsAfterTurn();
                heroBehaviours[i].GetHeroStats().RestoreEnergy();
            }
            else
            {
                //Если герой мертв, то он не может ходить
                heroBehaviours[i].GetHeroStats().SetEnergyCount(0);
            }
            //Код для переключения хода между командами
            if (heroBehaviours[i].GetHeroStats().GetTeam() == _turn % teamCount)
            {
                heroBehaviours[i].GetHeroStats().RestoreEnergy();
            }
            else
            {
                heroBehaviours[i].GetHeroStats().SetEnergyCount(0);
            }
        }
        HideAccesibleCells();
        RewriteField();
    }

    public void Dispose()
    {
        inputController.SelectHero -= OnSelectedHero;
        inputController.SelectCell -= OnSelectCell;
        inputController.DefocusHero -= HideAccesibleCells;
        inputController.ChangeTurn -= ChangeTurn;
    }

    /*
    public void ShowAccesibleCellsForHero(HeroStats _hero)
    {
        /*
        int cols = _hero.GetHeroCell().GetCellIndex()[0];
        int rows = _hero.GetHeroCell().GetCellIndex()[1];
        int leftBorder, rightBorder, topBorder, bottomBorder;

        //Беру в цикле каждый узел, потом от этим узлов создаю еще узлы тем же способом

        if (cols - 2 >= 0) leftBorder = cols - 2;
        else leftBorder = 0;
        if (cols + 2 <= n - 1) rightBorder = cols + 2;
        else rightBorder = n - 1;
        if (rows + 2 <= m - 1) topBorder = rows + 2;
        else topBorder = m - 1;
        if (cols - 2 >= 0) bottomBorder = cols - 2;
        else bottomBorder = 0;
        */
    /*
    FieldTraversal(1, _hero.GetHeroCell().GetCellIndex()[0], _hero.GetHeroCell().GetCellIndex()[1], _hero);
}

private void FieldTraversal(int k, int i, int j, HeroStats _hero)
{
    if (i >= 0 && j >= 0 && (_cell[i, j].GetCellState() == State.empty) && (Mathf.Abs(_hero.GetHeroCell().GetCellIndex()[0] - i) + Mathf.Abs(_hero.GetHeroCell().GetCellIndex()[1] - j) <= _hero.GetHeroSteps()))
    {
        _cell[i, j].SetCellState(State.nearby);
        _cell[i, j].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        switch (k)
        {
            //влево
            case 1: { FieldTraversal(2, i - 1, j, _hero); break; }
            //вправо
            case 2: { FieldTraversal(3, i + 1, j, _hero); break; }
            //вверх
            case 3: { FieldTraversal(4, i, j + 1, _hero); break; }
            //вниз
            default: { FieldTraversal(1, i, j - 1, _hero); break; }
        }
    }
}
*/
}
