using System.Collections.Generic;
using System.Linq;
using System.Windows;
using tddd49_holdem.Players;

namespace tddd49_holdem
{
    public class Table : Data
    {
        public int TableId { set; get; }
        public virtual List<Player> AllPlayers { get; set; } = new List<Player>();
        public virtual HoldemQueue<Player> BeforeMove { get; set; } // active players waiting to move
        public virtual HoldemQueue<Player> AfterMove { get; set; } // active players already moved
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

        public void ContinueRound() {
            NextPlayer();
        }

        public void StartNewRound()
        {
            MoveAllAfterMoveToBeforeMove();
            MakeAllPlayersActive();
            CardsOnTable.Clear();
            Deck = new Deck();
            ClearPlayerCards();
            HandOutCards();
            ShowGuiPlayersCards();
            ResetCardQueue();
            LogBox.Log("Round started!");
            NextPlayer();
        }


        public void NextPlayer() {
            if (ActivePlayer != null) ActivePlayer.Active = false;
            ActivePlayer = BeforeMove.Peek();
            ActivePlayer.Active = true;
            ActivePlayer.RequestActionExcecution();
            MainWindow.SyncState();
        }

        public void ReactOnActionExecution()
        {
            if (GetNumberOfActivePlayers() == 1) EndRound();
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
        }

        private void ShowGuiPlayersCards() {
            foreach (Card card in AllPlayers.Where(player => player.IsUsingGui).SelectMany(player => player.Cards)) {
                card.Show = true;
            }
        }

        private void ShowAllCards() {
            foreach (Card card in AllPlayers.SelectMany(player => player.Cards)) {
                card.Show = true;
            }
        }

        /// <summary>
        /// Gives all players at the table the number of cards defined in RulesEngine
        /// </summary>
        private void HandOutCards()
        {
            foreach (Player player in AllPlayers)
            {
                player.Cards = Deck.Dequeue(RulesEngine.CardsOnHand);
            }
        }

        /// <summary>
        /// Removes all cards from all players at the table
        /// </summary>
        private void ClearPlayerCards()
        {
            foreach (Player player in AllPlayers) {
                player.Cards?.Clear();
            }
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
            AllPlayers.Add(player);
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
            foreach (Player player in AllPlayers)
            {
                Pot += player.CurrentBet;
                player.CurrentBet = 0;
            }
        }

        /// <summary>
        /// Returns the player or players at the table who currently 
        /// has the highest bet.
        /// </summary>
        /// <returns>Returns a set with the player or players
        /// who currently has the highest bet.</returns>
        public HashSet<Player> GetHighestBetPlayers()
        {
            int currentHighestBet = 0;
            HashSet<Player> currentHighestBetPlayers = new HashSet<Player>();
            foreach (Player player in AllPlayers)
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
        public int GetHighestBet() {
            return AllPlayers.Select(player => player.CurrentBet).Concat(new[] {0}).Max();
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

        /// <summary>
        /// Makes all players active by adding all players at the table to the BeforeMove queue. 
        /// </summary>
        public void MakeAllPlayersActive()
        {
            BeforeMove = new HoldemQueue<Player>(AllPlayers);
        }

        public void MoveAllAfterMoveToBeforeMove()
        {
            while (AfterMove != null && AfterMove.Count > 0)
            {
                BeforeMove.Enqueue(AfterMove.Dequeue());
            }
        }

        public int GetNumberOfActivePlayers()
        {
            return GetActivePlayers().Count;
        }


        public List<Player> GetActivePlayers()
        {
            return AfterMove.Concat(BeforeMove).ToList();
        }

        public bool IsPlayerActive(Player player)
        {
            return GetActivePlayers().Contains(player);
        }

        public void EndRound()
        {
            MovePlayerBetsToPot();
            HashSet<Player> winners = Rules.GetWinners(this);
            string message;
            if (winners.Count == 1)
            {
                Player winner = winners.First();
                message = "Game over! The winner is " + winner.Name;
                if (GetNumberOfActivePlayers() > 1) message += " with " + Rules.GetDrawType(winner.GetAllCards());
            }
            else
            {
                message = "Game over! There was a Tie between: ";
                foreach (Player winner in winners) LogBox.Log(winner.Name);
            }

            GivePotToPlayers(winners);
            ShowAllCards();
            LogBox.Log(message);
            MessageBoxResult result = MessageBox.Show(message + ". Start next round?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                StartNewRound();
            }
        }

        private void GivePotToPlayers(HashSet<Player> winners)
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

        private void ResetCardQueue() {
            if (NumberOfCardsToPutOnTable == null)
                NumberOfCardsToPutOnTable = new HoldemQueue<IntegerObject>();

            NumberOfCardsToPutOnTable.Clear();
            foreach (IntegerObject inteObj in RulesEngine.CardsBeforeRound.Select(variable => new IntegerObject(variable))) {
                NumberOfCardsToPutOnTable.Add(inteObj);
            }
        }

        public bool HasNextPlayer()
        {
            return BeforeMove.Any();
        }
    }
}