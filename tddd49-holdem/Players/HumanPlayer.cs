using tddd49_holdem.actions;

namespace tddd49_holdem.Players
{
    public class HumanPlayer : Player
    {
        public override bool IsUsingGui { get; } = true;
        public HumanPlayer(string name) : base(name){ }
        public HumanPlayer() { }
        
        private void UpdatePossibleActions()
        {
            CanFold = new Fold(this).IsValid();
            CanCheck = new Check(this).IsValid();
            CanCall = new Call(this).IsValid();
            CanRaise = new Raise(this).IsValid();
        }


        public override void RequestActionExcecution() {
            UpdatePossibleActions();
        }
    }



}