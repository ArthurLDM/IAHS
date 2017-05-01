using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsPoint = System.Windows.Point;
using System.Threading;

using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using Hearthstone_Deck_Tracker.Utility.HotKeys;

namespace Warchief
{
    public class Warchief : IPlugin
    {


        public string Name => "IAHS & Warchief";
        public string Description
            =>
                "\n\nUse your keyboard or gamepad to play Hearthstone. && IAHS tests\n\n\n";

        public string ButtonText => "DO NOT PUSH THIS BUTTON!";
        public string Author => "realchriscasey && Thur";
        public Version Version => new Version(0, 0, 1);
        public System.Windows.Controls.MenuItem MenuItem => null;


        internal Mode Choix = new Mode();

        private bool gameStarted = false;
        private bool mulliganDone = false;
        IAHS iahs = new IAHS();

        void IPlugin.OnButtonPress()
        {
            /*NOP*/
        }

        void IPlugin.OnUnload()
        {
            /*NOP*/
        }

        void IPlugin.OnLoad()
        {
            #region Add KeyCommand
            if (keyCommands.Count == 0)
            {
                // TODO it's probably possible to use delegates instead of simple function refs here
                keyCommands.Add(new HotKey(Keys.Up), this.Up);
                keyCommands.Add(new HotKey(Keys.Down), this.Down);
                keyCommands.Add(new HotKey(Keys.Left), this.Left);
                keyCommands.Add(new HotKey(Keys.Right), this.Right);

                keyCommands.Add(new HotKey(Keys.Z), this.Select);
                keyCommands.Add(new HotKey(Keys.X), this.Unselect);
            }

            #endregion

            mulliganDone = CoreAPI.Game.IsMulliganDone;
            gameStarted = mulliganDone && !CoreAPI.Game.IsInMenu;

            GameEvents.OnGameStart.Add(() => 
            {
                gameStarted = true;
                GameStart();
                iahs.GameStart();
            });

            GameEvents.OnTurnStart.Add(a => TurnStart());
            GameEvents.OnTurnStart.Add(iahs.TurnStart);
            


            GameEvents.OnGameEnd.Add(() =>
            {
                gameStarted = false;
                iahs.GameEnd();
            });

            //Cas ou on ouvre HDT dans une game lancée
            if (!mulliganDone)
                iahs.GameEnd();
            else
                GameStart();
        }

        void TurnStart()
        {
            TargetingDummy T =(TargetingDummy)currentModule;
            iahs.regions = T.regions;
        }
        

        Action GameStart()
        {
            if (!mulliganDone)
                currentModule = new MulliganCommand();
            else
                currentModule = new TargetingDummy();
            return (null);
        }

        private bool areHotkeysRegistered = false;
        private Dictionary<HotKey, Action> keyCommands = new Dictionary<HotKey, Action>();
        private CommandModule currentModule;
       

        void IPlugin.OnUpdate()
        {
            #region HotKeys
            if (!this.areHotkeysRegistered && User32.IsHearthstoneInForeground())
            {
                foreach(KeyValuePair<HotKey, Action> keyCommand in this.keyCommands)
                {
                    HotKeyManager.RegisterHotkey(keyCommand.Key, keyCommand.Value, keyCommand.Key.Key.ToString());
                }

                this.areHotkeysRegistered = true;
            }
            else if (this.areHotkeysRegistered && !User32.IsHearthstoneInForeground())
            {
                //hotkeys are registered, but hearthstone isn't active. unregister them.
                
                foreach (KeyValuePair<HotKey, Action> keyCommand in this.keyCommands)
                {
                    HotKeyManager.RemovePredefinedHotkey(keyCommand.Key);
                }

                this.areHotkeysRegistered = false;
            }
            #endregion

            if (!Choix.Visible)
                Choix.Show();
            if (gameStarted)
            {
                if (!iahs.A.Visible)
                    iahs.A.Show();
                //Ici, on met a jour le CardNumber et les cardPos 
                //Car Player.HasCoin=false puis est mis a jour 
                if (currentModule.GetType() == typeof(MulliganCommand))
                {
                    MulliganCommand I = (MulliganCommand)currentModule;

                    I.getCardNumber();
                    I.CreateCardList();
                    currentModule = (CommandModule)I;
                }

                if (currentModule.GetType() == typeof(TargetingDummy))
                {
                    TargetingDummy I = (TargetingDummy)currentModule;
                    iahs.regions = I.regions;
                }


                // Checking currentmodule
                //Si on ouvre HDT pendant une game
                if (mulliganDone && currentModule.GetType() != typeof(TargetingDummy))
                    currentModule = new TargetingDummy();

                if (!mulliganDone && currentModule.GetType() != typeof(MulliganCommand))
                    currentModule = new MulliganCommand();


                if (!mulliganDone && CoreAPI.Game.IsMulliganDone)
                    GameStart();

                mulliganDone = CoreAPI.Game.IsMulliganDone;

                iahs.A.SetLabel(iahs.turn.Debug());
                if (iahs.turn.Actions.Count != 0)
                    if (Choix.BotRB_Checked()) // si est pas endturn && on a active le bot mode
                    {
                        Thread.Sleep(5000);
                        iahs.ExecuteTurn(iahs.turn);
                    }
                iahs.A.SetLabel(iahs.turn.Debug());
            }
        }

        private void SendCommand(InputCommand input)
        {
            CommandModule result = this.currentModule.Command(input);

            if (result != null)
            {
                this.currentModule = result;
            }
        }

        private void Up() { SendCommand(InputCommand.Up); }
        private void Down() { SendCommand(InputCommand.Down); }
        private void Left() { SendCommand(InputCommand.Left); }
        private void Right() { SendCommand(InputCommand.Right); }
        private void Select() { SendCommand(InputCommand.Select); }
        private void Unselect() { SendCommand(InputCommand.Unselect); }
    }
}