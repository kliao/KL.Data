using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL.Data.Trie
{

    public interface IFoldingTrie<TLabel, TData> : ITrie<TLabel, TData>
    {
        TResult Fold<TAccumulate, TResult>(TLabel[] query, TAccumulate seed, Func<TAccumulate, TData, TAccumulate> folder, Func<TAccumulate, TResult> resultSelector);
    }

    public class FoldingTrie<TLabel, TData> : Trie<TLabel, TData>, IFoldingTrie<TLabel, TData>
    {
        public TResult Fold<TAccumulate, TResult>(TLabel[] query, 
            TAccumulate seed, 
            Func<TAccumulate, TData, TAccumulate> folder, 
            Func<TAccumulate, TResult> resultSelector)
        {
            int position = 0;
            var accumulate = seed;
            var child = (TrieNodeBase<TLabel, TData>)this;
            while (!EndOfLabels(position, query))
            {
                child = child.GetChildOrNull(query, position);
                if (child == null) break;

                TrieNode<TLabel, TData> nextNode = (TrieNode<TLabel, TData>)child;
                if (nextNode.Data.Any())
                    accumulate = folder(accumulate, nextNode.Data.First());
                position++;
            }
            return resultSelector(accumulate);
        }
    }

}
