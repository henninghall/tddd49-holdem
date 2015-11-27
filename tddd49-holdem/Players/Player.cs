﻿using tddd49_holdem.actions;

namespace tddd49_holdem.Players
{
    public abstract class Player : Data
    {
        public Fold Fold;
        public Check Check;
        public Call Call;
        public Raise Raise;

        public abstract bool IsUsingGui { get; }
        
        private Cards _cards;
        public Cards Cards {
            get { return _cards; }
            set { SetField(ref _cards, value, "Cards"); }
        }

        public Table Table;

        public Table get_Table() {
            return Table;
        }

        public void set_Table(Table t) {
            SetField(ref Table, t, "Table");
        }

        private int _chipsOnHand;

        public int ChipsOnHand {
            get { return _chipsOnHand; }
            set { SetField(ref _chipsOnHand, value, "ChipsOnHand"); }
        }

        /// <summary>
        /// The active field is needed for the gui active player detection.
        /// The table also keeps tracks of the active player in a variable. 
        /// Probably possibly to solve by using Multibinding and/or Converters
        /// in the PlayerPanel or MainWindow.
        /// </summary>
        private bool _active;

        public bool Active {
            get { return _active; }
            set { SetField(ref _active, value, "Active"); }
        }

        private int _currentBet;

        public int CurrentBet {
            get { return _currentBet; }
            set { SetField(ref _currentBet, value, "CurrentBet"); }
        }

        private string _name;

        public string Name {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }

        private bool _canCheck;

        public bool CanCheck {
            get { return _canCheck; }
            set { SetField(ref _canCheck, value, "CanCheck"); }
        }

        private bool _canFold;

        public bool CanFold {
            get { return _canFold; }
            set { SetField(ref _canFold, value, "CanFold"); }
        }

        private bool _canCall;

        public bool CanCall {
            get { return _canCall; }
            set { SetField(ref _canCall, value, "CanCall"); }
        }

        private bool _canRaise;
        public bool CanRaise {
            get { return _canRaise; }
            set { SetField(ref _canRaise, value, "CanRaise"); }
        }


        protected Player() { }

        protected Player(string name) {
            Name = name;
            Fold = new Fold(this);
            Check = new Check(this);
            Call = new Call(this);
            Raise = new Raise(this);
        }

        public void Bet(int amount) {
            ChipsOnHand -= amount;
            CurrentBet += amount;
        }

        public Cards GetAllCards() {
            Cards allCards = new Cards();
            allCards.AddRange(Cards);
            allCards.AddRange(Table.CardsOnTable);
            return allCards;
        }

        /// <summary>
        /// This method is called by the table when it requests an action from the player. 
        /// </summary>
        public abstract void RequestActionExcecution();
       
    }
}