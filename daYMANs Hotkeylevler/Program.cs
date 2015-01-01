using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace daYMANs_Hotkeylevler
{
    internal class Program
    {
        private static Menu menu;
        private static int lastlevel;
        private static Boolean sequenceset = false;
        private static string nextlevelup,draw;
        private static float timeset;
        private static Obj_AI_Hero hero
        {
            get { return ObjectManager.Player; }
        }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {

            menu = new Menu("Autolevler", "Autolevler", true);
            menu.AddItem(
                new MenuItem("hootkey", "combo hotkey").SetValue(new KeyBind(20, KeyBindType.Press)));
            menu.AddItem(new MenuItem("Activate", "Activate")).SetValue(true);
            menu.AddItem(new MenuItem("writeinfo", "Write info?")).SetValue(true);
            menu.AddItem(new MenuItem("Q", "Q").SetValue(new KeyBind(81, KeyBindType.Press)));
            menu.AddItem(new MenuItem("W", "W").SetValue(new KeyBind(87, KeyBindType.Press)));
            menu.AddItem(new MenuItem("E", "E").SetValue(new KeyBind(69, KeyBindType.Press)));
            menu.AddItem(new MenuItem("R", "R").SetValue(new KeyBind(82, KeyBindType.Press)));
            menu.AddItem(new MenuItem("Color", "Color for the Information")).SetValue(new Circle(true, Color.White));
            menu.AddItem(new MenuItem("offX3", "Positon X").SetValue(new Slider((int)(Drawing.Width*0.75f), 0, Drawing.Width)));
            menu.AddItem(new MenuItem("offY3", "Positon Y").SetValue(new Slider(10, 0, Drawing.Height)));
            menu.AddToMainMenu();
            Print("Loaded!");
            lastlevel = hero.Level;
            Game.OnGameUpdate += Game_OnGameUpdate;

        }

        private static void Print(String text)
        {
            Game.PrintChat("<font color='#ff3232'>Hotkeylever: </font> <font color='#FFFFFF'>" + text + "</font>");
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (menu.Item("Activate").GetValue<bool>())
            {
                if (hero.Level != lastlevel && sequenceset)
                {
                    SpellSlot abilitySlot = SpellSlot.Recall;
                    switch (nextlevelup)
                    {
                        case "q":
                            abilitySlot = SpellSlot.Q;
                            break;
                        case "w":
                            abilitySlot = SpellSlot.W;
                            break;
                        case "e":
                            abilitySlot = SpellSlot.E;
                            break;

                        case "r":
                            abilitySlot = SpellSlot.R;
                            break;
                        default:
                            Print("Error in code?");
                            break;
                    }
                    if (abilitySlot != SpellSlot.Recall)
                    {
                        ObjectManager.Player.Spellbook.LevelSpell(abilitySlot);
                    }
                    lastlevel = hero.Level;
                    sequenceset = false;
                }
                if (menu.Item("hootkey").GetValue<KeyBind>().Active && menu.Item("Q").GetValue<KeyBind>().Active &&
                    timeset + 0.5f < Game.Time)
                {
                    sequenceset = true;
                    nextlevelup = "q";
                    timeset = Game.Time;

                }
                if (menu.Item("hootkey").GetValue<KeyBind>().Active && menu.Item("W").GetValue<KeyBind>().Active &&
                    timeset + 0.5f < Game.Time)
                {
                    sequenceset = true;
                    nextlevelup = "w";
                    timeset = Game.Time;

                }
                if (menu.Item("hootkey").GetValue<KeyBind>().Active && menu.Item("E").GetValue<KeyBind>().Active &&
                    timeset + 0.5f < Game.Time)
                {
                    sequenceset = true;
                    nextlevelup = "e";
                    timeset = Game.Time;

                }
                if (menu.Item("hootkey").GetValue<KeyBind>().Active && menu.Item("R").GetValue<KeyBind>().Active &&
                    timeset + 0.5f < Game.Time)
                {
                    sequenceset = true;
                    nextlevelup = "r";
                    timeset = Game.Time; 
                }
                if (menu.Item("writeinfo").GetValue<bool>())
                {
                    if (sequenceset)
                    {
                        draw = nextlevelup.ToUpper();
                    }
                    else
                    {
                        {
                            draw = "not set";
                        }
                    }
                    Drawing.DrawText(menu.Item("offX3").GetValue<Slider>().Value, menu.Item("offY3").GetValue<Slider>().Value, menu.Item("Color").GetValue<Circle>().Color, draw);
                 
                }
            }
        }
    }
}

