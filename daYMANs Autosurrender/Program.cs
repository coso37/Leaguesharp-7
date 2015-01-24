using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace daYMANs_Autosurrender
{
    class Program
    {
        private static Menu menu;
        private static int calledMin;
        private static Stopwatch tmr;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
       menu =new Menu("Autosurrender","Autosurrender",true);
       menu.AddItem(new MenuItem("Activate", "Activate")).SetValue(true);
            menu.AddToMainMenu();
       Print("Loaded!");
            tmr = new Stopwatch();
            tmr.Start();
          
       Game.OnGameUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (menu.Item("Activate").GetValue<bool>())
            {


                if (canSurrender())
                {
                    Game.Say("/ff");
                }
            }
        }

        private static bool canSurrender()
        {
                   //   Print("time:"+tmr.Elapsed);
            if (20 <= tmr.Elapsed.Minutes && (tmr.Elapsed.Minutes - 0) % 3 == 0 && tmr.Elapsed.Minutes== calledMin && (Utility.Map.GetMap().Type == Utility.Map.MapType.HowlingAbyss || Utility.Map.GetMap().Type == Utility.Map.MapType.SummonersRift))
            {//todo add other maps
                calledMin = tmr.Elapsed.Minutes+3;
                return true;
            }
            if (15 <= tmr.Elapsed.Minutes && (tmr.Elapsed.Minutes-15)%3 == 0&&(Utility.Map.GetMap().Type==Utility.Map.MapType.TwistedTreeline||Utility.Map.GetMap().Type==Utility.Map.MapType.CrystalScar))
            {//todo add other maps
                Print("2");
                return true;
            }

            return false;
        }
        private static void Print(String text)
        {
            Game.PrintChat("<font color='#ff3232'>Autosurrender: </font> <font color='#FFFFFF'>" + text + "</font>");
        }
    }
}
