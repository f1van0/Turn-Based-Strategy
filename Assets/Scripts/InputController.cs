using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;
    private Cell selectedNeededCell;
    private BattleFieldManager battlefieldManager;
    private HeroBehaviour heroBehaviour;
    private HeroBehaviour focusedHero;
    private Cell focusedCell;

    public Canvas canvasCellInfo;
    private UICellInfo infoPanel;

    private bool isHeroSelected = false;

    public event Action<HeroBehaviour> SelectHero;
    public event Action DefocusHero;
    public event Action<GameObject, HeroBehaviour> SelectCell;
    public event Action<int> ChangeTurn;

    private Vector2 rayPos;
    private RaycastHit2D hit;

    public HeroValues heroValues;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        infoPanel = canvasCellInfo.GetComponent<UICellInfo>();
    }


    private void Update()
    {
        rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
        //Если луч задел что-либо
        if (hit)
        {
            //TODO: Информация при наведении

            //Посмотреть информацию о клетке в InfoPanel в GameUI либо выбрать персонажа на клетке, сделав его активным.
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.tag == "Cell")
                {
                    //Если эта клетка занята подконтрольным вам героем, то необходимо узнать у сервера информацию о доступных для перемещения клеток
                    Cell _cell = hit.transform.GetComponent<Cell>();
                    if (_cell.cellValues.GetHeroValues().owner != "None")
                    {
                        //TODO: Show AccesibleCellsForHero
                        //BattleFieldManager.instance.ShowAccesibleCellsByWave(_cell);
                        heroValues = _cell.cellValues.GetHeroValues();
                        isHeroSelected = true;
                        BattleFieldManager.instance.SelectHero(_cell.cellValues.position);
                    }
                    //Если эта клетка занята героем-соперником/другом либо является свободной, то просто выводится информация о клетке и клетка считается выбранной
                    else
                    {
                        //TODO: Show InfoPanel
                        //GameUI.instance.OpenInfoPanel();
                        //GameUI.instance.UpdateinfoPanel(_cell);
                        isHeroSelected = false;
                        BattleFieldManager.instance.HideAvailableCells();
                    }
                }
            }
            //Правая кнопка мыши для того, чтобы сделать какое-то действие героем (например переместиться в клетку либо атаковать)
            else if (Input.GetMouseButtonDown(1) && isHeroSelected && hit.transform.tag == "Cell")
            {
                Cell _cell = hit.transform.GetComponent<Cell>();
                if (_cell.cellValues.GetHeroValues().owner == "None" && BattleFieldManager.instance.isAvailableCellSelected(_cell.cellValues.position))
                {
                    //                      (hero's id,      previous position,    next position)
                    GameManager.SendMoveHero(heroValues.ID, heroValues.position, _cell.cellValues.position);
                    BattleFieldManager.instance.HideAvailableCells();
                }
            }
        }
    }
    
    /*
    private void SelectCellForHero(Cell _cell)
    {
        CellState state = _cell.GetState();
        if (state == CellState.nearby)
        {
            isNeededCellSelected = true;
            heroBehaviour.MoveToCell(_cell);
            //Повторяющийся код
            battlefieldManager.HideAccesibleCells();
            battlefieldManager.ShowHeroes(heroBehaviour);
            battlefieldManager.HighlightCellWithSelectedHero(heroBehaviour);
            battlefieldManager.ShowAccesibleCellsByWave(heroBehaviour);
            //battlefieldManager.RewriteField();
        }
        else if (state == CellState.attack)
        {
            isNeededCellSelected = true;
            heroBehaviour.SetTargetID(_cell.GetHeroStats().ID);
            heroBehaviour.GetHeroStats().SetEnergyCount(0);
            //Повторяющийся код
            battlefieldManager.HideAccesibleCells();
            battlefieldManager.ShowHeroes(heroBehaviour);
            battlefieldManager.HighlightCellWithSelectedHero(heroBehaviour);
            battlefieldManager.ShowAccesibleCellsByWave(heroBehaviour);
        }
    }

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        //canvasCellInfo = FindObjectOfType<Canvas>();
        infoPanel = canvasCellInfo.GetComponent<UICellInfo>();
    }


    private void Update()
    {
        rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
        //Если луч задел что-либо
        if (hit)
        {
            //Информация при наведении
            /*
            if (hit.transform.TryGetComponent<HeroBehaviour>(out focusedHero))
            {
                //focusedHero.GetHeroStats().GetCell().ShowInfo;
            }
            else if (hit.transform.TryGetComponent<Cell>(out focusedCell))
            {
                //focusedCell.ShowInfo();
            }
            */
            /*
            if (Input.GetMouseButtonDown(0))
            {
                //Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                //RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
                //Если луч задел героя, то выбираем его
                if (hit.transform.tag == "Hero")
                {
                    isHeroSelected = true;
                    heroBehaviour = hit.transform.GetComponent<HeroBehaviour>();
                    //Выбран герой heroBehaviour
                    SelectHero(heroBehaviour);
                }
                //Иначе "рассредотачиваем" внимание / поле
                else
                {
                    if (hit.transform.tag == "Cell")
                    {
                        canvasCellInfo.gameObject.SetActive(true);
                        infoPanel.ShowCellStats(hit.transform.GetComponent<Cell>());
                    }
                    //battlefieldManager.HideAccesibleCells();
                    DefocusHero();
                    isHeroSelected = false;
                }
            }
            //Правая кнопка мыши для того, чтобы сделать какое-то действие героем
            else if (Input.GetMouseButtonDown(1) && isHeroSelected)
            {
                //Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                //RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
                //Если луч задел что-либо
                SelectCell(hit.transform.gameObject, heroBehaviour);
            }
        }
        //Левая кнопка мыши для того, чтобы выбрать героя
        if (Input.GetMouseButtonDown(0))
        {
            //Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            //RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            //Если луч задел что-либо
            if (hit)
            {
                //Если луч задел героя, то выбираем его
                if (hit.transform.tag == "Hero")
                {
                    isHeroSelected = true;
                    heroBehaviour = hit.transform.GetComponent<HeroBehaviour>();
                    //Выбран герой heroBehaviour
                    SelectHero(heroBehaviour);
                }
                //Иначе "рассредотачиваем" внимание / поле
                else
                {
                    //battlefieldManager.HideAccesibleCells();
                    DefocusHero();
                    isHeroSelected = false;
                }
            }
        }
        //Правая кнопка мыши для того, чтобы сделать какое-то действие героем
        else if (Input.GetMouseButtonDown(1) && isHeroSelected)
        {
            //Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            //RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            //Если луч задел что-либо
            if (hit)
            {
                SelectCell(hit.transform.gameObject,heroBehaviour);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            turn++;
            ChangeTurn(turn);
        }
    }
        */
        }
