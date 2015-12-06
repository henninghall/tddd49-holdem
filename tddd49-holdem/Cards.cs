using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace tddd49_holdem
{
	public class Cards : ObservableCollection<Card>
	{
		public override string ToString () {
		    return this.Aggregate("", (current, card) => current + (card + " "));
		}

	    public void AddRange(Cards cards) {
	        foreach (Card card in cards) {
	            Add(card);
	        }
	    }

	    public Cards Concat(Cards cards) {
            Cards allCards = new Cards();
            allCards.AddRange(cards);
            allCards.AddRange(this);
	        return allCards;
	    }
	}
}

