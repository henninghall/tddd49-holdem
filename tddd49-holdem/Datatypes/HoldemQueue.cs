using System;
using System.Collections.Generic;

namespace tddd49_holdem
{
    /// <summary>
    /// The HoldemQueue class is used as a regular queue.
    /// The regular queue is not supported in EntityFramwork 6
    /// but this class does since it derives from a List. 
    /// </summary>
    public class HoldemQueue<T> : List<T>
    {

        public HoldemQueue(){}

        public HoldemQueue(IEnumerable<T> items)
        {
            AddRange(items);
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