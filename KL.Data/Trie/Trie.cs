using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Add(query, 0, data);
        }
    }
}
