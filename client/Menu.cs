using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using System;
using System.Collections.Generic;

namespace client
{
    public class Menu : BaseScript
    {
        public static MenuPool _menuPool;
        public static UIMenu mainMenu;
        public static List<dynamic> playerList;
        public static List<dynamic> playerNameList;
        public static string SelectedPlayerID = null;
        public static string SelectedPlayerName = null;
        public static int SelectedPlayer;
        public static bool IsPlayerSelected = false;
        public static int CreatedBlip;

        public void MainOptions(UIMenu menu)
        {
            //Create the list
            playerList = new List<dynamic>();
            playerNameList = new List<dynamic>();

            //Add placeholders
            playerList.Add("0");
            playerNameList.Add("John Doe");

            //Create menu list items
            var activePlayerNames = new UIMenuListItem("Player", playerNameList, 0);
            var activePlayers = new UIMenuListItem("Player", playerList, 0);

            //Add to menu
            menu.AddItem(activePlayerNames);

            //Do change events
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == activePlayerNames)
                {
                    activePlayers.Index = activePlayerNames.Index; //This sets so the ID will match the selected player
                }
            };

            //Do select events
            menu.OnListSelect += (sender, item, index) =>
            {
                if (item == activePlayerNames)
                {
                    SelectedPlayerName = activePlayerNames.Items[index].ToString();
                    SelectedPlayerID = activePlayers.Items[index].ToString();
                    SelectedPlayer = API.GetPlayerFromServerId(int.Parse(SelectedPlayerID));
                    IsPlayerSelected = true;
                    Screen.ShowNotification($"~y~[DAS]~w~ Drawing route to ~b~{SelectedPlayerName} ~o~({SelectedPlayerID})");
                }
            };

            //Clear route item
            var clearroute = new UIMenuItem("~o~Clear Route");
            menu.AddItem(clearroute);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == clearroute)
                {
                    Screen.ShowNotification("~y~[DAS]~w~ Route has been cleared");
                    IsPlayerSelected = false;
                    SelectedPlayerName = null;
                    SelectedPlayerID = null;
                    API.ClearGpsPlayerWaypoint();
                    API.SetWaypointOff();
                }
            };
        }

        public Menu()
        {
            //Events
            EventHandlers["DispositionDisplay:OpenMenuReturn"] += new Action<List<dynamic>, List<dynamic>>(GetPlayerList);

            //Create Menu Items
            _menuPool = new MenuPool();
            mainMenu = new UIMenu("Disposition Display", "Mod by ~b~Abel Gaming");
            _menuPool.Add(mainMenu);

            //Add menu options
            MainOptions(mainMenu);

            //Process Menu
            _menuPool.MouseEdgeEnabled = false;
            _menuPool.ControlDisablingEnabled = false;
            _menuPool.RefreshIndex();

            //Tick
            Tick += async () =>
            {
                _menuPool.ProcessMenus();
            };
            Tick += async () =>
            {
                if (IsPlayerSelected)
                {
                    //Get the player ID
                    int playerID = int.Parse(SelectedPlayerID);

                    //Get the position
                    Vector3 position = API.NetworkGetPlayerCoords(SelectedPlayer);

                    //Draw the waypoint
                    API.SetNewWaypoint(position.X, position.Y);

                    //Get the street name
                    string street = World.GetStreetName(position);

                    //Draw Recentangle
                    API.DrawRect(0.5f, 0.05f, 0.2f, 0.1f, 0, 0, 0, 150);

                    //Draw Title Text
                    API.SetTextScale(0.4f, 0.4f);
                    API.SetTextFont(6);
                    API.SetTextProportional(true);
                    API.SetTextColour((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                    API.SetTextOutline();
                    API.SetTextEntry("STRING");
                    API.AddTextComponentString("~y~ROUTE DETAILS");
                    API.DrawText(0.46f, 0.0f);
                    API.EndTextComponent();

                    //Draw Officer
                    API.SetTextScale(0.5f, 0.5f);
                    API.SetTextFont(6);
                    API.SetTextProportional(true);
                    API.SetTextColour((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                    API.SetTextOutline();
                    API.SetTextEntry("STRING");
                    API.AddTextComponentString($"Officer: ~b~{SelectedPlayerName}");
                    API.DrawText(0.405f, 0.03f);
                    API.EndTextComponent();

                    //Draw Street
                    API.SetTextScale(0.5f, 0.5f);
                    API.SetTextFont(6);
                    API.SetTextProportional(true);
                    API.SetTextColour((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                    API.SetTextOutline();
                    API.SetTextEntry("STRING");
                    API.AddTextComponentString($"Location: ~b~{street}");
                    API.DrawText(0.405f, 0.06f);
                    API.EndTextComponent();
                }
            };
        }

        private void GetPlayerList(List<dynamic>playerListReturn, List<dynamic>playerNamesReturn)
        {
            //Clear lists
            playerList.Clear();
            playerNameList.Clear();

            //Add the players to the list
            foreach (dynamic player in playerListReturn)
            {
                playerList.Add(player);
            }
            foreach (dynamic playerName in playerNamesReturn)
            {
                playerNameList.Add(playerName);
            }

            //Open Menu
            mainMenu.Visible = !mainMenu.Visible;
        }
    }
}
