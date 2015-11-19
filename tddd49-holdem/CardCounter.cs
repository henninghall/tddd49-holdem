using System;
using System.Collections.Generic;


namespace tddd49_holdem
{
	public class CardCounter : List<Cards>
	{
		public CardCounter (int size)
		{
			for (int i = 0; i < size; i++) {
				Add(new Cards());
			}
		}

		public int Max(){
			int currentMax = 0;
			foreach (Cards cards in this) {
				if (cards.Count > currentMax)
					currentMax = cards.Count;
			}
			return currentMax;
		}

		public int HighestMaxIndex(){
			int currentMax = 0;
			int currentMaxIndex = 0;
			int i = 0;
			foreach (Cards cards in this) {
				// >= to make sure the highest card index is returned if there are several max indexes
				if (cards.Count >= currentMax) {
					currentMax = cards.Count;
					currentMaxIndex = i; 
				}
				i++;
			}
			return currentMaxIndex;
		}

		///  Example: n=2, amount=3 returns the second highest threeOfAKind index
		private int IndexOfNthAmountOfCards(int n, int amount){
			List<int> foundIndexes = new List<int> ();

			for (int i = 0; i < this.Count; i++) {
				if (this[i].Count == amount) foundIndexes.Add(i);
			}

			return foundIndexes[foundIndexes.Count-n];
		}

		///  Example: n=2, amount=3 returns the second highest threeOfAKind
		private Cards GetNthAmountOfCards(int n, int amount){
			return this[this.IndexOfNthAmountOfCards(n,amount)].GetSubCards(0, amount);
		}

		public bool Contains(int cardsCount){
			foreach (Cards cards in this) {
				if (cards.Count == cardsCount)
					return true;
			}
			return false;
		}

		/// Example size=2 returns number of pairs
		public int NumberOfCardsOfSize(int size){
			int count = 0;
			foreach (Cards cards in this) {
				if (cards.Count == size)
					count++;
			}
			return count;
		}

		public override string ToString ()
		{
			string s = "";
			foreach (Cards cards in this) {
				s += " (" + cards + ")";
			}
			return s;
		}

		public Cards getStraight(int straightSize, int startsAt){
			Cards chosen = new Cards();
			for (int i = startsAt; i < straightSize + startsAt; i++) {
				chosen.Add(this[i][0]);
			}
			return chosen;
		}

		public Cards GetHighestSingelCards(int amount){
			Cards singleCards = new Cards ();
			for (int i = 0; i < amount; i++) {
				singleCards.AddRange(GetNthAmountOfCards(i+1,1));
			}
			return singleCards;
		}

		public Cards GetHighestPair(){
			return GetNthAmountOfCards(1,2);
		}

		public Cards GetSecondHighestPair(){
			return GetNthAmountOfCards(2,2);
		}

		public Cards GetHighestThreeOfAKind(){
			return GetNthAmountOfCards(1,3);
		}

		public Cards GetHighestFourOfAKind(){
			return GetNthAmountOfCards(1,4);
		}

		public Cards GetSecondHighestThreeOfAKind(){
			return GetNthAmountOfCards(2,3);
		}


			



	}
}

