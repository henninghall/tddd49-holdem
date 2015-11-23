namespace tddd49_holdem.actions
{
    public class Raise : PlayerAction
    {
        public Raise(Table table, Player player) : base(table, player)
        {
        }

        public override bool IsValid()
        {
            // have enough chips
            if ((Player.CurrentBet + Player.ChipsOnHand) > Table.GetHighestBet())
            {
                return true;
            }

            return false;
        }

        // TODO: Remove fixed bet.
        public override void Execute()
        {
           // _player.Bet(10);
        }
    }
}