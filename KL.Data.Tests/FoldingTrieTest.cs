using KL.Data.Trie;
using NUnit.Framework;
using System;

namespace KL.Data.Tests
{
    public class FoldingTrieTest : BaseKeyWordsTrieTest
    {
        protected override ITrie<string, int?> CreateTrie()
        {
            return new FoldingTrie<string, int?>();
        }

        [TestCase(new [] { "hello", "world" }, 0)]
        [TestCase(new [] { "hello", "world", "!" }, 1)]
        [TestCase(new [] { "hello", "there", "world" }, 2)]
        [TestCase(new [] { "hi", "world" }, 3)]
        [TestCase(new [] { "hi", "world", "!" }, 7)]
        public void FoldAddTest(string[] query, int expected)
        {
            TestFunction<int, int?, int>(
                expected, 
                query, 
                0, 
                (acc, data) => data.HasValue ? acc + data.Value : acc, 
                x => x);
        }

        [TestCase(new [] { "hello", "world" }, 0)]
        [TestCase(new [] { "hello", "world", "!" }, 0)]
        [TestCase(new [] { "hello", "there", "world" }, 2)]
        [TestCase(new [] { "hi", "world" }, 3)]
        [TestCase(new [] { "hi", "world", "!" }, 12)]
        public void FoldMultiplicationTest(string[] query, int expected)
        {
            TestFunction<int, int?, int>(
                expected, 
                query, 
                1, 
                (acc, data) => data.HasValue ? acc * data.Value : acc, 
                x => x);
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
            TResult actual = foldingTree.Fold(query, seed, folder, resultSelector);
            Assert.AreEqual(expected, actual);
        }
    }
}
