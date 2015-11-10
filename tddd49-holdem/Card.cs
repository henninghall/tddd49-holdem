namespace tddd49_holdem
{
    public class Card
    {
        private byte _color;
        private byte _value;
    
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