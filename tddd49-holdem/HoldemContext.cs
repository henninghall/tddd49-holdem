using System.Data.Entity;
using tddd49_holdem.Players;

namespace tddd49_holdem
{
    public class HoldemContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<IntegerObject> NumberOfCardsToPutOnTable { get; set; }

        public HoldemContext(){}
    }
}