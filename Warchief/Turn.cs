using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WindowsPoint = System.Windows.Point;

using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using HearthDb.Enums;

namespace Warchief
{
    class Turn
    {

        internal List<List<WindowsPoint>> Actions;
        internal int Mana=1;
        internal int HandCount=CoreAPI.Game.Player.HandCount;

        internal int BoardSize = CoreAPI.Game.Player.Board.Count();
        internal IEnumerable<Entity> Hand = CoreAPI.Game.Player.Hand;

        internal List<Entity> Entities =>
            Helper.DeepClone<Dictionary<int, Entity>>(CoreAPI.Game.Entities).Values.ToList<Entity>();

        internal Entity Opponent => Entities?.FirstOrDefault(x => x.IsOpponent);
        internal Entity Player => Entities?.FirstOrDefault(x => x.IsPlayer);


        internal List<string> cards = new List<string>();
        internal List<int> pos = new List<int>();
        internal Turn()
        {
            Actions = new List<List<WindowsPoint>>();
            getHandSize();
            AvailableMana();

        }
        
        internal void AvailableMana()
        {

            var player = Player;
            if (player != null && player.GetTag(GameTag.RESOURCES)!=0)
            { 
                var mana = player.GetTag(GameTag.RESOURCES);
                var overload = player.GetTag(GameTag.OVERLOAD_OWED);

                // looking a turn ahead, so add one mana
                Mana += mana - overload;
            }
        }

        internal void getHandSize()
        {
                HandCount = CoreAPI.Game.Player.HandCount;
        }

        internal void MakeTurn ()
        {
            

            if (CoreAPI.Game.IsMulliganDone)
            {
                Attack();

                //Check if there is card that can be played
                List<Entity> playableCards;
                if(canPlayMinions())
                {
                    playableCards = PlayableCards();
                    PlayMinion(playableCards);
                }
            }

            if (Mana >= 2)
                HeroPower();
            
            EndTurn();
        }

        internal bool canPlayMinions()
        {
            return (BoardSize < 7 && PlayableCards().Count() != 0);
        }
        
        internal List<Entity> PlayableCards()
        {
            List<Entity> Cards = new List<Entity>();

            IEnumerable<Entity> hand = CoreAPI.Game.Player.Hand;
            foreach(Entity card in hand)
            {
                if (card.Cost <= Mana && card.IsMinion)
                    Cards.Add(card);
            }
            return (Cards);
        }

        internal void PlayMinion(List<Entity> playableCards)
        {

            WindowsPoint MinionLocation = new WindowsPoint(0, 0);
            WindowsPoint BoardLocation = new WindowsPoint(0, -20);
            int MinionsPlayed = 0;
           
            if (playableCards.Count != 0)
            {

                HandNavigator HandNav = new HandNavigator();

                foreach (Entity card in playableCards)
                {
                    List<WindowsPoint> Positions = new List<WindowsPoint>();
                    if (card.Cost <= Mana)
                    {
                        getHandSize();
                        MinionLocation = HandNav.handLocations[HandCount- MinionsPlayed][card.GetTag(GameTag.ZONE_POSITION)-1];

                        Positions.Add(MinionLocation);
                        Positions.Add(BoardLocation);

                        Actions.Add(Positions);

                        cards.Add(card.LocalizedName);
                        pos.Add(card.GetTag(GameTag.ZONE_POSITION));
                        Mana -= card.Cost;
                        MinionsPlayed++;
                    }
                }
            }
        }

        internal void HeroPower()
        {
            List<WindowsPoint> HeroPower = new List<WindowsPoint>();
            HeroPower.Add(new WindowsPoint(33, -60));
            Actions.Add(HeroPower);
        }

        internal void EndTurn()
        {
            List<WindowsPoint> endTurn = new List<WindowsPoint>();
            endTurn.Add(new WindowsPoint(112, 0));
            Actions.Add(endTurn);
        }

        internal void Attack()
        {
            List<Entity> OppBoard = CoreAPI.Game.Opponent.Board.Where(x => x.IsMinion).OrderBy(x => x.GetTag(GameTag.ZONE_POSITION)).ToList();
            List<int> Targets = FindTarget(OppBoard);
            

            MinionNavigator PlayerBoardNav = new MinionNavigator(true);

            List<Entity> PlayerBoard = CoreAPI.Game.Player.Board.Where(x => x.IsMinion).OrderBy(x => x.GetTag(GameTag.ZONE_POSITION)).ToList();
            int index = 0;
            foreach(Entity minion in PlayerBoard)
            {
                List<WindowsPoint> Attack = new List<WindowsPoint>();

                if (Targets.Count==0)
                {
                    
                    Attack.Add(PlayerBoardNav.minionLocation(index));
                    Attack.Add(new WindowsPoint(0, 63));

                    Actions.Add(Attack);
                    index++;
                }

                index++;
            }
        }

        internal List<int> FindTarget(IEnumerable<Entity> oppBoard)
        {
            List<int> TauntPos = new List<int>();
            int i = 0;
            foreach (var e in oppBoard)
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

        internal string Debug()
        {
            string name = "";

            getHandSize();
            name += "Hand size : " + HandCount.ToString() + "\r\n";

            name += "Mana : " + Mana.ToString() + "\r\n";

            name += "CanPlayMinion : " + canPlayMinions().ToString() + "\r\n"; 
        
            foreach(Entity E in PlayableCards())
                name += "Playable : " + E.LocalizedName + "\r\n";

            name += "Action : " + Actions.Count.ToString() + "\r\n";

            foreach (List<WindowsPoint> a in Actions)
            {
                name += "Action points :";
                foreach (WindowsPoint b in a)
                {
                    name +=" (" + b.X.ToString()+","+b.Y.ToString() + ")";
                }
                name += "\r\n";
            }

            foreach (string a in cards)
            {
                name += "Cartes a jouer : ";
                
                name += a + " ";
                name += "\r\n";
            }

            foreach(int a in pos)
            {
                name += "Pos a jouer : ";

                name += a + " ";
                name += "\r\n";
            }
            return name;

        }
    }
}
