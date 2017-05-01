using Hearthstone_Deck_Tracker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DrawingPoint = System.Drawing.Point;
using WindowsPoint = System.Windows.Point;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace Warchief
{
    internal class MulliganCommand : CommandModule
    {

        internal int CardNumber;
        int currentCardIndex = 1; //Permet de savoir quelle carte le joueur est entrain de consulter 
        private static WindowsPoint FirstCardLocation =new WindowsPoint(0, -65); //place le curseur sur le bouton confirm lors du déubt du mulligan.
        internal List<WindowsPoint> CardPos; //contient la liste de la position des cartes proposée au joueur (3 cartes si le joueur commence, 4 sinon) 

        bool Down = false; // permet de savoir si le joueur a appuyé sur la touche bas (pour atteindre le bouton confirmer)


        //initialisation de l'objet
        public MulliganCommand()
        {
            Cursor.Position = getAbsolutePos(FirstCardLocation);
            getCardNumber();
            CreateCardList();
        }
        

        public void getCardNumber()
        {
            if (CoreAPI.Game.Player.HasCoin) // le joueur qui joue en deuxième possède la piece
                CardNumber = 4;
            else
                CardNumber = 3;
        }

        internal void CreateCardList()
        {
            if (CardNumber == 3)
                CardPos = new List<WindowsPoint> {
                new WindowsPoint(-60,0),
                new WindowsPoint(0,0),
                new WindowsPoint(60,0)};
            else
                CardPos = new List<WindowsPoint> {
                new WindowsPoint(-60,0),
                new WindowsPoint(-20,0),
                new WindowsPoint(20,0),
                new WindowsPoint(60,0)};
        }


        //Gestion des inputs 
        public CommandModule Command(InputCommand input)
        {
            switch (input)
            {
                case InputCommand.Up:
                    Down = false;
                    break;
                case InputCommand.Down:
                    Down = true;
                    break;
                case InputCommand.Left:
                    if( !Down) // tant que l'on est sur le bouton Confirmer, on ne peut pas naviguer entre les cartes
                        navigate(currentCardIndex - 1);
                    break;
                case InputCommand.Right:
                    if (!Down) // tant que l'on est sur le bouton Confirmer, on ne peut pas naviguer entre les cartes
                        navigate(currentCardIndex + 1);
                    break;
                case InputCommand.Select:
                    click();
                    break;
                case InputCommand.Unselect:
                    break;
            }

            updatePosition();
            return this;
        }


        private void navigate(int newIndex)
        {
            if (newIndex < 0 || newIndex >= CardNumber + 1)
            {
                return;
            }
            currentCardIndex = newIndex;
        }


        private void updatePosition()
        {
            if (!Down)
                Cursor.Position = getAbsolutePos(CardPos[currentCardIndex]);
            else
                Cursor.Position = getAbsolutePos(new WindowsPoint(0, -65));
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
