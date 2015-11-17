using System;
using System.Collections.Generic;

namespace tddd49_holdem
{
	public class Cards : HashSet<Card>
	{
		public override String ToString ()
		{
			String s = "";
			foreach (Card card in this) {
				s += card + " ";
			}
			return s;
		}
	}
}

