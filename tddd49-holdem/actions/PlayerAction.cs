
using System;

namespace cs_holdem.actions
{
    public abstract class PlayerAction
    {
        protected Table _table;
        protected Player _player; 

        protected PlayerAction(Table table, Player player)
        {
            _table = table;
            _player = player;
        }

        public abstract bool IsValid();
        public abstract void Execute();


    }
}