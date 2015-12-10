namespace tddd49_holdem
{
    public class IntegerObject
    {
        public int IntegerObjectId { get; set; }
        public int TheInt { get; set; }

        public IntegerObject() { }

        public IntegerObject(int theint)
        {
            TheInt = theint;
        }
    }
}