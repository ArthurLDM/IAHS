using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using HearthDb.Enums;
using System.Threading;

using WindowsPoint = System.Windows.Point;
using System.Windows.Forms;
using DrawingPoint = System.Drawing.Point;
using System.Drawing;


namespace Warchief
{
    class IAHS
    {
        static int TurnNum;
        static Player FirstPlayer;
        static Player CurrentPlayer;
        internal Advisor A = new Advisor();
        internal int mana = 0; // le mana représente le nombre de cristaux de mana en plus que ceux que le joueur devrait avoir
        // permet de gerer coin, innervation, croissance sauvage, nourrish ...

        internal List<BoardRegionNavigation> regions;

        internal IAHS()
            {
            regions = new List<BoardRegionNavigation>{
                     new OpponentNavigator(),
                     new MinionNavigator(false),
                     new MinionNavigator(true),
                     new HeroNavigator(),
                     new HandNavigator() };

            }

        internal void GameStart()
        {
            TurnNum = 0;
            A = new Advisor();
            A.Show();
        }

        internal void TurnStart()
        {
            if (TurnNum == 0)
            {
                SetFirstPlayer();
                CurrentPlayer = FirstPlayer;
                TurnNum = 1;
                mana = TurnNum;

            }
            else
            {
                SetCurrentPlayer();
                if (CurrentPlayer == FirstPlayer)
                {
                    TurnNum++;
                    mana=TurnNum;
                }
            }

            List<Entity> OppBoard = CoreAPI.Game.Opponent.Board.Where(x => x.IsMinion).OrderBy(x => x.GetTag(GameTag.ZONE_POSITION)).ToList();
            List<int> Targets = FindTarget(OppBoard);

            string name = "";
            if (Targets.Count == 0)
                name += CoreAPI.Game.Opponent.Name;
            else
            {
                foreach (int i in Targets)
                    name += OppBoard[i].LocalizedName + " - ";
            }

            //A.SetLabel(name);

            if (CurrentPlayer == CoreAPI.Game.Player)
            {
                GameData();
                OnCurveMinon();
            }
        }

        internal void SetFirstPlayer()
        {
            if (CoreAPI.Game.Player.HasCoin)
                FirstPlayer = CoreAPI.Game.Opponent;
            if (CoreAPI.Game.Opponent.HasCoin)
                FirstPlayer = CoreAPI.Game.Player;

        }

        internal void SetCurrentPlayer()
        {
            if (CurrentPlayer == CoreAPI.Game.Player)
                CurrentPlayer = CoreAPI.Game.Opponent;
            else
                CurrentPlayer = CoreAPI.Game.Player;
        }

        internal List<int> FindTarget(IEnumerable<Entity> oppBoard)
        {
            List<int> TauntPos = new List<int>();
            int i = 0;
            foreach(var e in oppBoard)
            {
                if (e.IsInPlay)
                {
                    if (e.GetTag(GameTag.TAUNT) == 1)
                        TauntPos.Add(i);
                    i++;
                }
            }
            return (TauntPos);
        }

        internal void GameEnd()
        {
            A.Hide();
        }

        internal bool PlayableCard()
        {
            GameData();

            if (Hand.Count() == 0)
                return (false);
            else
            {
                int min = 100;
                foreach (Entity Card in Hand)
                {
                    if (Card.Cost < min && Card.Cost<mana && Card.IsMinion)
                        min = Card.Cost;
                }
                return (min!=100);
            }
        }

        internal List<int> PlayableCards()
        {
            List<int> Cards = new List<int>();
            int index = 0;

            foreach (Entity Card in Hand)
            {
                if (Card.Cost < mana)
                    Cards.Add(index);
                index++;
            }

            return (Cards);

        }

        int BoardSize = CoreAPI.Game.Player.Board.Count();
        IEnumerable<Entity> Hand = CoreAPI.Game.Player.Hand;

        internal void GameData()
        {
            BoardSize = CoreAPI.Game.Player.Board.Count();
            Hand = CoreAPI.Game.Player.Hand;
            //A.SetLabel(Hand.Count().ToString());

        }


        internal void OnCurveMinon()
        {


            while (BoardSize < 7 && PlayableCards().Count() != 0)
            {
                int index = 0;
                GameData();
                int cost = Hand.ToList()[index].Cost;



                /*
                foreach (Entity Card in Hand)
                {
                    if (Card.Cost <= TurnNum && Card.IsMinion)
                    {
                        PosInHand = index;
                        cost = Card.Cost;
                    }

                    index++;
                }
                */
                //if (PosInHand != -1)
                PlayMinion(PlayableCards()[index], cost);
                index++;
            }

            if (mana>1)
                HeroPower();

            EndTurn();

        }

        internal void HeroPower()
        {
            mana -= 2;

            Thread.Sleep(10000);
            HeroNavigator HeroPower = (HeroNavigator)regions[3];
            WindowsPoint Position = HeroPower.playerHeroPowerLocation;
            Cursor.Position = getAbsolutePos(Position);
            click();
        }

        internal void EndTurn()
        {
            Thread.Sleep(5000);
            MinionNavigator EndTurn = (MinionNavigator)regions[1];
            WindowsPoint Position = EndTurn.endTurnLocation;
            Cursor.Position = getAbsolutePos(Position);
            click();
        }

        internal void PlayMinion(int PosInHand, int cost)
        {

            mana -= cost;
            Thread.Sleep(3000);
            GameData();
            HandNavigator hand = (HandNavigator)regions[4];

            WindowsPoint Position = hand.handLocations[Hand.Count()][PosInHand];
            Cursor.Position = getAbsolutePos(Position);
            click();

            Thread.Sleep(3000);
            MinionNavigator Board = (MinionNavigator)regions[2];
            Position = Board.playerMinionRowLocation;
            Cursor.Position = getAbsolutePos(Position);
            click();
        }


#region Navigation

        /* `ALGALON` coordinate system for hearthstone */

        /* define the center of board as (0,0). */
        /* define one unit as one percent of the distance from the center of the board to the top of the screen */
        /* (precision is approximately three digits) */

        /* top of screen is (0,100) */
        /* left side of board is (-133,0).  the board is roughly 4:3 shape. */
        /* in 16:9 displays, the padding around the board extends to (177,0) */

        //TODO: replace most references to `WindowsPoint` with `AlgalonPoint`

        static double GLOBAL_SCALE = 100.0;
        static double EMPIRICAL_X_OFFSET = 0;
        static double EMPIRICAL_Y_OFFSET = -0.07 * GLOBAL_SCALE;

        //get an algalon coordinate from a game client-relative coordinate
        private WindowsPoint getLocalPos(WindowsPoint clientPos)
        {
            Rectangle wrecked = User32.GetHearthstoneRect(false);
            double yCenter = wrecked.Height / 2;
            double xCenter = wrecked.Width / 2;
            double scale = GLOBAL_SCALE / yCenter;

            double xOffset = EMPIRICAL_X_OFFSET;
            double yOffset = EMPIRICAL_Y_OFFSET;

            WindowsPoint local = new WindowsPoint(
                (clientPos.X - xCenter) * scale + xOffset,
                (clientPos.Y - yCenter) * scale * -1 + yOffset);  //invert y axis (up should be positive, dammit)

            return local;
        }

        //get a client-relative coordinate from an algalon coordinate
        private DrawingPoint getWindowPos(WindowsPoint boardPos)
        {
            Rectangle wrecked = User32.GetHearthstoneRect(false);
            double yCenter = wrecked.Height / 2;
            double xCenter = wrecked.Width / 2;
            double scale = GLOBAL_SCALE / yCenter;

            double xOffset = EMPIRICAL_X_OFFSET;
            double yOffset = EMPIRICAL_Y_OFFSET;

            DrawingPoint windowPos = new DrawingPoint(
                (int)((boardPos.X - xOffset) / scale + xCenter),
                (int)((boardPos.Y - yOffset) * -1 / scale + yCenter)
                );

            return windowPos;
        }

        //get a windows absolute coordinate from an algalon coordinate
        private DrawingPoint getAbsolutePos(WindowsPoint boardPos)
        {
            DrawingPoint position = getWindowPos(boardPos);
            User32.ClientToScreen(User32.GetHearthstoneWindow(), ref position);
            return position;
        }

        /*
        private WindowsPoint getCanvasPos(DrawingPoint absolutePos)
        {
            Rectangle wrecked = User32.GetHearthstoneRect(false);

            double y = absolutePos.Y - wrecked.Top;
            double x = absolutePos.X - wrecked.Left;

            return new WindowsPoint(x, y);
        }
        */

        private const int CLICK_SLEEP_TIME_MS = 30;
        private const int CLICK_SLEEP_TIME_AFTER_MS = 30;

        private void click()
        {
            User32.mouse_event((uint)User32.MouseEventFlags.LeftDown, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(CLICK_SLEEP_TIME_MS);
            User32.mouse_event((uint)User32.MouseEventFlags.LeftUp, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(CLICK_SLEEP_TIME_AFTER_MS);
        }

        private void rightClick()
        {
            User32.mouse_event((uint)User32.MouseEventFlags.RightDown, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(CLICK_SLEEP_TIME_MS);
            User32.mouse_event((uint)User32.MouseEventFlags.RightUp, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(CLICK_SLEEP_TIME_AFTER_MS);
        }

        public void SwitchTo()
        {
            return;
        }
#endregion
    }
}
