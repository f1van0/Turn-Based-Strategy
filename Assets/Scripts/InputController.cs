using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject hero;
    private GameObject selectedCell;
    private BattleFieldManager battlefield;
    private HeroBehaviour heroBehaviour;
    private HeroStats heroStats;

    private void OnSelectedHero(GameObject hero)
    {
        heroBehaviour = hero.GetComponent<HeroBehaviour>();
        heroStats = heroBehaviour.GetHeroStats();
        battlefield.ShowAccesibleCellsByWave(heroStats);
    }

    private void SelectCellForHero(GameObject hero, GameObject cell)
    {
        Cell _cell = cell.GetComponent<Cell>();
        heroBehaviour.MoveHeroToCell(_cell);
        battlefield.HideAccesibleCells();
        battlefield.ShowAccesibleCellsByWave(heroStats);
    }

    // Start is called before the first frame update
    void Start()
    {
        battlefield = FindObjectOfType<BattleFieldManager>();
        battlefield.Initialization();
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
                else if (isHeroSelected && hit.transform.tag == "Cell" && hit.transform.GetComponent<Cell>().GetCellState() == State.nearby)
                {
                    selectedCell = hit.transform.gameObject;
                    isCellSelected = true;
                    SelectCellForHero(hero, selectedCell);
                }
                else battlefield.HideAccesibleCells();
            }
        }
    }
}
