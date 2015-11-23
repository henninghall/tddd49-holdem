using tddd49_holdem.counters;

namespace tddd49_holdem
{
	public class Draw
	{
		public Cards Cards { private set; get;}
		public DrawType Type { set; get;}
	    public ValueCounter ValueCounter;

		public Draw ()
		{
			
		}

	    public Draw(Cards cards, DrawType drawType) {
	        Cards = cards;
	        Type = drawType;
	    }
        
	}
}

