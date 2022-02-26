using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["DispositionDisplay:OpenMenu"] += new Action<Player>(OpenMenu);
        }

        private void OpenMenu([FromSource] Player player)
        {
            //Create new player list
            PlayerList players = new PlayerList();

            //Create two dynamic list
            List<dynamic> playerList = new List<dynamic>();
            List<dynamic> playerNameList = new List<dynamic>();

            //Add info to each list
            foreach (Player p in players)
            {
                playerList.Add(p.Handle);
                playerNameList.Add(p.Name);
            }

            //Trigger the client event
            player.TriggerEvent("DispositionDisplay:OpenMenuReturn", playerList, playerNameList);
        }
    }
}
