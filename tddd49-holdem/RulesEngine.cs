using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tddd49_holdem
{
    public class RulesEngine
    {
        private Table _table;

        public static readonly List<int> CardsBeforeRound = new List<int>{0, 3, 1, 1};
        public const int StartingChips = 1000;
        public const int CardsOnHand = 2;

        public RulesEngine(Table table)
        {
            _table = table;
        }

        public bool IsFoldValid(Player player)
        {
            // fold always ok
            return true;
        }

        public bool IsCheckValid(Player player)
        {
            return (_table.GetHighestBetPlayers().Contains(player));
        }

        public bool IsCallValid(Player player)
        {
             // not highest bet already
           if (!_table.GetHighestBetPlayers().Contains(player))
            {
                // have enough chips
                if (_table.GetHighestBet() <= (player.CurrentBet + player.ChipsOnHand))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsRaiseValid(Player player)
        {
            // have enough chips
            if ((player.CurrentBet + player.ChipsOnHand) > _table.GetHighestBet())
            {
                return true;
            }
            return false;
        }

    }
}
