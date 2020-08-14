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
    public string ipAddress = "127.0.0.1";
    public int port = 26950;

    private static bool isServerRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        isServerRunning = true;
        Thread mainThread = new Thread(new ThreadStart(Update));
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
}
