using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL.Data.Trie
{
    public abstract class TrieNodeBase<TLabel, TData>
    {
        public abstract IEnumerable<TData> Data { get; }
        protected abstract IEnumerable<TrieNodeBase<TLabel, TData>> Children { get; }
        public void Add(TLabel[] key, int position, TData value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (EndOfLabels(position, key))
            {
                AddValue(value);
                return;
            }
            TrieNodeBase<TLabel, TData> child = GetOrCreateChild(key[position]);
            child.Add(key, position + 1, value);
        }
        protected abstract void AddValue(TData data);

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
            return Subtree().SelectMany(node => node.Data);
        }

        protected IEnumerable<TrieNodeBase<TLabel, TData>> Subtree()
        {
            return Enumerable.Repeat(this, 1)
                .Concat(Children.SelectMany(child => child.Subtree()));
        }
    }

}
