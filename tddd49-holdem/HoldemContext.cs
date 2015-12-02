using System.Data.Entity;
using tddd49_holdem.Players;

namespace tddd49_holdem
{
    public class HoldemContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
    }
}