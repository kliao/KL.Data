using System;
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
                ITrie<char,int> trie = CreateTrie();
                LongPhrases40
                    .AsParallel()
                    .ForAll(phrase => trie.Add(phrase.ToCharArray(), phrase.GetHashCode()));
            }
        }

        protected override ITrie<char, int> CreateTrie()
        {
            return new Trie<char,int>();
        }
    }
    public class KeyWordsTrieTest : BaseKeyWordsTrieTest
    {
        protected override ITrie<string, int> CreateTrie()
        {
            return new Trie<string, int>();
        }
    }
}
