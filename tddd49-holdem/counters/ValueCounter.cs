using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tddd49_holdem.counters
{
    public class ValueCounter : CardCounter
    {
        public ValueCounter(Cards cards) :base(15) {

            foreach (Card card in cards)
            {
                this[card.Value].Add(card);
            }

        }
    }
}
