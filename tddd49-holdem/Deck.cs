using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;

namespace tddd49_holdem
{
    public class Deck<Card> : Stack<Card>()
    {
        public Deck()
        {
            Push(CreateAllCards()); 
            PrintDeck();
        }

        private List<Card> CreateAllCards()
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

        public void PrintDeck()
        {
            object[] cardsInDeck = ToArray();
            foreach (var card in cardsInDeck)
            {
                Console.WriteLine(card);
            }
        }

        public List<Card> Pop(int numberOfCards)
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < numberOfCards; i++)
            {
                cards.Add((Card) Pop());   
            }
            return cards;
        }

        
    }
}