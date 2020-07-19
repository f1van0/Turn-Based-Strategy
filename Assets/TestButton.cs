using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    Selectable selectableText;
    // Start is called before the first frame update
    void Start()
    {
         selectableText = this.GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
