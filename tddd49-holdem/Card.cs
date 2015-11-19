namespace tddd49_holdem
{
    public class Card
    {
		public int _color {private set; get;}
		public int _value {private set; get;}
    
        public Card(int color, int value)
        {
            _color = color;
            _value = value;
        }

        public override string ToString()
        {
			return  getColorSymbol(_color) + " " + getNumberSymbol(_value);
        }

		private string getColorSymbol(int color){
			switch (color){
			case 0: return "♠";
			case 1: return "♥";
			case 2: return "♦";
			case 3: return "♣";
			}
			return null;
		}

		private string getNumberSymbol(int number){
			switch (number){
			case 11: return "Kn";
			case 12: return "D";
			case 13: return "K";
			case 14: return "A";
			}
			return number.ToString();
		}

    }
}