namespace tddd49_holdem.actions
{
    public class Check : PlayerAction
    {
        public Check(Table table, Player player) : base(table, player){}

        public override bool IsValid()
        {
            return(Table.GetHighestBetPlayers().Contains(Player));
        }

        public override void Execute()
        {
           // a check does nothing
        }
    }
}