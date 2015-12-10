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

        public void Seed(HoldemContext context)
        {
            // Normal seeding goes here
            Table table1 = new Table();
            Player p1 = new HumanPlayer("Bamse");
            Player p2 = new AiPlayer("Skalman");
            table1.AttachPlayer(p1);
            table1.AttachPlayer(p2);
            context.Tables.Add(table1);
            context.SaveChanges();
            context.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }

        public class DropCreateIfChangeInitializer : DropCreateDatabaseIfModelChanges<HoldemContext>
        {
            protected override void Seed(HoldemContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }

        public class CreateInitializer : CreateDatabaseIfNotExists<HoldemContext>
        {
            protected override void Seed(HoldemContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }

        static HoldemContext()
        {
            Database.SetInitializer<HoldemContext>(new CreateInitializer());
        }
    }
}