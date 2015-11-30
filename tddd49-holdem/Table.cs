using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using tddd49_holdem.Players;

namespace tddd49_holdem
{
    public class Table : Data
    {
        private readonly List<Player> _allPlayers = new List<Player>();
        public Queue<Player> BeforeMove; // active players waiting to move
        public Queue<Player> AfterMove = new Queue<Player>(); // active players already moved
        public RulesEngine Rules { get; }
        public Deck Deck;
        private Queue<int> _numberOfCardsToPutOnTable;
        public LogBox LogBox { get; set; }
        public Cards CardsOnTable { get; set; }

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

        public Table()
        {
            CardsOnTable = new Cards();
            LogBox = new LogBox();
            Rules = new RulesEngine();
            Pot = 0;
        }

        public void StartRound()
        {
            MoveAllAfterMoveToBeforeMove();
            MakeAllPlayersActive();
            CardsOnTable.Clear();
            Deck = new Deck();
            HandOutCards();

            // Test data
            Player p1 = _allPlayers[0];
            Player p2 = _allPlayers[1];

            /*
            p1.Cards.Clear();
            p2.Cards.Clear();
            p1.Cards.Add(new Card(1, 14));
            p1.Cards.Add(new Card(2, 14));
            p2.Cards.Add(new Card(3, 14));
            p2.Cards.Add(new Card(0, 14));
            */

            ShowGuiPlayersCards();
            ResetCardQueue();
            NextPlayer();
            LogBox.Log("Round started!");
        }

        private void ShowGuiPlayersCards() {
            foreach (Player player in _allPlayers) {
                if (player.IsUsingGui) {
                    foreach (Card card in player.Cards) {
                        card.Show = true;
                    }
                }
            }
        }

        private void ShowAllCards() {
            foreach (Player player in _allPlayers) {
                foreach (Card card in player.Cards) {
                    card.Show = true;
                }
            }
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

        private void HandOutCards()
        {
            foreach (Player player in _allPlayers)
            {
                player.Cards = Deck.Pop(RulesEngine.CardsOnHand);
            }
        }

        public void AttachPlayer(Player player)
        {
            player.Table = this;
            player.ChipsOnHand = RulesEngine.StartingChips;
            _allPlayers.Add(player);
            LogBox.Log(player.Name + " joined the table.");
        }

        public void AttachPlayers(IEnumerable<Player> allPlayers)
        {
            foreach (Player player in allPlayers)
            {
                AttachPlayer(player);
            }
        }

        public void MovePlayerBetsToPot()
        {
            foreach (Player player in _allPlayers)
            {
                Pot += player.CurrentBet;
                player.CurrentBet = 0;
            }
        }

        public HashSet<Player> GetHighestBetPlayers()
        {
            int currentHighestBet = 0;
            HashSet<Player> currentHighestBetPlayers = new HashSet<Player>();
            foreach (Player player in _allPlayers)
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

        public int GetHighestBet()
        {
            int currentHighestBet = 0;
            foreach (Player player in _allPlayers)
            {
                if (player.CurrentBet > currentHighestBet)
                {
                    currentHighestBet = player.CurrentBet;
                }
            }
            return currentHighestBet;
        }

        public void PutCards(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                CardsOnTable.Add(card);
                LogBox.Log("Put card '" + card + "' on table.");
            }
        }

        public void MakeAllPlayersActive()
        {
            BeforeMove = new Queue<Player>(_allPlayers);
        }

        public void MoveAllAfterMoveToBeforeMove()
        {
            while (AfterMove.Count > 0)
            {
                BeforeMove.Enqueue(AfterMove.Dequeue());
            }
        }

        public int GetNumberOfActivePlayers()
        {
            return GetActivePlayers().Count;
        }

        // Winner is often one player but it could also be a tie between several players. 
        public HashSet<Player> GetWinners()
        {
            HashSet<Player> activePlayers = new HashSet<Player>(GetActivePlayers());

            // if all players except one folded
            if (activePlayers.Count == 1) return activePlayers;

            // else winner(s) is determined due to rules
            return Rules.GetBestDrawPlayers(activePlayers);
        }

        private List<Player> GetActivePlayers()
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
            HashSet<Player> winners = GetWinners();
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
                StartRound();
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
            PutCards(Deck.Pop(_numberOfCardsToPutOnTable.Dequeue()));
        }

        public bool HasNextCard()
        {
            return _numberOfCardsToPutOnTable.Any();
        }

        private void ResetCardQueue()
        {
            _numberOfCardsToPutOnTable = new Queue<int>(new Queue<int>(RulesEngine.CardsBeforeRound));
        }

        public void NextPlayer()
        {
            if (ActivePlayer != null) ActivePlayer.Active = false;
            ActivePlayer = BeforeMove.Peek();
            ActivePlayer.Active = true;
            ActivePlayer.RequestActionExcecution();
        }

        public bool HasNextPlayer()
        {
            return BeforeMove.Any();
        }
    }
}