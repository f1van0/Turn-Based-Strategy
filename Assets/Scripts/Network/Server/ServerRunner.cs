using System.Collections;
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
    private static bool isServerRunning = false;
    /*
    static void Main(string[] args)
    {
        Console.Title = "Turn-Based-Strategy_Server";
        isServerRunning = true;

        Thread mainThread = new Thread(new ThreadStart(MainThread));
        mainThread.Start();

        Server.Start(4, 26950);
    }
    */
    private static void MainThread()
    {

        /*
        while (isServerRunning)
        {
            while (_nextLoop < DateTime.Now)
            {
                GameLogic.Update();

                _nextLoop = _nextLoop.AddMilliseconds(Constants.ms_per_tick);

                if (_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        isServerRunning = true;
        Thread mainThread = new Thread(new ThreadStart(Update));
        mainThread.Start();
        Debug.Log($"[Server] Main thread started. Running at {Constants.ms_per_tick} ticks per second.");

        Server.Start(4, 26950);
        //StartCoroutine(ServerLoop(1000 / 12));
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
                //GameLogic.Update();

                _nextLoop = _nextLoop.AddMilliseconds(Constants.ms_per_tick);

                if (_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }
    }
}
