using System;

namespace tddd49_holdem
{
	public class Draw
	{
		public Cards Cards { private set; get;}
		public DrawType Type { private set; get;}

		public Draw (Cards allCardsAvailable, RulesEngine rules)
		{
			Cards cards;
			DrawType type;
			rules.GetDraw(allCardsAvailable, out type, out cards);
			Cards = cards;
			Type = type;
		}
	}
}

