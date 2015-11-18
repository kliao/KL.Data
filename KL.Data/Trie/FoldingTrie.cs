using System;

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

                var nextNode = (TrieNode<TLabel, TData>)child;
                accumulate = folder(accumulate, nextNode.Data);
                position++;
            }
            return resultSelector(accumulate);
        }
    }

}
