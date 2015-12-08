using System.Collections.ObjectModel;
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
	}
}

