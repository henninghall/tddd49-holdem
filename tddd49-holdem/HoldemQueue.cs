using System;
using System.Collections.Generic;

namespace tddd49_holdem
{
    public class HoldemQueue<T> : List<T>
    {
        public HoldemQueue()
        {

        }

        public HoldemQueue(IEnumerable<T> players)
        {
            AddRange(players);
        }

        public T Peek()
        {
            return this[GetFirstElementIndex()];
        }

        public void Enqueue(T t)
        {
            Add(t);
        }

        public T Dequeue()
        {
            T t = this[GetFirstElementIndex()];
            RemoveAt(GetFirstElementIndex());
            return t;
        }

        public Cards Dequeue(int cardsOnHand)
        {
            Cards cards = new Cards();
            for (int i = 0; i < cardsOnHand; i++)
            {
                cards.Add(Dequeue() as Card);
            }
            return cards;
        }

        private int GetFirstElementIndex()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] != null) return i;
            }
            throw new IndexOutOfRangeException();
        }
    }
}
