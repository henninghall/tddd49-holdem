namespace cs_holdem.actions
{
    public class Raise : PlayerAction
    {
        public Raise(Table table, Player player) : base(table, player)
        {
        }

        public override bool IsValid()
        {
            // have enough chips
            if ((_player.CurrentBet + _player.ChipsOnHand) > _table.GetHighestBet())
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