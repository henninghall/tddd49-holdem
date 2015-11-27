using tddd49_holdem.Players;

namespace tddd49_holdem.actions
{
    public abstract class PlayerAction
    {
        protected Player Player;
        
        protected PlayerAction(Player player)
        {
            Player = player;
        }

        public abstract bool IsValid();
        public abstract void Execute();

    }
}