using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject hero;
    private GameObject selectedCell;
    private BattleFieldManager battlefieldManager;
    private HeroBehaviour heroBehaviour;
    private HeroStats heroStats;

    private int turn = 0;

    private void OnSelectedHero(GameObject hero)
    {
        heroBehaviour = hero.GetComponent<HeroBehaviour>();
        heroStats = heroBehaviour.GetHeroStats();
        battlefieldManager.ShowAccesibleCellsByWave(heroStats);
        battlefieldManager.ShowHero(heroBehaviour);
    }

    private void SelectCellForHero(GameObject hero, GameObject cell)
    {
        Cell _cell = cell.GetComponent<Cell>();
        heroBehaviour.MoveToCell(_cell);
        battlefieldManager.HideAccesibleCells();
        battlefieldManager.ShowAccesibleCellsByWave(heroStats);
    }

    // Start is called before the first frame update
    void Start()
    {
        battlefieldManager = FindObjectOfType<BattleFieldManager>();
        battlefieldManager.Initialization();
        mainCamera = FindObjectOfType<Camera>();
    }

    bool isHeroSelected = false;
    bool isCellSelected = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            if (hit)
            {
                if (hit.transform.tag == "Hero")
                {
                    isHeroSelected = true;
                    hero = hit.transform.gameObject;
                    OnSelectedHero(hero);
                }
                else if (isHeroSelected && hit.transform.tag == "Cell" && hit.transform.GetComponent<Cell>().GetState() == State.nearby)
                {
                    selectedCell = hit.transform.gameObject;
                    isCellSelected = true;
                    SelectCellForHero(hero, selectedCell);
                }
                else battlefieldManager.HideAccesibleCells();
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("changed");
            battlefieldManager.ChangeTurn(turn);
            turn++;
        }
    }
}
