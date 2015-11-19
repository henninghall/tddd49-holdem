using System;
using System.Collections.Generic;
using System.Linq;

namespace tddd49_holdem
{
	public class Cards : List<Card>
	{
		public override string ToString () {
		    return this.Aggregate("", (current, card) => current + (card + " "));
		}
    }
}

