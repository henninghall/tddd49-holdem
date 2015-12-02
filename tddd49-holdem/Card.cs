using System;
using tddd49_holdem.Exceptions;

namespace tddd49_holdem
{
    //public enum CardColor { Black, Red }

    public class Card : Data
    {
		public int CardId { get; set; }
		public int Suit { get; set; }
		public int Value {get; set; }
        public string SuitSymbol { get; set; }
        public string ValueSymbol { get; set; }
        public string Color { get; set; }
        private bool _show;
        public bool Show
        {
            get { return _show; }
            set { SetField(ref _show, value, "Show"); }
        }

        // Needed for sample data in gui    
        public Card() { }

        public Card(int suit, int value)
        {
            Suit = suit;
            Value = value;
            SuitSymbol = GetSuitSymbol(suit);
            ValueSymbol = GetNumberSymbol(value);
            Color = GetColor(suit);
        }


        public override string ToString()
        {
			return  GetSuitSymbol(Suit) + " " + GetNumberSymbol(Value);
        }

		private string GetSuitSymbol(int suit){
			switch (suit){
			case 0: return "♠";
			case 1: return "♥";
			case 2: return "♦";
			case 3: return "♣";
			}
            throw new InvalidSuitException();
        }

        private string GetNumberSymbol(int number){
			switch (number){
			case 11: return "Kn";
			case 12: return "D";
			case 13: return "K";
			case 14: return "A";
			}
			return number.ToString();
		}

        private string GetColor(int suit) {
            switch (suit)
            {
                case 0: return "Black";
                case 1: return "Red"; 
                case 2: return "Red";
                case 3: return "Black";
            }
            throw new InvalidSuitException();
        }
        
    }
}