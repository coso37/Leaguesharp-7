using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;
namespace LSharpClock
{
    class Program
    {
        
            public static Menu Clock;
        public static String time;
        public static int OffsetX=0;
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Clock = new Menu("Clock","Clock", true);
            Clock.AddItem(new MenuItem("Activate", "Activate")).SetValue(true);
            Clock.AddItem(new MenuItem("AM/PM", "AM/PM")).SetValue(true);
			Clock.AddItem(new MenuItem("ShowSek", "Show seconds?")).SetValue(true);
            Clock.AddItem(new MenuItem("Color", "Color")).SetValue(new Circle(true, Color.White));
            Clock.AddItem(new MenuItem("offX2", "Offset for width").SetValue(new Slider(0, -50, 50)));
            Clock.AddItem(new MenuItem("offY2", "Offset for height").SetValue(new Slider(0, -50, 50)));
            Clock.AddToMainMenu();
                Game.PrintChat("Clock2 loaded");
                Drawing.OnDraw += Drawing_OnDraw;
        }
        private static void Drawing_OnDraw(EventArgs args)
        {

            if (Clock.Item("Activate").GetValue<bool>())
            {
                if (Clock.Item("AM/PM").GetValue<bool>())
                {
                    if (Clock.Item("ShowSek").GetValue<bool>())
                    {
                        time = DateTime.Now.ToString("hh:mm:ss tt", new CultureInfo("en-US"));
                        OffsetX = Clock.Item("offX2").GetValue<Slider>().Value - 12; //10 px for AM /PM
                    }
                    else
                    {
                        time = DateTime.Now.ToString("hh:mm tt", new CultureInfo("en-US"));
                        OffsetX = Clock.Item("offX2").GetValue<Slider>().Value - 12 - 8; //10 px for AM /PM

                    }
                }
                else
                {
                    if (Clock.Item("ShowSek").GetValue<bool>())
                    {

                        time = DateTime.Now.ToString("HH:mm:ss tt");
                        OffsetX = Clock.Item("offX2").GetValue<Slider>().Value;
                    }
                    else
                    {
                        time = DateTime.Now.ToString("HH:mm tt");
                        OffsetX = Clock.Item("offX2").GetValue<Slider>().Value - 8;
                    }
                }
            }
            Drawing.DrawText((Drawing.Width - (Drawing.Width * 0.15f)) + OffsetX, (Drawing.Height * 0.05f) + Clock.Item("offY2").GetValue<Slider>().Value, Clock.Item("Color").GetValue<Circle>().Color, time);               
           }
   
        }
        }
    
