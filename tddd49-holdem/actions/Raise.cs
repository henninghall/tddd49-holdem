using tddd49_holdem.Players;

namespace tddd49_holdem.actions
{
    public class Raise : PlayerAction
    {
        public Raise(Player player) : base(player) { }

        public override bool IsValid()
        {
            // must have enough chips
            return (Player.CurrentBet + Player.ChipsOnHand) > Player.Table.GetHighestBet();
        }

        // TODO: Remove fixed bet.
        public override void Execute()
        {
            int bet = 10;
            // bet = raise + diff to max bet. 
            bet += (Player.Table.GetHighestBet() - Player.CurrentBet);

            Player.Bet(bet);

            // force every active player to move again...
            Player.Table.MoveAllAfterMoveToBeforeMove();
            // ... except player who made the bet
            Player.Table.AfterMove.Enqueue(Player.Table.BeforeMove.Dequeue());

            Player.Table.LogBox.Log(Player.Name + " raised!");
            Player.Table.ReactOnActionExecution();
        }
    }
}