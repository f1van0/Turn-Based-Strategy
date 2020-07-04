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

    private int turn = 0;
    private bool isHeroSelected = false;
    private bool isNeededCellSelected = false;

    public event Action<HeroBehaviour> SelectHero;

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
            heroBehaviour.GetHeroStats().SetStepsCount(0);
            //Повторяющийся код
            battlefieldManager.HideAccesibleCells();
            battlefieldManager.ShowHeroes(heroBehaviour);
            battlefieldManager.HighlightCellWithSelectedHero(heroBehaviour);
            battlefieldManager.ShowAccesibleCellsByWave(heroBehaviour);
            //battlefieldManager.RewriteField();
        }
        //else
        //    battlefieldManager.HideAccesibleCells();
    }

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }


    private void Update()
    {
        //Левая кнопка мыши для того, чтобы выбрать героя
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
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
                    battlefieldManager.HideAccesibleCells();
                    isHeroSelected = false;
                }
            }
        }
        //Правая кнопка мыши для того, чтобы сделать какое-то действие героем
        else if (Input.GetMouseButtonDown(1) && isHeroSelected)
        {
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            //Если луч задел что-либо
            if (hit)
            {
                //Если луч задел героя, то выбираем его
                if (hit.transform.tag == "Hero")
                {
                    //Проблема, я отсылаю в функцию SelectCellForHero клетку, хотя там тоже есть разветвление на клетку с врагом и пустой, стоящей рядом
                    selectedNeededCell = hit.transform.gameObject.GetComponent<HeroBehaviour>().GetHeroStats().GetCell();
                    isNeededCellSelected = true;
                    SelectCellForHero(selectedNeededCell);
                }
                //Если лучь задел клетку, то 
                else if (hit.transform.tag == "Cell")
                {
                    selectedNeededCell = hit.transform.gameObject.GetComponent<Cell>();
                    isNeededCellSelected = true;
                    SelectCellForHero(selectedNeededCell);
                }
                //Иначе "рассредотачиваем" внимание / поле
                else
                {
                    battlefieldManager.HideAccesibleCells();
                    isNeededCellSelected = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            turn++;
            battlefieldManager.ChangeTurn(turn);
        }
    }
}
