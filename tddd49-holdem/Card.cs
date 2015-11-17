namespace cs_holdem
{
    public class Card
    {
        private readonly byte _color;
        private readonly byte _value;
    
        public Card(byte color, byte value)
        {
            _color = color;
            _value = value;
        }

        public override string ToString()
        {
            return "Card: " + _color + " " + _value;
        }
    }
}