using System;
using System.Linq;
using tddd49_holdem.actions;


namespace tddd49_holdem
{
    public class Player : Data
    {
        public Cards Cards { set; get; }
        public Table Table;
        private int _chipsOnHand;

        public int ChipsOnHand
        {
            get { return _chipsOnHand; }
            set { SetField(ref _chipsOnHand, value, "ChipsOnHand"); }
        }
        private int _currentBet;
        public int CurrentBet
        {
            get { return _currentBet; }
            set { SetField(ref _currentBet, value, "CurrentBet"); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }
        

        public Player() { }

        public Player(string name)
        {
            Name = name;
            Console.WriteLine("Player created: " + name);
        }

        public void Bet(int amount)
        {
            ChipsOnHand -= amount;
            CurrentBet += amount;
        }

        public void MakeMove(PlayerAction action)
        {
            if (!action.IsValid()) throw new InvalidActionException();
            action.Execute();
            if (Table.GetNumberOfActivePlayers() == 1) Table.EndGame();
            if (Table.BeforeMove.Count == 0)
            {
                Table.MovePlayerBetsToPot();
                if (!Table.HasNextCard()) Table.EndGame();
                else
                {
                    Table.NextCard();
                    Table.MoveAllAfterMoveToBeforeMove();
                }
            }
            Table.NextPlayer();
        }

        public Cards GetAllCards()
        {
            Cards allCards = new Cards();
            allCards.AddRange(Cards);
            allCards.AddRange(Table.CardsOnTable);
            return allCards;
        }


    }



}