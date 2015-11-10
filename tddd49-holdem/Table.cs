using System.Collections.Generic;

namespace tddd49_holdem
{
    public class Table
    {
        private List<Player> _activePlayers;
        private List<Player> _inactivePlayers;
        private Deck _deck;
        private List<Card> _cardsOnTable;

        public Table(List<Player> allPlayers, Deck deck )
        {
            _activePlayers = allPlayers;
            _inactivePlayers = new List<Player>();
            _deck = deck;
            _cardsOnTable = new List<Card>();
        }

        public int GetNumberOfCardsOnTable()
        {
            return _cardsOnTable.Count;
        }

        public List<Player> GetActivePlayers()
        {
            return _activePlayers;
        } 

    }
}