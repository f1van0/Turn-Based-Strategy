using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BattleFieldManager battlefield;
    private Camera mainCamera;
    private GameObject hero;
    private GameObject selectedCell;

    private void OnSelectedHero(GameObject hero)
    {
        //battlefield.ShowAccesibleCellsByWave(hero.GetComponent<HeroBehaviour>());
    }

    private void SelectCellForHero(GameObject hero, GameObject cell)
    {
        HeroBehaviour hb = hero.GetComponent<HeroBehaviour>();
        Cell _cell = cell.GetComponent<Cell>();
        hb.MoveToCell(_cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        //battlefield = FindObjectOfType<BattleFieldManager>();
        //battlefield.Initialization();
        mainCamera = FindObjectOfType<Camera>();
    }

    bool isHeroSelected = false;
    bool isCellSelected = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("mousedown");
            if (Physics.Raycast(ray, out hit))
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
