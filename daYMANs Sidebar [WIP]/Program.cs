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
using System.Threading;
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
        private static Obj_AI_Hero hero { get { return ObjectManager.Player; } }
        private static float x, y;
        public static SpellSlot[] SummonerSpellSlots = { ((SpellSlot)4), ((SpellSlot)5) };
        private static float[] respawntime = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static string[] SummonersNames =
{
"summonerbarrier", "summonernoost", "summonerclairvoyance",
"summonerdot", "summonerexhaust", "summonerflash", "summonerhaste", "summonerheal", "summonermana",
"summonerodingarrison", "summonerrevive", "summonersmite", "summonerteleport"
};
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
        static SharpDX.Direct3D9.Font small, respawnfont,medium;
        private static Texture HUD, HUDult, hpTexture, manaTexture, blackTexture, energieTexture;
        private static Texture temp, summonerheal, summonerbarrier, summonerboost, summonerclairvoyance, summonerdot, summonerexhaust, summonerflash, summonerhaste, summonermana, summonerodingarrison, summonerrevive, summonersmite, summonerteleport;
        private static void Main(string[] args)
        {
            Sprite = new Sprite(Drawing.Direct3DDevice);
            HUDult = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.HUDult, typeof(byte[])), 16, 16, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            blackTexture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.schwarz, typeof(byte[])), 62 + 24, 90, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            HUD = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.HUDtest, typeof(byte[])), 62 + 24, 90, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            hpTexture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.HPbar, typeof(byte[])), 58, 10, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            manaTexture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.MANAbar, typeof(byte[])), 58, 10, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            energieTexture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.Energiebar, typeof(byte[])), 58, 10, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            #region summeners
            summonerheal = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerHeal, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerbarrier = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerBarrier, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerboost = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerBoost, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerclairvoyance = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerClairvoyance, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerdot = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerDot, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerflash = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerFlash, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerhaste = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerHaste, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerexhaust = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerExhaust, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonermana = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerMana, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerodingarrison = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerOdinGarrison, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerrevive = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerRevive, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonersmite = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerSmite, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            summonerteleport = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Resources.SummonerTeleport, typeof(byte[])), 24, 480, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);

            #endregion
            small = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new FontDescription()
            {
                FaceName = "Verdana",
                Height = 10,
                OutputPrecision = FontPrecision.Default,
                Quality = FontQuality.Default
            });
            medium = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new FontDescription()
            {
                FaceName = "Verdana",
                Height = 16,
                OutputPrecision = FontPrecision.Default,
                Quality = FontQuality.Default
            });
            respawnfont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new FontDescription()
            {
                FaceName = "Verdana",
                Height = 40,
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
                Drawing.OnPostReset += DrawingOnPostReset;
                Drawing.OnPreReset += DrawingOnPreReset;
                Drawing.OnEndScene += Drawing_OnEndscene;
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
        static void Drawing_OnEndscene(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
                return;
            try
            {
                if (SidebarMenu.Item("Activate").GetValue<bool>()) //drawHUD
                {
                    x = -Width + ((62 + 24) * scale)+100;
                    y = Height * -.10f;
                    int zahler = 0;
                    String timetorespawn;
                    foreach (var enemie in enemyList)
                    {
                        int z = 0;
                        foreach (var sSlot in SummonerSpellSlots)    //Imeh again
                        {
                            var spell = enemie.Hero.Spellbook.GetSpell(sSlot);

                            var t = spell.CooldownExpires - Game.Time;
                            var percent = (Math.Abs(spell.Cooldown) > float.Epsilon) ? t / spell.Cooldown : 1f;
                            var n = (t > 0) ? (int)(19 * (1f - percent)) : 19;
                            var ts = TimeSpan.FromSeconds((int)t);
                            var s = t > 60 ? string.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds) : String.Format("{0:0}", t);
                            if (t > 0)
                            {

                                medium.DrawText(
                                    null, s, Convert.ToInt32(-x - 4 - s.Length * 7), Convert.ToInt32(-y + 7 + z),
                                    new ColorBGRA(255, 255, 255, 255));
                            }



                            Sprite.Begin();
                            switch (spell.Name)
                            {//shitty code but im to tired
                                case "summonerodingarrison":
                                    Sprite.Draw(summonerodingarrison, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerrevive":
                                    Sprite.Draw(summonerrevive, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));

                                    break;
                                case "summonerclairvoyance":
                                    Sprite.Draw(summonerclairvoyance, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerboost":
                                    Sprite.Draw(summonerboost, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonermana":
                                    Sprite.Draw(summonermana, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerteleport":
                                    Sprite.Draw(summonerteleport, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x, y - 7 - z, 0));
                                    break;
                                case "summonerheal":
                                    Sprite.Draw(summonerheal, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerexhaust":
                                    Sprite.Draw(summonerexhaust, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonersmite":
                                    Sprite.Draw(summonersmite, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerdot":
                                    Sprite.Draw(summonerdot, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerhaste":
                                    Sprite.Draw(summonerhaste, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerflash":
                                    Sprite.Draw(summonerflash, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                default:
                                    Sprite.Draw(summonerbarrier, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                            }
                            Sprite.End();
                            z = 24;
                        } //Imeh end 
                        x = x - 23;//fix wege i ha falsch agfange


                        Sprite.Begin(); //DRAW icon 255, 255, 255, 255
                        Sprite.Draw(enemie.Icon, new ColorBGRA(255, 255, 255, 255), null, new Vector3(x - 5, y - 8, 0), null);
                        Sprite.End();

                        if (enemie.Hero.IsDead && respawntime[zahler] < Game.ClockTime)
                        {
                            respawntime[zahler] = Game.ClockTime + enemie.Hero.DeathDuration;
                            //todo get respawn timer
                        }
                        else if (enemie.Hero.IsDead && (respawntime[zahler] > Game.ClockTime))
                        {
                            timetorespawn = (Math.Round(respawntime[zahler] - Game.ClockTime)).ToString();
                            if (timetorespawn.Length == 1)
                            {
                                respawnfont.DrawText(null, timetorespawn, (int)x * -1 + 21, (int)y * -1 + 13, new ColorBGRA(248, 248, 255, 255));
                            }
                            else
                            {
                                respawnfont.DrawText(null, timetorespawn, (int)x * -1 + 10, (int)y * -1 + 13, new ColorBGRA(248, 248, 255, 255));
                            }
                        }


                        String HP = Math.Round(enemie.Hero.Health) + "/" + Math.Round(enemie.Hero.MaxHealth);

                        int hplength = ((58 - (HP.Length * 5)) / 2); //to center text
                        hpwidth = Convert.ToInt32(((58f / 100f) * (enemie.Hero.HealthPercentage())));
                        Sprite.Begin(); //DRAW HUD
                        // //ziel:-1617 / -124 //bild 1 -4/-26 55x55
                        x = x + 23;//fix wege i ha falsch agfange
                        Sprite.Draw(HUD, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(1, 0, 62 + 23, 90), new Vector3(x, y, 0), null); //todo add % value for heigh 
                        x = x - 23;//fix wege i ha falsch agfange
                        Sprite.End();
                        // //draw level  weiss =    248-248-255
                        small.DrawText(null, enemie.Hero.Level.ToString(), (int)x * -1 + 48, (int)y * -1 + 52, new ColorBGRA(248, 248, 255, 255));


                        if (enemie.Hero.Spellbook.GetSpell(SpellSlot.R).CooldownExpires < Game.Time && enemie.Hero.Spellbook.GetSpell(SpellSlot.R).Level > 0)
                        {
                            Sprite.Begin();
                            Sprite.Draw(HUDult, new ColorBGRA(255, 255, 255, 255), null, new Vector3(x + -46, y + -2, 0), null);
                            Sprite.End();

                        }
                        if (!NoEnergie.Contains(enemie.Hero.ChampionName))
                        {
                            String Mana = Math.Round(enemie.Hero.Mana) + "/" + Math.Round(enemie.Hero.MaxMana);
                            int manawidth = Convert.ToInt32(((58f / 100f) * (enemie.Hero.ManaPercentage())));
                            int Manalength = ((58 - (Mana.Length * 5)) / 2); //to center t
                            //draw MANA /Manabar
                            Sprite.Begin();
                            if (!Energie.Contains(enemie.Hero.ChampionName))
                            {
                                Sprite.Draw(manaTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, manawidth, 10), new Vector3(x - 2, y - 57 - 7 - 14, 0), null);
                            }
                            else
                            {
                                Sprite.Draw(energieTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, manawidth, 10), new Vector3(x - 2, y - 57 - 7 - 14, 0),
                                    null);
                            }
                            Sprite.End();
                            small.DrawText(null, Mana, (int)x * -1 + 2 + Manalength, (int)y * -1 + 65 + 14,
                                new ColorBGRA(248, 248, 255, 255));
                        }
                        else
                        {

                        }

                        //draw HP/MAXHP 

                        Sprite.Begin();
                        Sprite.Draw(hpTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, hpwidth, 10), new Vector3(x - 2, y - 57 - 7, 0), null);
                        Sprite.End();
                        small.DrawText(null, HP, (int)x * -1 + 2 + hplength, (int)y * -1 + 65, new ColorBGRA(248, 248, 255, 255));
                        if (!enemie.Hero.IsVisible || enemie.Hero.IsDead)//make it black :)
                        {
                            Sprite.Begin(); //DRAW icon 255, 255, 255, 255
                            Sprite.Draw(blackTexture, new ColorBGRA(255, 255, 255, 110), null, new Vector3(x + 24, y, 0), null);
                            Sprite.End();
                        }

                        //if (enemie.Hero.Health < 350 && ((int)hero.Spellbook.GetSpell(SpellSlot.R).SData.CastRange.GetValue(0)) < 5000 && enemie.Hero.ServerPosition.Distance(hero.ServerPosition) < (int)hero.Spellbook.GetSpell(SpellSlot.R).SData.CastRange.GetValue(0) && hero.Spellbook.GetSpell(SpellSlot.R).Cooldown.Equals(0)&&hero.Level>=6)
                        //{
                        //  Print("mach ult");
                        //  //todo ping on  enemie.Hero.ServerPosition
                        //  {
                        //      Packet.S2C.Ping.Encoded(new Packet.S2C.Ping.Struct(enemie.Hero.Position.X, enemie.Hero.Position.Y,
                        //      enemie.Hero.NetworkId,
                        //      ObjectManager.Player.NetworkId, Packet.PingType.Danger)).Process();
                        //  }

                        //}
                        //todo ping on  enemie.Hero.ServerPosition


                        x = x + 23;
                        y = y - 94;
                        zahler++;

                    }



                }
            }
            catch
            {

            }
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
                return;
            try
            {
                if (SidebarMenu.Item("Activate").GetValue<bool>()) //drawHUD
                {
                    x = -Width + ((62 + 24) * scale);
                    y = Height * -.10f;
                    int zahler = 0;
                    String timetorespawn;
                    foreach (var enemie in enemyList)
                    {
                        int z = 0;
                        foreach (var sSlot in SummonerSpellSlots)    //Imeh again
                        {
                            var spell = enemie.Hero.Spellbook.GetSpell(sSlot);

                            var t = spell.CooldownExpires - Game.Time;
                            var percent = (Math.Abs(spell.Cooldown) > float.Epsilon) ? t / spell.Cooldown : 1f;
                           var n = (t > 0) ? (int)(19 * (1f - percent)) : 19;
                             var ts = TimeSpan.FromSeconds((int)t);
                             var s = t > 60 ? string.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds) : String.Format("{0:0}", t);
                        if (t > 0)
                            {
                               
                                medium.DrawText(
                                    null, s, Convert.ToInt32(-x - 4 - s.Length * 7), Convert.ToInt32(-y + 7 + z),
                                    new ColorBGRA(255, 255, 255, 255));
                            }



                            Sprite.Begin();
                            switch (spell.Name)
                            {//shitty code but im to tired
                                case "summonerodingarrison":
                                    Sprite.Draw(summonerodingarrison, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerrevive":
                                    Sprite.Draw(summonerrevive, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));

                                    break;
                                case "summonerclairvoyance":
                                    Sprite.Draw(summonerclairvoyance, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerboost":
                                    Sprite.Draw(summonerboost, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonermana":
                                    Sprite.Draw(summonermana, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerteleport":
                                    Sprite.Draw(summonerteleport, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x, y - 7 - z, 0));
                                    break;
                                case "summonerheal":
                                    Sprite.Draw(summonerheal, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerexhaust":
                                    Sprite.Draw(summonerexhaust, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonersmite":
                                    Sprite.Draw(summonersmite, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerdot":
                                    Sprite.Draw(summonerdot, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerhaste":
                                    Sprite.Draw(summonerhaste, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                case "summonerflash":
                                    Sprite.Draw(summonerflash, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                                default:
                                    Sprite.Draw(summonerbarrier, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 24 * n, 24, 24), new Vector3(x - 2, y - 7 - z, 0));
                                    break;
                            }
                            Sprite.End();
                            z = 24;
                        } //Imeh end 
                        x = x - 23;//fix wege i ha falsch agfange


                        Sprite.Begin(); //DRAW icon 255, 255, 255, 255
                        Sprite.Draw(enemie.Icon, new ColorBGRA(255, 255, 255, 255), null, new Vector3(x - 5, y - 8, 0), null);
                        Sprite.End();

                        if (enemie.Hero.IsDead && respawntime[zahler] < Game.ClockTime)
                        {
                            respawntime[zahler] = Game.ClockTime + enemie.Hero.DeathDuration;
                            //todo get respawn timer
                        }
                        else if (enemie.Hero.IsDead && (respawntime[zahler] > Game.ClockTime))
                        {
                            timetorespawn = (Math.Round(respawntime[zahler] - Game.ClockTime)).ToString();
                            if (timetorespawn.Length == 1)
                            {
                                respawnfont.DrawText(null, timetorespawn,(int)x * -1 + 21,(int)y * -1 + 13,new ColorBGRA(248, 248, 255, 255));
                            }
                            else
                            {
                                respawnfont.DrawText(null, timetorespawn,(int)x * -1 + 10,(int)y * -1 + 13,new ColorBGRA(248, 248, 255, 255));
                            }
                        }


                        String HP = Math.Round(enemie.Hero.Health) + "/" + Math.Round(enemie.Hero.MaxHealth);

                        int hplength = ((58 - (HP.Length * 5)) / 2); //to center text
                        hpwidth = Convert.ToInt32(((58f / 100f) * (enemie.Hero.HealthPercentage())));
                        Sprite.Begin(); //DRAW HUD
                        // //ziel:-1617 / -124 //bild 1 -4/-26 55x55
                        x = x + 23;//fix wege i ha falsch agfange
                        Sprite.Draw(HUD, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(1, 0, 62 + 23, 90), new Vector3(x, y, 0), null); //todo add % value for heigh 
                        x = x - 23;//fix wege i ha falsch agfange
                        Sprite.End();
                        // //draw level  weiss =    248-248-255
                        small.DrawText(null, enemie.Hero.Level.ToString(), (int)x * -1 + 48, (int)y * -1 + 52, new ColorBGRA(248, 248, 255, 255));


                        if (enemie.Hero.Spellbook.GetSpell(SpellSlot.R).CooldownExpires < Game.Time && enemie.Hero.Spellbook.GetSpell(SpellSlot.R).Level > 0)
                        {
                            Sprite.Begin();
                            Sprite.Draw(HUDult, new ColorBGRA(255, 255, 255, 255), null, new Vector3(x + -46, y + -2, 0), null);
                            Sprite.End();

                        }
                        if (!NoEnergie.Contains(enemie.Hero.ChampionName))
                        {
                            String Mana = Math.Round(enemie.Hero.Mana) + "/" + Math.Round(enemie.Hero.MaxMana);
                            int manawidth = Convert.ToInt32(((58f / 100f) * (enemie.Hero.ManaPercentage())));
                            int Manalength = ((58 - (Mana.Length * 5)) / 2); //to center t
                            //draw MANA /Manabar
                            Sprite.Begin();
                            if (!Energie.Contains(enemie.Hero.ChampionName))
                            {
                                Sprite.Draw(manaTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, manawidth, 10), new Vector3(x - 2, y - 57 - 7 - 14, 0), null);
                            }
                            else
                            {
                                Sprite.Draw(energieTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, manawidth, 10), new Vector3(x - 2, y - 57 - 7 - 14, 0),
                                    null);
                            }
                            Sprite.End();
                            small.DrawText(null, Mana, (int)x * -1 + 2 + Manalength, (int)y * -1 + 65 + 14,
                                new ColorBGRA(248, 248, 255, 255));
                        }
                        else
                        {

                        }

                        //draw HP/MAXHP 

                        Sprite.Begin();
                        Sprite.Draw(hpTexture, new ColorBGRA(255, 255, 255, 255), new SharpDX.Rectangle(0, 0, hpwidth, 10), new Vector3(x - 2, y - 57 - 7, 0), null);
                        Sprite.End();
                        small.DrawText(null, HP, (int)x * -1 + 2 + hplength, (int)y * -1 + 65, new ColorBGRA(248, 248, 255, 255));
                        if (!enemie.Hero.IsVisible || enemie.Hero.IsDead)//make it black :)
                        {
                            Sprite.Begin(); //DRAW icon 255, 255, 255, 255
                            Sprite.Draw(blackTexture, new ColorBGRA(255, 255, 255, 110), null, new Vector3(x+24, y, 0), null);
                            Sprite.End();
                        }

                        //if (enemie.Hero.Health < 350 && ((int)hero.Spellbook.GetSpell(SpellSlot.R).SData.CastRange.GetValue(0)) < 5000 && enemie.Hero.ServerPosition.Distance(hero.ServerPosition) < (int)hero.Spellbook.GetSpell(SpellSlot.R).SData.CastRange.GetValue(0) && hero.Spellbook.GetSpell(SpellSlot.R).Cooldown.Equals(0)&&hero.Level>=6)
                        //{
                        //  Print("mach ult");
                        //  //todo ping on  enemie.Hero.ServerPosition
                        //  {
                        //      Packet.S2C.Ping.Encoded(new Packet.S2C.Ping.Struct(enemie.Hero.Position.X, enemie.Hero.Position.Y,
                        //      enemie.Hero.NetworkId,
                        //      ObjectManager.Player.NetworkId, Packet.PingType.Danger)).Process();
                        //  }
                         
                        //}
                        //todo ping on  enemie.Hero.ServerPosition

                       
                        x = x + 23;
                        y = y - 94;
                        zahler++;

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
            }
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


