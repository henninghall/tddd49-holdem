using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;


namespace tddd49_holdem
{
    public class Player
    {
		public Cards Cards;
        public Table Table;
        public string Name { private set; get; }
		public int ChipsOnHand; 
		public int CurrentBet;
    
        public Player(string name)
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
			if (Table.Rules.IsCheckValid(this, Table)) {
				Table.AfterMove.Enqueue (this);
				Console.WriteLine (Name + " checks.");
			} else throw new InvalidActionException("Check is not valid");		
		}

        private void Call()
        {
            if (Table.Rules.IsCallValid(this, Table))
            {
                Bet(Table.GetHighestBet() - CurrentBet);
                Table.AfterMove.Enqueue(this);
                Console.WriteLine(Name + " calls.");
            }
			else throw new InvalidActionException("Call is not valid");	
        }

        private void Raise(int bet)
        {
            if (Table.Rules.IsRaiseValid(this, Table))
            {
				// bet = raise + diff to max bet. 
				bet += (Table.GetHighestBet() - CurrentBet);

                Bet(bet);

                // force every active player to move again
                Table.MoveAllAfterMoveToBeforeMove();
              
                Table.AfterMove.Enqueue(this);
				Console.WriteLine(Name + " raises " + bet + ".");
            }
			else throw new InvalidActionException("Raise is not valid");	
        }

        public void MakeMove()
        {
			int input = Convert.ToInt32(Console.ReadLine());
			switch (input)
				{
				case 0: Fold(); break;
				case 1: Check(); break;
				case 2: Call(); break;
				case 3: Raise(100); break;
				default:
					Console.WriteLine ("Invalid input, try again: ");
					break;
				}
        }

		public Cards GetAllCards(){
            Cards allCards = new Cards();
            allCards.AddRange(Cards);
            allCards.AddRange(Table.CardsOnTable);
			return allCards; 
		}
			
    }
}