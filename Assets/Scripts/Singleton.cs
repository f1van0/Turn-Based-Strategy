using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Singleton<T> where T: MonoBehaviour
{
    private static T _instance;
    public static T Instance {
        get {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<T>();
            return _instance;
        } }
}