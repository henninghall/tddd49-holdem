using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace tddd49_holdem
{
    public class Table : Data
    {
        /* private List<Player> ActivePlayers { get; private set; }
        private List<Player> InactivePlayers { get; private set; }
        */
        private readonly List<Player> _allPlayers = new List<Player>();
        public Queue<Player> BeforeMove; // active players waiting to move
        public Queue<Player> AfterMove = new Queue<Player>(); // active players already moved
        public RulesEngine Rules { get; private set; }
        public int Pot { get; set; }
        public Deck Deck = new Deck();
        private bool _gameOver = false;
        private readonly Queue<int> _numberOfCardsToPutOnTable = new Queue<int>(new Queue<int>(RulesEngine.CardsBeforeRound));
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
            Rules = new RulesEngine();
            Pot = 0;
        }

        public void StartGuiGame()
        {
            MakeAllPlayersActive();
            NextPlayer();
        }

        public void AttachPlayer(Player player)
        {
            player.Table = this;
            player.ChipsOnHand = RulesEngine.StartingChips;
            player.Cards = Deck.Pop(RulesEngine.CardsOnHand);
            _allPlayers.Add(player);
            Console.WriteLine(player.Name + " joined table with cards: " + player.Cards.First() + " and " + player.Cards.Last());
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

        /*
        public void DisplayValidActionsForPlayer(Player player)
        {
            if (Rules.IsFoldValid(player, this)) Console.WriteLine("0: Fold");
            if (Rules.IsCheckValid(player, this)) Console.WriteLine("1: Check");
            if (Rules.IsCallValid(player, this)) Console.WriteLine("2: Call");
            if (Rules.IsRaiseValid(player, this)) Console.WriteLine("3: Raise");
        }
        */
        public void PutCards(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                CardsOnTable.Add(card);
                Console.WriteLine("Put card '" + card + "' on table.");
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

        public void PrintTable()
        {
            Console.WriteLine();
            Console.WriteLine("Table Cards: \t" + CardsOnTable);
            Console.WriteLine("Table Pot: \t" + Pot);
            Console.WriteLine();
            Console.WriteLine("Name \t\t Active \t Cards \t\t CurrentBet");

            foreach (Player player in _allPlayers)
            {
                Console.WriteLine(player.Name + "\t " + IsPlayerActive(player) + "\t\t " + player.Cards + "\t " + player.CurrentBet);
            }
            Console.WriteLine();
        }

        // TODO: Print all winners
        public void EndGame()
        {
            MessageBox.Show("Game over! The winner is " + GetWinners().First().Name);
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
    }
}