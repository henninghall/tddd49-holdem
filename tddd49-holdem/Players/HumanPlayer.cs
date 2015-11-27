namespace tddd49_holdem.Players
{
    public class HumanPlayer : Player
    {
        public override bool IsUsingGui { get; } = true;
        public HumanPlayer(string name) : base(name){ }
        public HumanPlayer() { }
        
        private void UpdatePossibleActions()
        {
            CanFold = Fold.IsValid();
            CanCheck = Check.IsValid();
            CanCall = Call.IsValid();
            CanRaise = Raise.IsValid();
        }


        public override void RequestActionExcecution() {
            UpdatePossibleActions();
        }
    }



}