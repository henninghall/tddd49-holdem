namespace tddd49_holdem.counters
{
    public class ColorCounter : CardCounter
    {
        public ColorCounter(Cards cards) : base(4) {
            foreach (Card card in cards)
            {
                this[card.Suit].Add(card);
            }

        }

    }
}