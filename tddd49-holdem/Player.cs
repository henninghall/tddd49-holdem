﻿using System;

namespace tddd49_holdem
{
    public class Player : Data
    {
        public Cards Cards { set; get; }

        public Table Table;
        public Table get_Table()
        {
            return Table;
        }
        public void set_Table(Table t) {
            SetField(ref Table, t, "Table");
        }

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

        public Cards GetAllCards()
        {
            Cards allCards = new Cards();
            allCards.AddRange(Cards);
            allCards.AddRange(Table.CardsOnTable);
            return allCards;
        }

    }



}