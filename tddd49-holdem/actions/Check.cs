namespace tddd49_holdem.actions
{
    public class Check : PlayerAction
    {
        public Check(Table table, Player player) : base(table, player){}

        public override bool IsValid()
        {
            return(_table.GetHighestBetPlayers().Contains(_player));
        }

        public override void Execute()
        {
           // a check does nothing
        }
    }
}