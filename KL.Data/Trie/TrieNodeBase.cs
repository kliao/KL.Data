using System;
using System.Collections.Generic;
using System.Linq;

namespace KL.Data.Trie
{
    public abstract class TrieNodeBase<TLabel, TData>
    {
        public abstract TData Data { get; }
        protected abstract IEnumerable<TrieNodeBase<TLabel, TData>> Children { get; }

        public void Add(TLabel[] key, int position, TData value, Func<TData,TData,TData> combineData = null)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (EndOfLabels(position, key))
            {
                AddValue(value, combineData);
                return;
            }
            TrieNodeBase<TLabel, TData> child = GetOrCreateChild(key[position]);
            child.Add(key, position + 1, value, combineData);
        }

        protected abstract void AddValue(TData data, Func<TData, TData, TData> combineData);

        public static bool EndOfLabels(int position, TLabel[] key)
        {
            return position >= key.Length;
        }

        protected abstract TrieNodeBase<TLabel, TData> GetOrCreateChild(TLabel label);

        protected virtual IEnumerable<TData> Retrieve(TLabel[] query, int position)
        {
            return EndOfLabels(position, query)
                ? ValuesDeep()
                : SearchDeep(query, position);
        }

        protected virtual IEnumerable<TData> SearchDeep(TLabel[] query, int position)
        {
            TrieNodeBase<TLabel, TData> nextNode = GetChildOrNull(query, position);
            return nextNode != null
                ? nextNode.Retrieve(query, position + 1)
                : Enumerable.Empty<TData>();
        }

        public abstract TrieNodeBase<TLabel, TData> GetChildOrNull(TLabel[] query, int position);

        private IEnumerable<TData> ValuesDeep()
        {
            return Subtree().Select(node => node.Data);
        }

        protected IEnumerable<TrieNodeBase<TLabel, TData>> Subtree()
        {
            return Enumerable.Repeat(this, 1)
                .Concat(Children.SelectMany(child => child.Subtree()));
        }
    }

}
