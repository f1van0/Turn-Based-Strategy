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

    public GameObject playerInfoPrefab;

    public Transform spectatorsList;
    public Transform team1List;
    public Transform team2List;

    public Text playersInLobby;
    public Button buttonReady;
    public Button buttonJoinSpectators;
    public Button buttonJoinTeam1;
    public Button buttonJoinTeam2;
    public PlayerInfo[] playersInfo = new PlayerInfo[4];
    public int myId = 0;
    public int i = 0;
    public Vector2 position = new Vector2(-1f, -1f);
    public bool isLocalPlayerReady = false;

    public void JoinSpectators()
    {
        buttonJoinSpectators.enabled = false;
        ClientSend.SendPlayerPosition();
    }

    public void SetReadibleLocalPlayer()
    {
        isLocalPlayerReady = !isLocalPlayerReady;
        if (isLocalPlayerReady)
        {
            buttonReady.GetComponentInChildren<Text>().text = "Unready";
        }
        else
        {
            buttonReady.GetComponentInChildren<Text>().text = "Ready";
        }

        ClientSend.SendPlayerReadiness();
    }

    public void AddNewPlayer(int _playerId, string _username, Vector2 _position, bool _isReady)
    {
        playersInfo[i] = Instantiate(playerInfoPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)).GetComponent<PlayerInfo>();
        playersInfo[i].gameObject.transform.SetParent(spectatorsList, false);
        SetPlayerInfo(_playerId, _username, _position, _isReady);
        i++;
        playersInLobby.text = "Players in lobby: " + i;
    }

    public void SetPlayerInfo(int _playerId, string _username, Vector2 _position, bool _isReady)
    {
        playersInfo[_playerId -1].ChangeNickName(_username);
        playersInfo[_playerId -1].ChangeReadiness(_isReady);
    }

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
