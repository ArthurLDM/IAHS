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

        internal DebugWindow A = new DebugWindow();
        internal int mana = 0; // le mana représente le nombre de cristaux de mana en plus que ceux que le joueur devrait avoir
                               // permet de gerer coin, innervation, croissance sauvage, nourrish ...

        internal List<Entity> Entities =>
        Helper.DeepClone<Dictionary<int, Entity>>(CoreAPI.Game.Entities).Values.ToList<Entity>();

        internal Entity Opponent => Entities?.FirstOrDefault(x => x.IsOpponent);
        internal Entity Player => Entities?.FirstOrDefault(x => x.IsPlayer);

        internal Turn turn;

        internal List<BoardRegionNavigation> regions;


        internal IAHS()
        {
            regions = new List<BoardRegionNavigation>{
                     new OpponentNavigator(),
                     new MinionNavigator(false),
                     new MinionNavigator(true),
                     new HeroNavigator(),
                     new HandNavigator() };
            turn = new Turn();
        }

        internal void GameStart()
        {
            A = new DebugWindow();
            A.Show();
            turn = new Turn();

    }

    internal void TurnStart(ActivePlayer player)
        {
                turn = new Turn();
                turn.MakeTurn();
        }

        

        internal void GameEnd()
        {
            //A.Hide();
        }

        internal void ExecuteTurn(Turn turn)
        {
            int index = 0;

            foreach (List<WindowsPoint> action in turn.Actions)
            {
                foreach (WindowsPoint Position in action)
                {
                    Thread.Sleep(1000);
                    Cursor.Position = getAbsolutePos(Position);
                    Thread.Sleep(1000);
                    click();
                }
                turn.Actions.RemoveAt(index);
                index++;
            }
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


    //    class IAHS
    //    {

    //        internal DebugWindow A = new DebugWindow();

    //        internal List<BoardRegionNavigation> regions;


    //        internal List<Entity> Entities =>
    //    Helper.DeepClone<Dictionary<int, Entity>>(CoreAPI.Game.Entities).Values.ToList<Entity>();

    //        internal Entity Opponent => Entities?.FirstOrDefault(x => x.IsOpponent);
    //        internal Entity Player => Entities?.FirstOrDefault(x => x.IsPlayer);

    //        internal Turn turn;

    //        internal IAHS()
    //            {
    //            regions = new List<BoardRegionNavigation>{
    //                     new OpponentNavigator(),
    //                     new MinionNavigator(false),
    //                     new MinionNavigator(true),
    //                     new HeroNavigator(),
    //                     new HandNavigator() };
    //            turn = new Turn();
    //            }

    //        internal void GameStart()
    //        {
    //            A = new DebugWindow();
    //            A.Show();

    //            HandNavigator handnavigator = new HandNavigator();
    //            WindowsPoint position =handnavigator.handLocations[6][4];
    //        }


    //        internal void TurnStart(ActivePlayer player)
    //        {

    //            if (player == ActivePlayer.Player && Opponent != null)
    //            {
    //                List<Entity> OppBoard = CoreAPI.Game.Opponent.Board.Where(x => x.IsMinion).OrderBy(x => x.GetTag(GameTag.ZONE_POSITION)).ToList();
    //                List<int> Targets = FindTarget(OppBoard);

    //                string name = "";
    //                if (Targets.Count == 0)
    //                    name += CoreAPI.Game.Opponent.Name;
    //                else
    //                {
    //                    foreach (int i in Targets)
    //                        name += OppBoard[i].LocalizedName + " - ";
    //                }

    //                turn = new Turn();
    //                turn.MakeTurn();
    //            }

    //        }

    //        internal void ExecuteTurn(Turn turn)
    //        {
    //            int index = 0;

    //            foreach (List<WindowsPoint> action in turn.Action)
    //            {
    //                foreach (WindowsPoint Position in action)
    //                {
    //                    Thread.Sleep(4000);
    //                    Cursor.Position = getAbsolutePos(Position);
    //                    Thread.Sleep(1000);
    //                    click();
    //                }
    //                turn.Action.RemoveAt(index);
    //                index++;
    //            }
    //        }

    //        internal List<int> FindTarget(IEnumerable<Entity> oppBoard)
    //        {
    //            List<int> TauntPos = new List<int>();
    //            int i = 0;
    //            foreach(var e in oppBoard)
    //            {
    //                if (e.IsInPlay)
    //                {
    //                    if (e.GetTag(GameTag.TAUNT) == 1)
    //                        TauntPos.Add(i);
    //                    i++;
    //                }
    //            }
    //            return (TauntPos);
    //        }

    //        internal void GameEnd()
    //        {
    //            A.Hide();
    //            turn = new Turn();
    //        }

   
    //    }
}
