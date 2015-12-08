using System.Collections.Generic;

namespace tddd49_holdem
{
    public class NumberOfCardsList : HoldemQueue<int>
    {
        private HoldemQueue<int> _cardList;

        public NumberOfCardsList(IEnumerable<int> cardList)
        {
            AddRange(cardList);

        }

    }
}