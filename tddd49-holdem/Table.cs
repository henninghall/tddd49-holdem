using System.Collections.Generic;
using System.Linq;
using System.Windows;
using tddd49_holdem.Players;

namespace tddd49_holdem
{
    public class Table : Data
    {
        public int TableId { set; get; }
        public virtual PlayerList Players { set; get; } = new PlayerList();
        public virtual RulesEngine Rules { get; } = new RulesEngine();
        public virtual Deck Deck { get; set; } = new Deck();
        public virtual HoldemQueue<IntegerObject> NumberOfCardsToPutOnTable { get; set; }
        public virtual LogBox LogBox { get; set; } = new LogBox();
        public virtual Cards CardsOnTable { get; set; } = new Cards();

        private int _pot;
        public int Pot
        {
            get { return _pot; }
            set { SetField(ref _pot, value, "Pot"); }
        }
        private Player _activePlayer;
        public Player ActivePlayer
        {
            get { return _activePlayer; }
            set { SetField(ref _activePlayer, value, "ActivePlayer"); }
        }

        public void ContinueRound()
        {
            NextPlayer();
        }

        public void StartNewRound()
        {
            CardsOnTable.Clear();
            Deck = new Deck();
            ResetCardQueue();

            Players.SetNoBet();
            Players.ClearCards();
            Players.HandOutCards(Deck);
            Players.ShowGuiPlayersCards();
            LogBox.Log("Round started!");
            NextPlayer();
        }


        public void NextPlayer()
        {
            if (ActivePlayer != null) ActivePlayer.Active = false;
            ActivePlayer = Players.NextPlayerWithCards();
            ActivePlayer.Active = true;
            ActivePlayer.RequestActionExcecution();

            MainWindow.SyncState();
        }

        public void ReactOnActionExecution()
        {
            // time to end round
            if (Players.NumberOfPlayersWithCards() == 1) EndRound();

            // time for next card or end round
            if (Players.NextPlayerWasLastToBet())
            {
                MovePlayerBetsToPot();
                if (HasNextCard())
                {
                    NextCard();
                    NextPlayer();
                    Players.SetNoBet();
                }
                else EndRound();
            }
            // time for next player
            else NextPlayer();

            // Doesnt wait for any more player actions 
            /*  if (GetNumberOfActivePlayers() == 1) EndRound();
            if (BeforeMove.Count == 0)
            {
                MovePlayerBetsToPot();
                if (!HasNextCard()) { EndRound(); }
                else
                {
                    NextCard();
                    MoveAllAfterMoveToBeforeMove();
                }
            }
            if (HasNextPlayer()) { NextPlayer(); }

              */
        }

        /// <summary>
        /// Attaches a player to the table and gives the player 
        /// the amount of starting chips defined in Rulesengine.
        /// </summary>
        /// <param name="player"></param>
        public void AttachPlayer(Player player)
        {
            player.Table = this;
            player.ChipsOnHand = RulesEngine.StartingChips;
            Players.Add(player);
            LogBox.Log(player.Name + " joined the table.");
        }

        /// <summary>
        /// Attaches several players to the table and gives the player 
        /// the amount of starting chips defined in Rulesengine.
        /// </summary>
        /// <param name="allPlayers"></param>
        public void AttachPlayers(IEnumerable<Player> allPlayers)
        {
            foreach (Player player in allPlayers)
            {
                AttachPlayer(player);
            }
        }

        /// <summary>
        /// Moves all players current bets to the table pot.
        /// </summary>
        public void MovePlayerBetsToPot()
        {
            foreach (Player player in Players)
            {
                Pot += player.CurrentBet;
                player.CurrentBet = 0;
            }
        }

        /// <summary>
        /// Puts cards on the table.
        /// </summary>
        /// <param name="cards"></param>
        public void PutCards(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                CardsOnTable.Add(card);
                LogBox.Log("Put card '" + card + "' on table.");
            }
        }


        public void EndRound()
        {
            MovePlayerBetsToPot();
            PlayerList winners = Rules.GetWinners(this);
            string message;
            if (winners.Count == 1)
            {
                Player winner = winners.First();
                message = "Game over! The winner is " + winner.Name;
                if (Players.Count > 1) message += " with " + Rules.GetDrawType(winner.GetAllCards());
            }
            else
            {
                message = "Game over! There was a Tie between: ";
                foreach (Player winner in winners) LogBox.Log(winner.Name);
            }

            GivePotToPlayers(winners);
            Players.ShowAllCards();
            LogBox.Log(message);
            MessageBoxResult result = MessageBox.Show(message + ". Start next round?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                StartNewRound();
            }
        }

        private void GivePotToPlayers(PlayerList winners)
        {
            foreach (Player winner in winners)
            {
                int winSum = Pot / winners.Count;
                winner.ChipsOnHand += winSum;
                Pot -= winSum;
            }
        }

        public void NextCard()
        {
            PutCards(Deck.Dequeue(NumberOfCardsToPutOnTable.Dequeue().TheInt));
        }

        public bool HasNextCard()
        {
            return NumberOfCardsToPutOnTable.Any();
        }

        private void ResetCardQueue()
        {
            if (NumberOfCardsToPutOnTable == null)
                NumberOfCardsToPutOnTable = new HoldemQueue<IntegerObject>();

            NumberOfCardsToPutOnTable.Clear();
            foreach (IntegerObject inteObj in RulesEngine.CardsBeforeRound.Select(variable => new IntegerObject(variable)))
            {
                NumberOfCardsToPutOnTable.Add(inteObj);
            }
        }
    }
}