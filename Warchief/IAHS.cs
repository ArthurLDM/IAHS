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
        static int TurnNum = 0;
        static Player CurrentPlayer;
        internal Advisor A = new Advisor();


        internal void GameStart()
        {
            A.Show();
        }

        internal void TurnStart()
        {
            SetCurrentPlayer();


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

        }

        internal void SetCurrentPlayer()
        {
            if (TurnNum == 0 && CoreAPI.Game.Player.GoingFirst)
                CurrentPlayer = CoreAPI.Game.Player;
            if (TurnNum == 0 && CoreAPI.Game.Opponent.GoingFirst)
                CurrentPlayer = CoreAPI.Game.Opponent;

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

    }
}
