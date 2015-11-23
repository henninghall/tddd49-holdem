
using System;

namespace tddd49_holdem.actions
{
    public abstract class PlayerAction
    {
        protected Table Table;
        protected Player Player; 

        protected PlayerAction(Table table, Player player)
        {
            Table = table;
            Player = player;
        }

        public abstract bool IsValid();
        public abstract void Execute();


    }
}