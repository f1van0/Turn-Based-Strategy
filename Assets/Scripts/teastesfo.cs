using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teastesfo : MonoBehaviour
{
    private BattleFieldManager battlefield;
    private Camera mainCamera;
    private GameObject hero;
    private GameObject selectedCell;

    private void OnSelectedHero(GameObject hero)
    {
        HeroStats hs = hero.GetComponent<HeroBehaviour>().GetHeroStats();
        battlefield.ShowAccesibleCellsByWave(hs);
    }

    private void SelectCellForHero(GameObject hero, GameObject cell)
    {
        HeroBehaviour hb = hero.GetComponent<HeroBehaviour>();
        Cell _cell = cell.GetComponent<Cell>();
        hb.MoveHeroToCell(_cell);
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
                Debug.Log("ray is here");
                if (hit.transform.tag == "Hero")
                {
                    isHeroSelected = true;
                    hero = hit.transform.gameObject;
                    OnSelectedHero(hero);
                }
                else if (isHeroSelected && hit.transform.tag == "Cell")
                {
                    selectedCell = hit.transform.gameObject;
                    isCellSelected = true;
                    SelectCellForHero(hero, selectedCell);
                }
            }
        }
    }
}
