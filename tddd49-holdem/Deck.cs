using System;
using System.Collections.Generic;
using System.Linq;

namespace tddd49_holdem
{
    public class Deck : HoldemQueue<Card>
    {
        public Deck()
        {
            AddRange(CreateAllCards());
            Shuffle();
        }

        private void Push(IEnumerable<Card> cardList)
        {
            AddRange(cardList);
        }

        private IEnumerable<Card> CreateAllCards()
        {
            List<Card> allCards = new List<Card>();
            for (byte i = 2; i < 15; i++)
            {
                for (byte j = 0; j < 4; j++)
                {
                    allCards.Add(new Card(j, i));
                }
            }
            return allCards;
        }

        public override string ToString()
        {
            return this.ToArray().Aggregate("", (current, card) => current + (card + "\n"));
        }

        public void Shuffle()
        {
            Card[] deck = ToArray();
            Clear();

            //	Based on Java code from wikipedia:
            //	http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
            Random r = new Random();
            for (int n = deck.Length - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card temp = deck[n];
                deck[n] = deck[k];
                deck[k] = temp;
            }
            Push(new List<Card>(deck));
        }


        public Cards Dequeue(int cardsOnHand)
        {
            Cards cards = new Cards();
            for (int i = 0; i < cardsOnHand; i++)
            {
                cards.Add(Dequeue());
            }
            return cards;
        }
    }
}