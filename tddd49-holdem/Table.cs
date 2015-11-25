using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using tddd49_holdem.actions;

namespace tddd49_holdem
{
    public class Table : Data
    {
        private readonly List<Player> _allPlayers = new List<Player>();
        public Queue<Player> BeforeMove; // active players waiting to move
        public Queue<Player> AfterMove = new Queue<Player>(); // active players already moved
        public RulesEngine Rules { get; }
        public int Pot { get; set; }
        public Deck Deck = new Deck();
        private readonly Queue<int> _numberOfCardsToPutOnTable = new Queue<int>(new Queue<int>(RulesEngine.CardsBeforeRound));
        public LogBox LogBox { get; set; }
        private Player _activePlayer;
        public Player ActivePlayer
        {
            get { return _activePlayer; }
            set { SetField(ref _activePlayer, value, "ActivePlayer"); }
        }

        public Cards CardsOnTable { get; set; }

        public Table()
        {
            CardsOnTable = new Cards();
            LogBox = new LogBox();
            Rules = new RulesEngine();
            Pot = 0;
        }

        public void StartGuiGame()
        {
            MakeAllPlayersActive();
            NextPlayer();
        }

        public void MakeMove(PlayerAction action)
        {
            if (!action.IsValid()) throw new InvalidActionException();
            action.Execute();
            if (GetNumberOfActivePlayers() == 1) EndGame();
            if (BeforeMove.Count == 0)
            {
                MovePlayerBetsToPot();
                if (!HasNextCard()) EndGame();
                else
                {
                    NextCard();
                    MoveAllAfterMoveToBeforeMove();
                }
            }
            if(HasNextPlayer()) NextPlayer();
        }

        public void AttachPlayer(Player player) {
            player.Table = this;
            player.ChipsOnHand = RulesEngine.StartingChips;
            player.Cards = Deck.Pop(RulesEngine.CardsOnHand);
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

        public void EndGame() {
            HashSet<Player> winners = GetWinners();
            if (winners.Count == 1) {
                Player winner = winners.First();
                LogBox.Log("Game over! The winner is " + winner.Name + " with " +
                           Rules.GetDrawType(winner.GetAllCards()));
            }
            else {
                LogBox.Log("Game over! There was a Tie between: ");
                foreach (Player winner in winners) {
                    LogBox.Log(winner.Name);
                }
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

        public void NextPlayer()
        {
            ActivePlayer = BeforeMove.Peek();
        }
        public bool HasNextPlayer()
        {
            return BeforeMove.Any();
        }
    }
}