using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public InputField PlayerNickNameField;

    public GameObject serverPrefab;

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

    public void RunServer()
    {
        Instantiate(serverPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    }

    public string GetUserName()
    {
        return PlayerNickNameField.text;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
