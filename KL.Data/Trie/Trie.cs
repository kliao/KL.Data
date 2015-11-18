using System;
using System.Collections.Generic;

namespace KL.Data.Trie
{
    public class Trie<TLabel, TData> : TrieNode<TLabel, TData>, ITrie<TLabel, TData>
    {
        public IEnumerable<TData> Retrieve(TLabel[] query)
        {
            return Retrieve(query, 0);
        }

        public void Add(TLabel[] query, TData data)
        {
            Add(query, data, (existingValue, insertValue) => insertValue);
        }

        public void Add(TLabel[] query, TData data, Func<TData, TData, TData> combineData)
        {
            Add(query, 0, data, combineData);
        }
    }
}
