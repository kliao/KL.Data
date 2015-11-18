using System;
using System.Collections.Generic;

namespace KL.Data.Trie
{
    public interface ITrie<in TLabel, TData>
    {
        IEnumerable<TData> Retrieve(TLabel[] query);
        void Add(TLabel[] key, TData value);
        void Add(TLabel[] query, TData data, Func<TData, TData, TData> combineData);
    }
}
