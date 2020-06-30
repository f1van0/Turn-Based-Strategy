using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject heroPrefab;

    public Color emptyCellColor = Color.white;
    public Color nearbyCellColor = Color.yellow;
    public Color wallCellColor = Color.black;
    public Color heroCellColor = Color.cyan;
    public Color enemyCellColor = Color.red;
    public Color friendCellColor = Color.green;
    public Color attackCellColor = Color.magenta;

    const int n = 4;
    const int m = 4;
    private int distanceBetweenCells = 2;

    //private int turn = 0;

    //private GameObject[,] cells = new GameObject[n, m];
    private GameObject hero;
    private HeroBehaviour[] heroBehaviours = new HeroBehaviour[n];
    private Cell[,] _cell = new Cell[n, m];

    public void Initialization()
    {
        //Создаем поле
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Vector2 pos = new Vector2(i * distanceBetweenCells, j * distanceBetweenCells);
                _cell[i, j] = Instantiate(cellPrefab, pos, new Quaternion(0, 0, 0, 0)).GetComponent<Cell>();
                _cell[i, j].Initialize(pos, new int[2] { i, j }, State.empty);
            }
        }

        //Создаем игрока, к примеру в клетке (0, 0) левый нижний угол
        for (int i = 0; i < n; i++)
        {
            heroBehaviours[i] = Instantiate(heroPrefab, new Vector3(0, 0, -1), new Quaternion(0, 0, 0, 0)).GetComponent<HeroBehaviour>();
            heroBehaviours[i].InitializeHero(_cell[i, i], i % 2);
        }
        //hero = Instantiate(heroPrefab, new Vector3(0, 0, -1), new Quaternion(0, 0, 0, 0));
        //heroBehaviours = hero.GetComponent<HeroBehaviour>();
        
        //Подсвечиваем нужное
        RebootField();
    }

    public void RebootCell(Cell cell)
    {
        SpriteRenderer cellSprite = cell.gameObject.GetComponent<SpriteRenderer>();
        Color cellColor = cellSprite.color;
        int cellState = (int) cell.GetState();
        switch (cellState)
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

    public void RebootField()
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                RebootCell(_cell[i, j]);
    }

    public void HideAccesibleCells()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (_cell[i, j].GetState() == State.nearby)
                    _cell[i, j].SetState(State.empty);
                RebootCell(_cell[i, j]);
            }
        }
    }

    public void ShowAccesibleCellsByWave(HeroStats _heroStats)
    {
        //Vector2[] historyOfNodes = new Vector2[(_hero.GetHeroSteps() + 1) ^ 2];
        //Mathf.Pow(Mathf.RoundToInt(_hero.GetHeroSteps()) + 1, 2)
        //Vector2[] newNodes = new Vector2[(_hero.GetHeroSteps() + 1) ^ 2];
        //int[,,] currentNodes = new int[(_hero.GetHeroSteps()+1)^2, (_hero.GetHeroSteps() + 1) ^ 2, (_hero.GetHeroSteps() + 1) ^ 2];
        //int[,,] newNodes = new int[(_hero.GetHeroSteps() + 1) ^ 2, (_hero.GetHeroSteps() + 1) ^ 2, (_hero.GetHeroSteps() + 1) ^ 2];
        int count = 1;
        int CountOfCurrentNodes = 1;
        int CountOfNewNodes = 0;
        int CountOfAllNodes = 1;
        int newCount = 0;
        int start = CountOfAllNodes - CountOfCurrentNodes;
        int io = _heroStats.GetCell().GetIndex()[0];
        int jo = _heroStats.GetCell().GetIndex()[1];
        int stepsCount = _heroStats.GetHeroStepsCount();

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
                //if (i < n - 1 && _cell[i + 1, j].IsCellEmpty() || (_cell[i + 1, j].GetCellState() == State.hero && _cell[i+1, j].GetCellHeroStats().GetHeroTeam() != _heroStats.GetHeroTeam())))
                if (i < n - 1 && _cell[i + 1, j].IsEmpty())
                {
                    currentNodes[CountOfAllNodes + CountOfNewNodes] = SetNearbyCell(i + 1, j);
                    newCount++;
                    CountOfNewNodes++;
                }
                if (i > 0 && _cell[i - 1, j].IsEmpty())
                {
                    currentNodes[CountOfAllNodes + CountOfNewNodes] = SetNearbyCell(i - 1, j);
                    newCount++;
                    CountOfNewNodes++;
                }
                if (j < m - 1 && _cell[i, j + 1].IsEmpty())
                {
                    currentNodes[CountOfAllNodes + CountOfNewNodes] = SetNearbyCell(i, j + 1);
                    newCount++;
                    CountOfNewNodes++;
                }
                if (j > 0 && _cell[i, j - 1].IsEmpty())
                {
                    currentNodes[CountOfAllNodes + CountOfNewNodes] = SetNearbyCell(i, j - 1);
                    newCount++;
                    CountOfNewNodes++;
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

    public Vector2 SetNearbyCell(int i, int j)
    {
        Vector2 node;
        _cell[i, j].SetState(State.nearby);
        RebootCell(_cell[i, j]);
        node = new Vector2(i, j);
        return node;
    }

    public void ShowHero(HeroBehaviour heroBehaviour)
    {
        //Проходим по каждому из героев
        for (int i = 0; i < n; i++)
        {
            if (heroBehaviours[i].GetHeroStats().GetTeam() != heroBehaviour.GetHeroStats().GetTeam())
                heroBehaviours[i].GetHeroStats().GetCell().SetState(State.enemy);
            else heroBehaviours[i].GetHeroStats().GetCell().SetState(State.friend);
            //Перерисовываем нужную клетку
            RebootCell(heroBehaviours[i].GetHeroStats().GetCell());
        }
    }

    public void ChangeTurn(int turn)
    {
        //Проходимся по каждому герою
        for (int i = 0; i < n; i++)
        {
            if (heroBehaviours[i].GetHeroStats().GetTeam() == turn % 2)
            {
                heroBehaviours[i].GetHeroStats().RestoreStepsCount();
            }
            else
            {
                heroBehaviours[i].GetHeroStats().SetStepsCount(0);
            }
        }
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
