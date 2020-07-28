using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Chat : MonoBehaviour
{
    public static Chat instance;
    public Transform chatContent;

    public GameObject messagePrefab;

    private ScrollView _scrollView;

    private static int fontHeigth = 16;
    private int contentSize = fontHeigth;
    private int expand = 1;

    private bool isInputFieldActive = false;

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

    public void SendMessageToChat()
    {
        string _message = GetComponentInChildren<InputField>().text;
        GetComponentInChildren<InputField>().text = "";
        if (_message != "")
            ClientSend.SendChatMessage(_message);
    }

    public void AddNewMessage(int _id, string _message)
    {
        Text _messageText = Instantiate(messagePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)).GetComponent<Text>();

        if (_message.Length > 40)
        {
            expand = _message.Length / 40;
            _messageText.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, expand * fontHeigth);
        }

        contentSize += expand * fontHeigth + 2;
        chatContent.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentSize);

        string _nickname = GameManager.players[_id].username;
        
        _messageText.text = _nickname + "[" + _id + "]" + ": " + _message;
        _messageText.transform.SetParent(chatContent, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            if (isInputFieldActive)
            {
                GetComponentInChildren<InputField>().DeactivateInputField();
                isInputFieldActive = false;
            }
            else
            {
                GetComponentInChildren<InputField>().ActivateInputField();
                isInputFieldActive = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            SendMessageToChat();
            GetComponentInChildren<InputField>().DeactivateInputField();
            isInputFieldActive = false;
        }
    }
}
