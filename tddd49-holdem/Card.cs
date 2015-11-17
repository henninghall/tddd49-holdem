namespace tddd49_holdem

{
    public class Card
    {
        private readonly int _color;
        private readonly int _value;
    
        public Card(int color, int value)
        {
            _color = color;
            _value = value;
        }

        public override string ToString()
        {
			return  getSymbol(_color) + " " + _value;
        }

		private string getSymbol(int color){
			switch (color){

			case 0: return "♠";
			case 1: return "♥";
			case 2: return "♦";
			case 3: return "♣";

			}
			return null;
		}
    }
}