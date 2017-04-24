using Hearthstone_Deck_Tracker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingPoint = System.Drawing.Point;
using WindowsPoint = System.Windows.Point;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;


namespace Warchief
{
    class TargetingDummy : CommandModule
    {
        //TODO: this would be a bit cleaner if it was a doubly-linked list instead of array/idx
        internal List<BoardRegionNavigation> regions;
        int currentRegionIndex;        

        public TargetingDummy()
        {
            regions = new List<BoardRegionNavigation>();
            //add a discover, choose one and mulligan region 
            regions.Add(new OpponentNavigator());
            regions.Add(new MinionNavigator(false));
            regions.Add(new MinionNavigator(true));
            regions.Add(new HeroNavigator());
            regions.Add(new HandNavigator());
            setRegion(3);

        }

        public CommandModule Command(InputCommand input)
        {
            switch (input)
            {
                case InputCommand.Up:
                    setRegion(currentRegionIndex - 1);
                    break;
                case InputCommand.Down:
                    setRegion(currentRegionIndex + 1);
                    break;
                case InputCommand.Left:
                case InputCommand.Right:
                    navigate(input);
                    break;
                case InputCommand.Select:
                    return Select();
                case InputCommand.Unselect:
                    return Unselect();
            }
            return this;
        }

        private static WindowsPoint playerHandLocation = new WindowsPoint(0, -90);

        public bool setRegion(int region)
        {
            if (region < 0 || region >= regions.Count)
                return false;


            currentRegionIndex = region;
            WindowsPoint location = regions[currentRegionIndex].SwitchTo();

            Cursor.Position = getAbsolutePos(location);
            return true;
        }

        private void navigate(InputCommand direction)
        {
            WindowsPoint location = regions[currentRegionIndex].Navigate(direction == InputCommand.Right);
            Cursor.Position = getAbsolutePos(location);
            return;
        }

        private CommandModule Select()
        {
            //TODO kiil urself
            MinionNavigator.HUGE_HACK = -1;

            CommandModule next = regions[currentRegionIndex].Select(this);
            click();
            next.SwitchTo();
            return next;
        }

        private CommandModule Unselect()
        {
            //TODO kiil urself
            MinionNavigator.HUGE_HACK = -1;

            CommandModule next = regions[currentRegionIndex].Unselect(this);
            rightClick();
            next.SwitchTo();
            return next;
        }

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
    }
}
