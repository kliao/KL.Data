using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL.Data.Trie
{
    public interface ITrie<TLabel, TData>
    {
        IEnumerable<TData> Retrieve(TLabel[] query);
        void Add(TLabel[] key, TData value);
    }
}
