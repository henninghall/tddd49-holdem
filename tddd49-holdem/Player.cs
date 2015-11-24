using System;
using System.Linq;
using tddd49_holdem.actions;


namespace tddd49_holdem
{
    public class Player : Data
    {
		public Cards Cards { set; get; }
        public Table Table;
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }
        public int ChipsOnHand { set; get; }
		public int CurrentBet { set; get; }
    
        public Player() { }

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

        public void Fold()
        {
            // does not add player to the doneMoveQueue which
            // makes the player inactive this round
            
            // throws away the cards on hand.
            Console.WriteLine(Name + " folds.");
        }

        public void Check()
        {
			if (Table.Rules.IsCheckValid(this, Table)) {
                Table.AfterMove.Enqueue(Table.BeforeMove.Dequeue());
				Console.WriteLine (Name + " checks.");
			} else throw new InvalidActionException("Check is not valid");		
		}

        public void Call()
        {
            if (Table.Rules.IsCallValid(this, Table))
            {
                Bet(Table.GetHighestBet() - CurrentBet);
                Table.AfterMove.Enqueue(Table.BeforeMove.Dequeue());
                Console.WriteLine(Name + " calls.");
            }
			else throw new InvalidActionException("Call is not valid");	
        }

        public void Raise(int bet)
        {
            if (Table.Rules.IsRaiseValid(this, Table))
            {
				// bet = raise + diff to max bet. 
				bet += (Table.GetHighestBet() - CurrentBet);

                Bet(bet);

                // force every active player to move again
                Table.MoveAllAfterMoveToBeforeMove();
                Table.AfterMove.Enqueue(Table.BeforeMove.Dequeue());

                Console.WriteLine(Name + " raises " + bet + ".");
            }
			else throw new InvalidActionException("Raise is not valid");	
        }

        public void MakeConsoleMove()
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

        public void MakeMove(PlayerAction action) {
            if (!action.IsValid()) throw new InvalidActionException();
            action.Execute();
            if (Table.GetNumberOfActivePlayers() == 1) Table.EndGame();
            if (Table.BeforeMove.Count == 0) {
                Table.MovePlayerBetsToPot();
                if (!Table.HasNextCard()) Table.EndGame();
                else {
                    Table.NextCard();
                    Table.MoveAllAfterMoveToBeforeMove();
                }
            }
            Table.NextPlayer();
        }

		public Cards GetAllCards(){
            Cards allCards = new Cards();
            allCards.AddRange(Cards);
            allCards.AddRange(Table.CardsOnTable);
			return allCards; 
		}

		
    }



}