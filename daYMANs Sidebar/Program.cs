using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using ConsoleApplication12.Properties;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;


namespace ConsoleApplication12
{
    internal class Program
    {
        public static Menu SidebarMenu;
        private static readonly IList<enemies> enemyList = new List<enemies>();
        private static string _version;
        private static Sprite Sprite;
        private static float x, y;
        public static string[] NoEnergie =
{
"Aatrox", "DrMundo", "Vladimir",
"Zac","Katarina","Garen" 
};

        public static string[] Energie =
        {
            "Akali", "Kennen", "LeeSin", "Shen", "Zed","Gnar","Katarina","RekSai","Riven","Renekton","Rengar","Rumble",
        };
        private static int hpwidth = 0;
        private static float Height = Drawing.Height;
        private static float Width = Drawing.Width;
        private static int scale = 1;
        static SharpDX.Direct3D9.Font levelFont;
        static SharpDX.Direct3D9.Font hpFont;
        private static Texture HUD, HUDult, hpTexture, manaTexture,blackTexture,energieTexture;
        private static void Main(string[] args)
        {
            Sprite = new Sprite(Drawing.Direct3DDevice);
            HUDult = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.HUDult, typeof(byte[])), 16, 16, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            blackTexture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.schwarz, typeof(byte[])), 62, 90, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            HUD = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.HUDtest, typeof(byte[])), 62, 90, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            hpTexture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.HPbar, typeof(byte[])), 58, 10, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            manaTexture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.MANAbar, typeof(byte[])), 58, 10, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            energieTexture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.Energiebar, typeof(byte[])), 58, 10, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            levelFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new FontDescription()
            {
                FaceName = "Verdana",
                Height = 10,
                OutputPrecision = FontPrecision.Default,
                Quality = FontQuality.Default
            });
            hpFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new FontDescription()
            {
                FaceName = "Verdana",
                Height = 10,
                OutputPrecision = FontPrecision.Default,
                Quality = FontQuality.Default
            });
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Print(String text)
        {
            Game.PrintChat("<font color='#ff3232'>Sidebar: </font> <font color='#FFFFFF'>" + text + "</font>");
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            SidebarMenu = new Menu("sidebar", "sidebar", true);
            SidebarMenu.AddItem(new MenuItem("Activate", "Activate")).SetValue(true);
            SidebarMenu.AddItem(new MenuItem("offX2", "Offset for width").SetValue(new Slider(0, -1680, 1680)));
            SidebarMenu.AddItem(new MenuItem("offY2", "Offset for height").SetValue(new Slider(0, -1680, 1680)));
            SidebarMenu.AddItem(new MenuItem("offX3", "Offset for width2").SetValue(new Slider(0, -100, 100)));
            SidebarMenu.AddItem(new MenuItem("offY3", "Offset for height2").SetValue(new Slider(0, -100, 100)));
            SidebarMenu.AddToMainMenu();


            var attempt = 0;
            _version = GameVersion();
            Print(_version);
            while (string.IsNullOrEmpty(_version) && attempt < 5)
            {
                _version = GameVersion();
                Print("attempt: " + attempt);
                attempt++;
            }//funtzt
            if (!string.IsNullOrEmpty(_version))
            {
                LoadImages();
                Print("Loaded! ");
                Drawing.OnDraw += Drawing_OnDraw;

            }
        }
        private static void CurrentDomainOnDomainUnload(object sender, EventArgs e)
        {
            Sprite.Dispose();
        }
        private static void DrawingOnPostReset(EventArgs args)
        {
            Sprite.OnResetDevice();
        }
        private static void DrawingOnPreReset(EventArgs args)
        {
            Sprite.OnLostDevice();
        }
        static void Drawing_OnDraw(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
                return;
            try
            {
                if (SidebarMenu.Item("Activate").GetValue<bool>()) //drawHUD
                {
                    x = -Width + (62 * scale);
                    y = Height * -.10f;

                    //   Print("x:"+x + "y:" + y);
                    foreach (var enemie in enemyList)
                    {



                  
                            Sprite.Begin(); //DRAW icon 255, 255, 255, 255
                            Sprite.Draw(enemie.Icon, new ColorBGRA(255, 255, 255, 255), null, new Vector3(x - 5, y - 8, 0), null);
                            Sprite.End();

                            if (enemie.Hero.IsDead)
                            {
                               Print(enemie.Hero.DeathDuration.ToString());
                               //todo get respawn timer
                           }
                        
                      

                        String HP = Math.Round(enemie.Hero.Health) + "/" + Math.Round(enemie.Hero.MaxHealth);
                       
                        int hplength = ((58-(HP.Length * 5 ))/2); //to center text
                        hpwidth = Convert.ToInt32(((58f / 100f) * (enemie.Hero.HealthPercentage())));
                        Sprite.Begin(); //DRAW HUD
                        // //ziel:-1617 / -124 //bild 1 -4/-26 55x55
                        Sprite.Draw(HUD, new ColorBGRA(255, 255, 255, 255), null, new Vector3(x, y, 0), null); //todo add % value for heigh 
                        Sprite.End();
                        // //draw level  weiss =    248-248-255
                        levelFont.DrawText(null, enemie.Hero.Level.ToString(), (int)x * -1 + 48, (int)y * -1 + 52, new ColorBGRA(248, 248, 255, 255));

                        
                        if (enemie.Hero.Spellbook.GetSpell(SpellSlot.R).CooldownExpires < Game.Time && enemie.Hero.Spellbook.GetSpell(SpellSlot.R).Level>0)
                        {
                            Sprite.Begin();
                            Sprite.Draw(HUDult, new ColorBGRA(255, 255, 255, 255), null, new Vector3(x + -46, y + -2, 0), null);
                            Sprite.End();

                        }
                       Print(enemie.Hero.ChampionName);
                        if (!NoEnergie.Contains(enemie.Hero.ChampionName))
                        {
                            String Mana = Math.Round(enemie.Hero.Mana) + "/" + Math.Round(enemie.Hero.MaxMana);
                            int manawidth = Convert.ToInt32(((58f/100f)*(enemie.Hero.ManaPercentage())));
                            int Manalength = ((58 - (Mana.Length*5))/2); //to center t
                            //draw MANA /Manabar
                            Sprite.Begin();
                            if (!Energie.Contains(enemie.Hero.ChampionName))
                            {
                                Sprite.Draw(manaTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, manawidth, 10), new Vector3(x - 2, y - 57 - 7 - 14, 0), null);
                            }
                            else
                            {
                                Sprite.Draw(manaTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, manawidth, 10), new Vector3(x - 2, y - 57 - 7 - 14, 0),
                                    null);
                            }
                            Sprite.End();
                            hpFont.DrawText(null, Mana, (int) x*-1 + 2 + Manalength, (int) y*-1 + 65 + 14,
                                new ColorBGRA(248, 248, 255, 255));
                        }
                        else

                        //draw HP/MAXHP 
                        
                        Sprite.Begin();
                        Sprite.Draw(hpTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, hpwidth, 10), new Vector3(x - 2, y - 57 - 7, 0), null);
                        Sprite.End();
                        hpFont.DrawText(null, HP, (int)x * -1 + 2 + hplength, (int)y * -1 + 65, new ColorBGRA(248, 248, 255, 255));
                        if (!enemie.Hero.IsVisible||enemie.Hero.IsDead)//make it black :)
                        {
                            Sprite.Begin(); //DRAW icon 255, 255, 255, 255
                            Sprite.Draw(blackTexture, new ColorBGRA(255, 255, 255, 110), null, new Vector3(x, y, 0), null);
                            Sprite.End();
                        }
                        y = y - 94;
                     }
                }
            }
            catch
            {

            }
        }

        //TheSaltyWaffle Universal Minimaphack (fetching Champion Icons)

        #region fetch images

        private static void LoadImages()
        {
            foreach (Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero != null && hero.Team != ObjectManager.Player.Team && hero.IsValid))
            {
                LoadImage(hero);
            }
        }

        private static void LoadImage(Obj_AI_Hero hero)
        {
            Bitmap bmp = null;
            if (File.Exists(GetImageCached(hero.ChampionName)))
            {
                bmp = new Bitmap(GetImageCached(hero.ChampionName));//works like a charm
            }
            else
            {
                int attempt = 0;
                 bmp = DownloadImage(hero.ChampionName);
                while (bmp == null && attempt < 5)
                {
                    bmp = DownloadImage(hero.ChampionName);

                    attempt++;

                }
                if (bmp == null)
                {

                    Game.PrintChat("Failed to load " + hero.ChampionName + " after " + attempt + 1 + " attempts!");
                }
                else
                {
                    
                    bmp.Save(GetImageCached(hero.ChampionName));
                }
            }
            if (bmp != null)
            {
                var enemie = new enemies(hero, bmp);
                enemyList.Add(enemie);
            } Print(GetImageCached(hero.ChampionName));
        }

        private float GetScale()
        {
            // return _slider.GetValue<Slider>().Value / 100f;
            return 1;
        }

        private static Bitmap DownloadImage(string champName)
        {
            WebRequest request =
                WebRequest.Create("http://ddragon.leagueoflegends.com/cdn/" + _version + "/img/champion/" + champName +
                                  ".png");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
                Stream responseStream;
                using (responseStream = response.GetResponseStream())
                {
                    return responseStream != null && responseStream != Stream.Null ? new Bitmap(responseStream) : null;
                }

            }
        }




        public static string GameVersion()
        {
            String json = new WebClient().DownloadString("http://ddragon.leagueoflegends.com/realms/euw.json");
            return (string)new JavaScriptSerializer().Deserialize<Dictionary<String, Object>>(json)["v"];
        }

        public static string GetImageCached(string champName)
        {
            string path = Path.GetTempPath() + "Sidebar";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "\\" + _version;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + "\\" + champName + ".png";
        }

        #endregion

        //end copy past

        private class enemies
        {
            //public Render.Sprite Image { get; set; }
            //public Render.Text Text { get; set; }
            //public Render.Rectangle Rect { get; set; }
            public Obj_AI_Hero Hero { get; set; }
            public Texture Icon { get; set; }

            public enemies(Obj_AI_Hero hero, Bitmap bmp)
            {
                // TODO: Complete member initialization
                this.Hero = hero;

                this.Icon = Texture.FromMemory(Drawing.Direct3DDevice,
                    (byte[])new ImageConverter().ConvertTo(bmp, typeof(byte[])), 55, 55, 0, Usage.None, Format.A1,
                    Pool.Managed, Filter.Default, Filter.Default, 0);

            }


        }
    }
}


