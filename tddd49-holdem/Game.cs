using System;
using System.Collections.Generic;
using System.Linq;
using cs_holdem.actions;

namespace cs_holdem
{
    public class Game
    {

        static void Main(string[] args)
        {
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
        }
    }
}