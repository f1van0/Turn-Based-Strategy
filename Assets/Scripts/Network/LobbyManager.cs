using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    /*
    TODO:
    Сделать отдельную отправку позиции, готовности, никнейма (смена ника после подключения), остальное потом (энергия, дамаг, лучше под это просто отправлять целый heroStats)
    */
    public static LobbyManager instance;

    public GameObject playerLobbyPrefab;

    public Transform spectatorsList;
    public Transform team1List;
    public Transform team2List;

    public Text playersCountInLobby;
    public Button buttonReady;
    public Button buttonJoinSpectators;
    public Button buttonJoinTeam1;
    public Button buttonJoinTeam2;

    public static List<GameObject> players;

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

    public void SetPlayerTeam(int _id, int _team)
    {
        if (_team == 0)
        {
            players[_id - 1].gameObject.transform.SetParent(spectatorsList, false);
        }
        else if (_team == 1)
        {
            players[_id - 1].gameObject.transform.SetParent(team1List, false);
        }
        else
        {
            players[_id - 1].gameObject.transform.SetParent(team2List, false);
        }
    }

    public void SetPlayerUsername(int _id, string _username)
    {
        players[_id - 1].GetComponentInChildren<Text>().text = _username;
    }

    public void SetPlayerReady(int _id, bool _isReady)
    {
        if (_isReady)
        {
            players[_id - 1].GetComponentInChildren<Image>().color = Color.green;
        }
        else
        {
            players[_id - 1].GetComponentInChildren<Image>().color = Color.red;
        }
    }

    public void JoinSpectators()
    {
        buttonJoinSpectators.enabled = false;
        buttonJoinTeam1.enabled = true;
        buttonJoinTeam2.enabled = true;
        GameManager.SetLocalPlayerTeam(0);
    }

    public void JoinTeam1()
    {
        buttonJoinSpectators.enabled = true;
        buttonJoinTeam1.enabled = false;
        buttonJoinTeam2.enabled = true;
        GameManager.SetLocalPlayerTeam(1);
    }

    public void JoinTeam2()
    {
        buttonJoinSpectators.enabled = true;
        buttonJoinTeam1.enabled = true;
        buttonJoinTeam2.enabled = false;
        GameManager.SetLocalPlayerTeam(2);
    }

    public void SetReadiness_ForLocalPlayer()
    {
        GameManager.SetlocalPlayerReady();
    }

    public void SetReadiness_ButtonState_ForLocalPlayer(bool _isReady)
    {
        if (_isReady)
        {
            buttonReady.GetComponentInChildren<Text>().text = "Unready";
        }
        else
        {
            buttonReady.GetComponentInChildren<Text>().text = "Ready";
        }
    }

    public void AddNewPlayer(int _id, string _username, int _team, bool _isReady)
    {
        players.Add(Instantiate(playerLobbyPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)));
        UpdateExsistingPlayer(_id, _username, _team, _isReady);
    }

    public void UpdateExsistingPlayer(int _id, string _username, int _team, bool _isReady)
    {
        SetPlayerUsername(_id, _username);
        SetPlayerReady(_id, _isReady);
        SetPlayerTeam(_id, _team);
    }

    /*
    public void DeleteExistingPlayer(int _id)
    {
        players[_id - 1].SetActive(false);
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        buttonJoinSpectators.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
