using System.Linq;
using tddd49_holdem.actions;

namespace tddd49_holdem.Players
{
    public class HumanPlayer : Player
    {
        public override bool IsUsingGui { get; } = true;
        public HumanPlayer(string name) : base(name){ }

        public HumanPlayer() { }

        private void UpdatePossibleActions() {

            using (HoldemContext db = new HoldemContext()) {
                Player player = db.Players.First(p => p.Name == Name);

                player.CanFold = new Fold(this).IsValid();
                player.CanCheck = new Check(this).IsValid();
                player.CanCall = new Call(this).IsValid();
                player.CanRaise = new Raise(this).IsValid();
                db.SaveChanges();
                MainWindow.RefreshContext();
            }
        }


        public override void RequestActionExcecution() {
            UpdatePossibleActions();
        }
    }



}