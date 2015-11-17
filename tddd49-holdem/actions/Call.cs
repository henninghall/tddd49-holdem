namespace tddd49_holdem.actions
{
    public class Call : PlayerAction
    {
        public Call(Table table, Player player) : base(table, player)
        {
        }

        public override bool IsValid()
        {
            // not highest bet already
            if (!_table.GetHighestBetPlayers().Contains(_player))
            {
                // have enough chips
                if (_table.GetHighestBet() <= (_player.CurrentBet + _player.ChipsOnHand))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Execute()
        {
           // _player.Bet(_table.GetHighestBet() - _player.CurrentBet);
        }
    }
}