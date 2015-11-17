namespace tddd49_holdem.actions
{
    public class Fold : PlayerAction
    {
        public Fold(Table table, Player player) : base(table, player){}

        public override bool IsValid()
        {
            // fold is always ok.
            return true;
        }

        public override void Execute()
        {
          //  _player.Fold();
        }
    }
}