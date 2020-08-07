using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    //TopPanel
    public Text turnsCountText;
    public Button readyButton;
    public Button nextTurnButton;

    private int turnNumber = 0;

    //private bool _readyState = false;

    //InfoPanel
    public GameObject infoPanel;
    public Text locationName;
    public Text damage;
    public Text health;
    public Text energy;

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

    public void ShowNextTurnButton()
    {
        nextTurnButton.gameObject.SetActive(true);
    }

    public void ToNextTurn()
    {
        Assets.Scripts.Network.Server.ServerSideComputing.ToNextTurn();
    }

    public void PressButtonReady()
    {
        if ((GameManager.players[GameManager.clientId].team - turnNumber) % 2 == 0)
        {
            GameManager.SendlocalPlayerReady();
        }
    }

    public void SetTurnsNumber(int _turnsNumber)
    {
        turnNumber = _turnsNumber;

        turnsCountText.text = "Turn " + turnNumber.ToString();
    }

    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void UpdateinfoPanel(Cell _cell)
    {
        if (_cell.cellValues.GetHeroValues() != null)
        {
            locationName.text = _cell.cellValues.locationName + "  (" + _cell.cellValues.GetHeroValues().owner + ")";
        }
        else
        {
            locationName.text = _cell.cellValues.locationName;
        }
        /*
        damage.text = _cell.cellValues.GetHeroValues().damage.ToString() + "  (" + _cell.cellValues.damagePerTurn + ")";
        health.text = _cell.cellValues.GetHeroValues().health.ToString() + "  (" + _cell.cellValues.healthPerTurn + ")";
        energy.text = _cell.cellValues.GetHeroValues().energy.ToString() + "  (" + _cell.cellValues.energyPerTurn + ")";
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        nextTurnButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
