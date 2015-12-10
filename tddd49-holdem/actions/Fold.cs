using tddd49_holdem.Players;

namespace tddd49_holdem.actions
{
    public class Fold : PlayerAction
    {
        public Fold(Player player) : base(player) { }

        public override bool IsValid()
        {
            // fold is always ok.
            return true;
        }

        public override void Execute()
        {
            //Player.Table.BeforeMove.Dequeue();
            Player.Cards.Clear();
            Player.Table.LogBox.Log(Player.Name + " folded!");
            Player.Table.ReactOnActionExecution();
        }
    }
}