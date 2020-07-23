using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Network.Server
{
    class GameLogic
    {
        public static void Update()
        {
            /* **New**
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    _client.player.Update();
                }
            }
            */
            ThreadManager.UpdateMain();
        }
    }
}
