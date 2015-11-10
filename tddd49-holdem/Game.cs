using System;
using System.Collections.Generic;

namespace tddd49_holdem
{
    public class Game
    {

        static void Main(string[] args)
        {   
           
            // creating deck
            Deck deck = new Deck();
            //deck.shuffle();

            // creating players
            var p1 = new Player(deck.Pop(2));
            var p2 = new Player(deck.Pop(2));
            List<Player> players = new List<Player>();
            players.Add(p1);
            players.Add(p2);

            // creating table
            Table table = new Table(players, deck);

            // game loop
            while (table.GetNumberOfCardsOnTable() < 5)
            {
                foreach (var player in table.GetActivePlayers())
                {   
                   // player.ChooseAction();

                }       
            }
        }
      }
}