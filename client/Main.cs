using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Threading.Tasks;

namespace client
{
    public class Main : BaseScript
    {
        public static string Author = "Abel Gaming";
        public static string ModName = "Disposition Display";
        public static string Version = "1.0";

        public Main()
        {
            //Debug
            Debug.WriteLine($"{ModName} version {Version} by {Author} has been loaded");

            //Commands
            API.RegisterCommand("+das", new Action(OpenDAS), false);
            API.RegisterCommand("-das", new Action(EmptyFunction), false);

            //Key Mappings
            API.RegisterKeyMapping("+das", "Open DAS Menu", "KEYBOARD", "F7");
        }

        private void OpenDAS()
        {
            TriggerServerEvent("DispositionDisplay:OpenMenu");
        }

        private void EmptyFunction()
        {

        }
    }
}
