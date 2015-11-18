using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KL.Data.Trie;

namespace KL.Data.Tests
{
    public class TrieTests : BaseTrieTest
    {
        [Test]
        [ExpectedException(typeof(AggregateException))]
        [Explicit]
        public void ExhaustiveParallelAddFails()
        {
            while (true)
            {
                ITrie<char, IEnumerable<int>> trie = CreateTrie();
                var q = new Queue<int>();
                q.Enqueue(LongPhrases40.GetHashCode());
                LongPhrases40
                    .AsParallel()
                    .ForAll(phrase => trie.Add(phrase.ToCharArray(), q));
            }
        }

        protected override ITrie<char, IEnumerable<int>> CreateTrie()
        {
            return new Trie<char, IEnumerable<int>>();
        }
    }
    public class KeyWordsTrieTest : BaseKeyWordsTrieTest
    {
        protected override ITrie<string, int?> CreateTrie()
        {
            return new Trie<string, int?>();
        }
    }
}
