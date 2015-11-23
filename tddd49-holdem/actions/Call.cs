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
            if (!Table.GetHighestBetPlayers().Contains(Player))
            {
                // have enough chips
                if (Table.GetHighestBet() <= (Player.CurrentBet + Player.ChipsOnHand))
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