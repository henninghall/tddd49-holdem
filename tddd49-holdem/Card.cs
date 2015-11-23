namespace tddd49_holdem
{
    public class Card
    {
		public int Color { get; set; }
		public int Value {get; set; }

        // Needed for sample data in gui    
        public Card() { }

        public Card(int color, int value)
        {
            Color = color;
            Value = value;
        }

        public override string ToString()
        {
			return  GetColorSymbol(Color) + " " + getNumberSymbol(Value);
        }

		private string GetColorSymbol(int color){
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