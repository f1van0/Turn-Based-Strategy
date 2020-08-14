using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;

    private int selectedHeroId = -1;

    private Vector2 rayPos;
    private RaycastHit2D hit;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
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
                    int _heroIdInCell = _cell.cellValues.heroId;

                    if (_heroIdInCell != -1)
                    {
                        //TODO: Show AccesibleCellsForHero
                        //BattleFieldManager.instance.ShowAccesibleCellsByWave(_cell);

                        selectedHeroId = _heroIdInCell;
                        BattleFieldManager.instance.SelectHero(_heroIdInCell);
                    }
                    //Если эта клетка занята героем-соперником/другом либо является свободной, то просто выводится информация о клетке и клетка считается выбранной
                    else
                    {
                        selectedHeroId = -1;
                        BattleFieldManager.instance.ClearAvailableCells();
                    }
                }
                else if (hit.transform.tag == "Hero")
                {
                    Hero _hero = hit.transform.GetComponent<Hero>();

                    selectedHeroId = _hero.heroValues.ID;
                    BattleFieldManager.instance.SelectHero(_hero.heroValues.ID);
                }
            }
            //Правая кнопка мыши для того, чтобы сделать какое-то действие героем (например переместиться в клетку либо атаковать)
            else if (Input.GetMouseButtonDown(1) && selectedHeroId != -1)
            {
                if (hit.transform.tag == "Cell")
                {
                    Cell _cell = hit.transform.GetComponent<Cell>();
                    if (BattleFieldManager.instance.isAvailableCellSelected(_cell.cellValues.position))
                    {
                        if (_cell.cellValues.heroId == -1)
                        {
                            GameManager.SendMoveHero(selectedHeroId, _cell.cellValues.position);
                        }
                        else
                        {
                            HeroValues attackingHeroValues = BattleFieldManager.instance.GetHeroValuesById(selectedHeroId);
                            HeroValues attackedHeroValues = BattleFieldManager.instance.GetHeroValuesById(_cell.cellValues.heroId);

                            //Is enemy attacked (not teammate)
                            if (attackingHeroValues.team != attackedHeroValues.team)
                            {
                                GameManager.SendAttackHero(selectedHeroId, _cell.cellValues.heroId);
                            }
                        }

                        selectedHeroId = -1;
                    }
                }
                else if (hit.transform.tag == "Hero")
                {
                    Hero _targetHero = hit.transform.GetComponent<Hero>();
                    HeroValues _selectedHeroValues = BattleFieldManager.instance.GetHeroValuesById(selectedHeroId);

                    //Is available cell selected and is enemy attacked (not teammate)
                    if (BattleFieldManager.instance.isAvailableCellSelected(_targetHero.heroValues.position) && (_targetHero.heroValues.team != _selectedHeroValues.team))
                    {
                        GameManager.SendAttackHero(selectedHeroId, _targetHero.heroValues.ID);

                        selectedHeroId = -1;
                    }
                }
            }
        }
    }
}
