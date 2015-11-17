using System;
using System.Collections.Generic;
using System.Linq;

namespace cs_holdem
{
    public class Table
    {
        /* private List<Player> ActivePlayers { get; private set; }
        private List<Player> InactivePlayers { get; private set; }
        */
        private List<Player> _allPlayers = new List<Player>();
        public Queue<Player> BeforeMove; // active players waiting to move
        public Queue<Player> AfterMove = new Queue<Player>(); // active players already moved
        public RulesEngine Rules { get; private set; }
        private List<Card> _cardsOnTable;
        public int Pot { get; private set; }
        public Deck Deck = new Deck();
        private bool _gameOver = false;
        

        public Table()
        {
            _cardsOnTable = new List<Card>();
            Rules = new RulesEngine(this);
            Pot = 0;
        }

        public void StartGame()
        {
            Console.WriteLine("Game started.");

            // make all players active before each game
            MakeAllPlayersActive();

            // game loop
            // normally 4 rounds 0,3,1,1 cards before each round.
            foreach (var cardsToPopCurrentRound in RulesEngine.CardsBeforeRound)
            {

                // moves all active players to beforeMove
                MoveAllAfterMoveToBeforeMove();

                // pop the round-specific amount of cards on the table defined by rules.
                // normal texas hold'em rules gives 0 cards before first round etc. 
                PutCards(Deck.Pop(cardsToPopCurrentRound));

                while (BeforeMove.Count != 0)
                {
                    var currentPlayer = BeforeMove.Dequeue();

                    //table.DisplayValidActionsForPlayer(player);
                    currentPlayer.MakeMove();

                    // quit game if only one player left
                    if (AfterMove.Count + BeforeMove.Count == 1)
                    {
                        _gameOver = true;
                        break;
                    }
                }
                //table.movePlayersBetToPot();

                if (_gameOver) break;
            }
            Console.WriteLine("Game ended. The winner is: " + GetWinner().Name);
        }

        public void AttachPlayer(Player player)
        {
            player.Table = this;
            player.ChipsOnHand = RulesEngine.StartingChips;
            player.Cards = Deck.Pop(RulesEngine.CardsOnHand);
            _allPlayers.Add(player);
            Console.WriteLine(player.Name + " joined table with cards: " + player.Cards.First() + " and " + player.Cards.Last());
        }

        public void AttachPlayers(List<Player> allPlayers)
        {
            foreach (var player in allPlayers)
            {
                AttachPlayer(player);
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
                return currentHighestBet;
            }
            return 0;
        }

        public void DisplayValidActionsForPlayer(Player player)
        {
            if (Rules.IsFoldValid(player)) Console.WriteLine("0: Fold");
            if (Rules.IsCheckValid(player)) Console.WriteLine("1: Check");
            if (Rules.IsCallValid(player)) Console.WriteLine("2: Call");
            if (Rules.IsRaiseValid(player)) Console.WriteLine("3: Raise");
        }

        public void PutCards(HashSet<Card> cards)
        {
            foreach (var card in cards)
            {
                _cardsOnTable.Add(card);
                Console.WriteLine("Put card '" + card + "' on table.");
            }
        }

        public void MakeAllPlayersActive()
        {
            BeforeMove = new Queue<Player>(_allPlayers);
        }

        public void MoveAllAfterMoveToBeforeMove()
        {
            while(AfterMove.Count > 0)
            {
                BeforeMove.Enqueue(AfterMove.Dequeue());
            }
        }

        public Player GetWinner()
        {
            // if all players except one folded
            if (AfterMove.Count + BeforeMove.Count == 1)
            {
                if (AfterMove.Any()) return AfterMove.Dequeue();
                return BeforeMove.Dequeue();
            }
            else throw new NotImplementedException("Winner algorithm sholud be implemented here."); 
        }


    }
}