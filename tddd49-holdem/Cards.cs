using System;
using System.Collections.Generic;

namespace tddd49_holdem
{
	public class Cards : List<Card>
	{
		public override String ToString ()
		{
			String s = "";
			foreach (Card card in this) {
				s += card + " ";
			}
			return s;
		}

		public Cards GetSubCards(int index, int count){
			Cards subCards = new Cards ();
			int i = 0;
			foreach (Card card in this) {
				if (i < index)
					continue;
				else if (i < index + count)
					subCards.Add (card);
				else
					break;
			}
			return subCards;
		}


	}
}

