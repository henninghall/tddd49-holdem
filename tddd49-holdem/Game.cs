
namespace tddd49_holdem
{
    public class Game
    {
        public static void Main(string[] args)
        {
            /*
            
            Card c7 = new Card(0, 11);
            Card c5 = new Card(0, 12);
            Card c1 = new Card (0, 13);
			Card c2 = new Card (0, 14);
			Card c3 = new Card (2, 4);
			Card c4 = new Card (1, 3);
			Card c6 = new Card (3, 14);    

            Cards cards1 = new Cards {c1, c2, c3, c4, c5, c6, c7};
            Cards cards2 = new Cards {c1, c1, c3, c3, c5, c6, c7};

			Draw draw1 = new RulesEngine(new Table()).GetDraw(cards1);
			Draw draw2 = new RulesEngine(new Table()).GetDraw(cards2);

            Console.WriteLine("Draw1 type: " + draw1.Type);
            Console.WriteLine("Draw1 cards: " + draw1.Cards);

            Console.WriteLine("Draw2 type: " + draw2.Type);
            Console.WriteLine("Draw2 cards: " + draw2.Cards);

            Draw bestDraw = new RulesEngine(new Table()).GetBestDraw(draw1, draw2);

            Console.WriteLine("Best draw: " + bestDraw.Cards);


             */

            // creating table
            Table table = new Table();

            // creating players
            Player p1 = new Player("Player 1");
            Player p2 = new Player("Player 2");
            Player p3 = new Player("Player 3");

            // Attach players to table
            table.AttachPlayer(p1);
            table.AttachPlayer(p2);
            table.AttachPlayer(p3);

            // start game at table
            table.StartGame();
                       
        }

    }
}