using System.Collections.Generic;

namespace Darkness.Collections
{
    // TODO: refactor PriorityQueue
    public class PriorityQueue<T>
    {
        private List<KeyValuePair<T, double>> items = new List<KeyValuePair<T, double>>();

        public int Count => items.Count;

        public void Enqueue(T item, double priority)
        {
            items.Add(new KeyValuePair<T, double>(item, priority));
        }

        public bool TryDequeue(out T item)
        {
            item = default(T);
            if (items.Count == 0)
            {
                return false;
            }

            var bestIndex = 0;
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Value < items[bestIndex].Value)
                {
                    bestIndex = i;
                }
            }

            item = items[bestIndex].Key;
            items.RemoveAt(bestIndex);
            return true;
        }
    }
}
