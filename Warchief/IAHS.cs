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

namespace Warchief
{
    class IAHS
    {
        static int TurnNum;
        static Player FirstPlayer;
        static Player CurrentPlayer;
        internal Advisor A= new Advisor();


        internal BoardRegionNavigation Hand= new HandNavigator();


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
            }
            else
            {
                SetCurrentPlayer();
                if(CurrentPlayer==FirstPlayer)
                    TurnNum++;
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


            A.SetLabel(CurrentPlayer.Name + TurnNum.ToString());

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

        internal void OnCurveMinon()
        {
            int HandSize = CoreAPI.Game.Player.HandCount;
            int BoardSize = CoreAPI.Game.Player.Board.Count();

            int PosInHand = -1;
            int index = 0;

            if (BoardSize < 7)
            {
                foreach (Entity Card in CoreAPI.Game.Player.Hand)
                {

                    
                }
            }
            else
                EndTurn();


            //Go through the hand
            //If coresponding minons
            //get hand on the minon
            // select
            // Get Hand to Board
            // Select

        }

        internal void EndTurn()
        {
            // get hand to end turn button
            // select
        }
        internal void PlayCard(int PosInHand)
        {

        }

    }
}
