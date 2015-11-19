using System;
using System.Collections.Generic;
using System.Linq;

namespace tddd49_holdem
{
    public class Game
    {

        static void Main(string[] args)
        {
			Card c1 = new Card (0, 2);
			Card c2 = new Card (1, 5);
			Card c3 = new Card (2, 7);
			Card c4 = new Card (1, 8);
			Card c5 = new Card (2, 11);
			Card c6 = new Card (1, 11);
			Card c7 = new Card (1, 14);

			Cards cards = new Cards();
			cards.Add(c1);
			cards.Add(c2);
			cards.Add(c3);
			cards.Add(c4);
			cards.Add(c5);
			cards.Add(c6);
			cards.Add(c7);

			//Cards subCards = cards.GetSubCards (0, 2);

			Draw draw = new Draw (cards,new RulesEngine(new Table()));

			Console.WriteLine("Draw type: " + draw.Type);
			Console.WriteLine("Draw cards: " + draw.Cards);

			/*
            // creating table
            var table = new Table();

            // creating players
            var p1 = new Player("Player 1");
            var p2 = new Player("Player 2");
            var p3 = new Player("Player 3");
           
            // Attach players to table
            table.AttachPlayer(p1);
            table.AttachPlayer(p2);
            table.AttachPlayer(p3);

            // start game at table
			table.StartGame();
			*/
			}

    }
}