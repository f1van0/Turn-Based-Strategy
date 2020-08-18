﻿using System.Collections;
using System.Collections.Generic;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Assets.Scripts.Network.Server;
using UnityEngine;

public class ServerRunner : MonoBehaviour
{
    private int port = 26955;

    private static bool isServerRunning = false;
    private static Thread mainThread;

    // Start is called before the first frame update
    void Start()
    {
        int _newPort = UIManager.instance.StringToAddress().Item2;

        if (_newPort >= 10000)
            port = _newPort;

        isServerRunning = true;

        mainThread = new Thread(new ThreadStart(Update));
        mainThread.Start();

        GameManager.AddNewLocalMessage($"Main thread started. Running at {Constants.ms_per_tick} ticks per second.", MessageType.fromServer);

        Server.Start(4, port);
    }

    // Update is called once per frame
    void Update()
    {
        DateTime _nextLoop = DateTime.Now;
        if (isServerRunning)
        {
            GameLogic.Update();

            while (_nextLoop < DateTime.Now)
            {

                _nextLoop = _nextLoop.AddMilliseconds(Constants.ms_per_tick);

                if (_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }
    }

    //if there are some problems, then they are most likely because of the static function and the thread
    public static void CloseServer()
    {
        Server.Stop();

        isServerRunning = false;
        mainThread.Abort();
    }

    private void OnApplicationQuit()
    {
        CloseServer();
    }
}
