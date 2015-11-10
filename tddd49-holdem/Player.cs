using System;
using System.Collections.Generic;


namespace tddd49_holdem
{
    public class Player
    {
        //private HashSet<Card> _cards = new HashSet<Card>();
        private List<Card> _cards = new List<Card>() ;  

        public Player(List<Card> cards)
        {
            _cards = cards;
            Console.WriteLine("player created");
        }

    }
}