using System.Collections.Generic;
using System.Linq;

namespace tddd49_holdem.counters
{
    public abstract class CardCounter : List<Cards>
    {
        protected CardCounter(int size) {
            for (int i = 0; i < size; i++)
            {
                Add(new Cards());
            }
        }

        public int Max()
        {
            int currentMax = 0;
            foreach (Cards cards in this)
            {
                if (cards.Count > currentMax)
                    currentMax = cards.Count;
            }
            return currentMax;
        }
        
        public int HighestMaxIndex()
        {
            int currentMax = 0;
            int currentMaxIndex = 0;
            int i = 0;
            foreach (Cards cards in this)
            {
                // >= to make sure the highest card index is returned if there are several max indexes
                if (cards.Count >= currentMax)
                {
                    currentMax = cards.Count;
                    currentMaxIndex = i;
                }
                i++;
            }
            return currentMaxIndex;
        }

        ///  Example: n=2, amount=3 returns the second highest threeOfAKind index
        private int IndexOfNthAmountOfCards(int n, int amount)
        {
            List<int> foundIndexes = new List<int>();

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Count == amount) foundIndexes.Add(i);
            }

            return foundIndexes[foundIndexes.Count - n];
        }

        ///  Example: n=2, amount=3 returns the second highest threeOfAKind
        private Cards GetNthAmountOfCards(int n, int amount)
        {
            return this[this.IndexOfNthAmountOfCards(n, amount)];
        }

        public bool Contains(int cardsCount) {
            return this.Any(cards => cards.Count == cardsCount);
        }

        /// Example size=2 returns number of pairs
        public int NumberOfCardsOfSize(int size) {
            return this.Count(cards => cards.Count == size);
        }

        public override string ToString() {
            return this.Aggregate("", (current, cards) => current + (" (" + cards + ")"));
        }

        public Cards GetStraight(int straightSize, int startsAt)
        {
            Cards chosen = new Cards();
            for (int i = startsAt; i < straightSize + startsAt; i++)
            {
                chosen.Add(this[i][0]);
            }
            return chosen;
        }

        //// returns highest single cards in decending order 
        ///  returns the specifyed amount as maximum or as many as possible
        public Cards GetHighestSingleCards(int amount)
        {
            int numberOfSingles = NumberOfCardsOfSize(1);
            if (numberOfSingles < amount) amount = numberOfSingles;
            Cards singleCards = new Cards();
            for (int i = 0; i < amount; i++)
            {
                singleCards.AddRange(GetNthAmountOfCards(i + 1, 1));
            }
            return singleCards;
        }

        public Cards GetHighestPair()
        {
            return GetNthAmountOfCards(1, 2);
        }

        public Cards GetSecondHighestPair()
        {
            return GetNthAmountOfCards(2, 2);
        }

        public Cards GetThirdHighestPair()
        {
            return GetNthAmountOfCards(3, 2);
        }

        public Cards GetHighestThreeOfAKind()
        {
            return GetNthAmountOfCards(1, 3);
        }

        public Cards GetHighestFourOfAKind()
        {
            return GetNthAmountOfCards(1, 4);
        }

        public Cards GetSecondHighestThreeOfAKind()
        {
            return GetNthAmountOfCards(2, 3);
        }
        
    }
}

