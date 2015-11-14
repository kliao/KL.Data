using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL.Data.Trie
{
    public class TrieNode<TLabel, TData> : TrieNodeBase<TLabel, TData>
    {
        private readonly Dictionary<TLabel, TrieNode<TLabel, TData>> children;
        private readonly Queue<TData> dataStore;

        protected TrieNode()
        {
            children = new Dictionary<TLabel, TrieNode<TLabel, TData>>();
            dataStore = new Queue<TData>();
        }

        public override IEnumerable<TData> Data
        {
            get { return dataStore; }
        }
        protected override IEnumerable<TrieNodeBase<TLabel, TData>> Children
        {
            get { return children.Values; }
        }

        protected override TrieNodeBase<TLabel, TData> GetOrCreateChild(TLabel key)
        {
            TrieNode<TLabel, TData> result;
            if (!children.TryGetValue(key, out result))
            {
                result = new TrieNode<TLabel, TData>();
                children.Add(key, result);
            }
            return result;
        }

        public override TrieNodeBase<TLabel, TData> GetChildOrNull(TLabel[] query, int position)
        {
            if (query == null) throw new ArgumentNullException("query");
            TrieNode<TLabel, TData> childNode;
            return children.TryGetValue(query[position], out childNode)
                ? childNode
                : null;
        }

        protected override void AddValue(TData data)
        {
            dataStore.Enqueue(data);
        }
    }

}
