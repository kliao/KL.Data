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

    public class FoldingTrieTest : BaseKeyWordsTrieTest
    {

        protected override ITrie<string, int> CreateTrie()
        {
            return new FoldingTrie<string, int>();
        }

        [TestCase(new string[] { "hello", "world" }, 0)]
        [TestCase(new string[] { "hello", "world", "!" }, 1)]
        [TestCase(new string[] { "hello", "there", "world" }, 2)]
        [TestCase(new string[] { "hi", "world" }, 3)]
        [TestCase(new string[] { "hi", "world", "!" }, 7 )]
        public void FoldAddTest(string[] query, int expected)
        {
            TestFunction<int, int, int>(expected, query, 0, (acc, data) => acc + data, x => x);
        }

        [TestCase(new string[] { "hello", "world" }, 0)]
        [TestCase(new string[] { "hello", "world", "!" }, 0)]
        [TestCase(new string[] { "hello", "there", "world" }, 2)]
        [TestCase(new string[] { "hi", "world" }, 3)]
        [TestCase(new string[] { "hi", "world", "!" }, 12)]
        public void FoldMultiplicationTest(string[] query, int expected)
        {
            TestFunction<int, int, int>(expected, query, 1, (acc, data) => acc * Math.Max(data, 1), x => x);
        }

        private void TestFunction<TAccumulate, TData, TResult>(
            TResult expected,
            string[] query,
            TAccumulate seed, 
            Func<TAccumulate, TData, TAccumulate> folder, 
            Func<TAccumulate, TResult> resultSelector)
        {
            Console.WriteLine(string.Join(",", query));
            IFoldingTrie<string, TData> foldingTree = (FoldingTrie<string, TData>)Trie;
            TResult actual = foldingTree.Fold<TAccumulate, TResult>(query, seed, folder, resultSelector);
            NUnit.Framework.Assert.AreEqual(expected, actual);
        }
    }


}
