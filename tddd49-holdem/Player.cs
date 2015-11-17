using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using cs_holdem.actions;


namespace cs_holdem
{
    public class Player
    {
        public HashSet<Card> Cards {  get; set; }
        public Table Table;
        public String Name { private set; get; }
        public int ChipsOnHand {set; get; }
        public int CurrentBet { private set; get; }
    
        public Player(String name)
        {
            Name = name;
            Console.WriteLine("Player created: " + name);
        }

        private void Bet(int amount)
        {
            ChipsOnHand -= amount;
            CurrentBet += amount;
        }

        private void Fold()
        {
            // does not add player to the doneMoveQueue which
            // makes the player inactive this round
            
            // throws away the cards on hand.
            Console.WriteLine(Name + " folds.");
        }

        private void Check()
        {
            if (Table.Rules.IsCheckValid(this))
            {
                Table.AfterMove.Enqueue(this);
                Console.WriteLine(Name + " checks.");
            }
        }

        private void Call()
        {
            if (Table.Rules.IsCallValid(this))
            {
                Bet(Table.GetHighestBet() - CurrentBet);
                Table.AfterMove.Enqueue(this);
                Console.WriteLine(Name + " calls.");
            }
        }

        private void Raise(int bet)
        {
            if (Table.Rules.IsRaiseValid(this))
            {
                Bet(bet);
                // force every active player to move again
                Table.MoveAllAfterMoveToBeforeMove();
              
                Table.AfterMove.Enqueue(this);
                Console.WriteLine(Name + " raises.");
            }
        }

        public void MakeMove()
        {
           
            int input = Console.Read();
            switch (input)
            {
                case 0: Fold(); break;
                case 1: Check(); break;
                case 2: Call(); break;
                case 3: Raise(10); break;
            }
        }

        
    }
}