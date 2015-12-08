using System;
using System.Collections.Generic;

namespace tddd49_holdem
{
    public class HoldemQueue<T> : List<T>
    {

        public HoldemQueue() {
            
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