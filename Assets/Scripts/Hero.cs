using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public GameObject heroGameObject;
    public HeroValues heroValues = null;

    public Hero(GameObject _heroGameObject, HeroValues _heroValues)
    {
        heroGameObject = _heroGameObject;
        heroValues = _heroValues;
    }

    public void SetHeroValues(HeroValues _heroValues)
    {
        heroValues = _heroValues;
    }

    public void Initialize(HeroValues _heroValues)
    {
        heroValues = _heroValues;
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
