using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICellInfo : MonoBehaviour
{
    public InputField inputFieldNickName;
    public static UICellInfo instance;


    public Text location_text;
    public Text damage_text;
    public Text health_text;
    public Text state_text;

    public void CloseInfoWindow()
    {
        this.gameObject.SetActive(false);
    }

    public void SetInfo(Cell _cell, HeroStats _heroStats)
    {
        location_text.text = "Location:" + _cell.location;
        damage_text.text = "Damage:" + (_cell.damagePerTurn + _heroStats.damage);
        health_text.text = "Health:" + (_cell.healthPerTurn + _heroStats.health);
        state_text.text = "State:" + _cell.GetState().ToString();
    }

    public void SetInfo(Cell _cell)
    {
        location_text.text = "Location:" + _cell.location;
        damage_text.text = "Damage:" + _cell.damagePerTurn;
        health_text.text = "Health:" + _cell.healthPerTurn;
        state_text.text = "State:" + _cell.GetState().ToString();
    }

    public void ShowCellStats(Cell _cell)
    {
        if (_cell.GetHeroStats() != null)
        {
            SetInfo(_cell, _cell.GetHeroStats());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
