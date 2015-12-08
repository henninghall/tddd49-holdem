﻿using System.Collections.Generic;
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
            foreach (Player player in AllPlayers)
            {
                player.Cards = Deck.Dequeue(RulesEngine.CardsOnHand);
            }
        }

        private void ClearPlayerCards()
        {
            foreach (Player player in AllPlayers) {
                player.Cards?.Clear();
            }
        }


        public void AttachPlayer(Player player)
        {
            player.Table = this;
            player.ChipsOnHand = RulesEngine.StartingChips;
            AllPlayers.Add(player);
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
            foreach (Player player in AllPlayers)
            {
                Pot += player.CurrentBet;
                player.CurrentBet = 0;
            }
        }

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

        public int GetHighestBet() {
            return AllPlayers.Select(player => player.CurrentBet).Concat(new[] {0}).Max();
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
            BeforeMove = new HoldemQueue<Player>(AllPlayers);
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

        public void NextPlayer() {

            if (ActivePlayer != null) ActivePlayer.Active = false;
            ActivePlayer = BeforeMove.Peek();
            ActivePlayer.Active = true;
            ActivePlayer.RequestActionExcecution();

            MainWindow.Db.SaveChanges();
        }

        public bool HasNextPlayer()
        {
            return BeforeMove.Any();
        }
    }
}