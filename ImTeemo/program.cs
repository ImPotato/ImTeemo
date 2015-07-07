using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using LeagueSharp;
using SharpDX;
using LeagueSharp.Common;


namespace ImTeemo
{
    internal class Program
    {
        public static Menu Menu { get; set; }

        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }

        }



        private static Orbwalking.Orbwalker Orbwalker;

        private static Spell Q, W, E, R;

        private static int Combo;

        private static int Mixed;

        private static int LaneClear;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Teemo")
                return;

            Q=new Spell(SpellSlot.Q, 680);
            W=new Spell(SpellSlot.W);
            E=new Spell(SpellSlot.E, 500);
            R=new Spell(SpellSlot.R, 230);



            Menu = new Menu("ImTeemo", "ImTeemo", true);

            Menu orbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);

            Menu ts = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(ts);

            Menu comboMenu = Menu.AddSubMenu(new Menu("Combo", "Combo"));

            comboMenu.AddItem(new MenuItem("useQCombo", "Use Q in combo").SetValue(true));

            comboMenu.AddItem(new MenuItem("useE", "Use E in combo (Not working D:)").SetValue(true));

            comboMenu.AddItem(new MenuItem("useW", "Use W in combo").SetValue(true));

            comboMenu.AddItem(new MenuItem("Combo", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

          
            Menu mixedMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));

            mixedMenu.AddItem(new MenuItem("useQHarass", "Use Q in harass").SetValue(true));

            mixedMenu.AddItem(new MenuItem("Harass", "Harass").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

          
            Menu fleeMenu = Menu.AddSubMenu(new Menu("Flee", "Flee"));

            fleeMenu.AddItem(new MenuItem("useW", "Use W in flee").SetValue(true));

            fleeMenu.AddItem(new MenuItem("useR", "Use R in flee").SetValue(true));

            fleeMenu.AddItem(new MenuItem("Flee", "Flee").SetValue(new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));


            Menu LaneClearMenu = Menu.AddSubMenu(new Menu("Lane Clear", "LaneClear"));

            LaneClearMenu.AddItem(new MenuItem("LaneClearUseQ", "Use Q in Lane Clear (Not working yet)").SetValue(true));

            LaneClearMenu.AddItem(new MenuItem("LaneClearUseR", "Use R in Lane Clear").SetValue(true));

            LaneClearMenu.AddItem(new MenuItem("LaneClear", "LaneClear").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));


            Menu PotatoMenu = Menu.AddSubMenu(new Menu("Potato", "Potato"));

            PotatoMenu.AddItem(new MenuItem("It does nothing, just potato", "It does nothing, just potato").SetValue(true));
          

            Menu.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;

            Game.OnUpdate += Game_OnGameUpdate;



        }


        private static void Flee()
        {
            if (!Menu.Item("useW").GetValue<bool>())
                return;

            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            if (W.IsReady())
            {
                W.Cast(Player);
            }



            if (!Menu.Item("useR").GetValue<bool>())
                return;
            if (R.IsReady())
            {
                R.Cast(Player.Position);
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
                return;

           if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                BlindingDart();
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                BlindingDart();
                ScoutRun();
            }

            if (Menu.Item("Flee").GetValue<KeyBind>().Active)
            {
                Flee();
            }


        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;

            if (Q.IsReady())
            {
                Utility.DrawCircle(Player.Position, 680, Color.LightSeaGreen);
            }
            else
            {
                Utility.DrawCircle(Player.Position, 680, Color.Black);
            }
        }

        private static void BlindingDart()
        {
            if (!Menu.Item("useQCombo").GetValue<bool>())
                return;

            Obj_AI_Hero target = TargetSelector.GetTarget(680, TargetSelector.DamageType.Magical);

            if (Q.IsReady())
            {
                if (target.IsValidTarget(Q.Range))
                {
                    Q.CastOnUnit(target);
                }
            }
        }

        private static void ScoutRun()
        {
            if (!Menu.Item("useW").GetValue<bool>())
                return;

            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            if (W.IsReady())
            {
                W.Cast(Player);
            }
        }
    }


}



//Thanks to Frosty, MasterGF and Golden gates for his skeleton template. Learned a lot of your codes guys
//LaneClear and combo AA WIP.
