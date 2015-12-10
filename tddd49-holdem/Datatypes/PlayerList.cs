using System.Collections.Generic;
using System.Linq;
using tddd49_holdem.Players;

namespace tddd49_holdem
{
    /// <summary>
    /// A circular list of players.
    /// </summary>
    public class PlayerList : List<Player>
    {
        private int _current;
        private int _lastBet;

        private int GetNextIndex()
        {
            return (_current + 1) % Count;
        }

        private void IncreaseIndex()
        {
            _current = GetNextIndex();
        }

        /// <summary>
        /// Returns next player in list.
        /// Increases the index to this player.
        /// </summary>
        private Player Next()
        {
            IncreaseIndex();
            return this[_current];
        }

        /// <summary>
        /// Returns next player with cards (active in the current game) in list.
        /// Increases the index to this player.
        /// </summary>
        public Player NextPlayerWithCards()
        {
            Player player;
            do
            {
                player = Next();
            } while (!player.HasCards());
            return player;
        }

        public Player PeekNext()
        {
            return this[GetNextIndex()];
        }

        public Player LastBet()
        {
            return this[_lastBet];
        }

        public bool HasPlayerWithCards()
        {
            return this.Any(player => player.HasCards());
        }

        public int NumberOfPlayersWithCards()
        {
            return this.Count(player => player.HasCards());
        }

        public PlayerList GetPlayerWithCards()
        {
            PlayerList players = new PlayerList();
            players.AddRange(this.Where(player => player.HasCards()));
            return players;
        }

        /// <summary>
        /// Returns true if the next player was the last to bet or first to start the round.
        /// </summary>
        public bool NextPlayerWasLastToBet()
        {
            return PeekNext().Equals(LastBet());
        }

        public void SetNoBet()
        {
            _lastBet = _current;
        }

        /// <summary>
        /// Gives all players at the table the number of cards defined in RulesEngine
        /// </summary>
        public void HandOutCards(Deck deck)
        {
            foreach (Player player in this)
            {
                player.Cards = deck.Dequeue(RulesEngine.CardsOnHand);
            }
        }

        /// <summary>
        /// Removes all cards from all players at the table
        /// </summary>
        public void ClearCards()
        {
            foreach (Player player in this)
            {
                player.Cards?.Clear();
            }
        }

        /// <summary>
        /// Returns the player or players at the table who currently 
        /// has the highest bet.
        /// </summary>
        /// <returns>Returns a set with the player or players
        /// who currently has the highest bet.</returns>
        public HashSet<Player> GetHighestBetters()
        {
            int currentHighestBet = 0;
            HashSet<Player> currentHighestBetPlayers = new HashSet<Player>();
            foreach (Player player in this)
            {
                if (player.CurrentBet > currentHighestBet)
                {
                    currentHighestBetPlayers.Clear();
                    currentHighestBetPlayers.Add(player);
                    currentHighestBet = player.CurrentBet;
                }
                else if (player.CurrentBet == currentHighestBet)
                {
                    currentHighestBetPlayers.Add(player);
                }
            }
            return currentHighestBetPlayers;
        }

        /// <summary>
        /// Returns the value of the current highest bet at the table.
        /// </summary>
        public int GetHighestBet()
        {
            return this.Select(player => player.CurrentBet).Concat(new[] { 0 }).Max();
        }

        /// <summary>
        /// Shows all the cards of all players
        /// </summary>
        public void ShowAllCards()
        {
            foreach (Card card in this.SelectMany(player => player.Cards))
            {
                card.Show = true;
            }
        }

        /// <summary>
        /// Shows the cards attached to gui players. 
        /// The Ai-players cards will still be hidden. 
        /// </summary>
        public void ShowGuiPlayersCards()
        {
            foreach (Card card in this.Where(player => player.IsUsingGui).SelectMany(player => player.Cards))
            {
                card.Show = true;
            }
        }

        /// <summary>
        /// Shares the pot equally between the players
        /// </summary>
        public void GivePot(int pot)
        {
            int potShare = pot / Count;

            foreach (Player player in this)
            {
                player.ChipsOnHand += potShare;
            }

        }
  
        public int GetBetsValue() {
            return this.Sum(player => player.CurrentBet);
        }

        public void ClearBets()
        {
            foreach (Player player in this)
            {
                player.CurrentBet = 0;
            }
        }
    }
}