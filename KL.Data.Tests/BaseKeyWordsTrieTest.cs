using KL.Data.Trie;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace KL.Data.Tests
{
    public abstract class BaseKeyWordsTrieTest
    {
        protected ITrie<string, int?> Trie { get; private set; }

        [TestFixtureSetUp]
        public virtual void Setup()
        {
            Trie = CreateTrie();
            for (int i = 0; i < KeyWords.Length; i++)
            {
                Trie.Add(KeyWords[i], i);
            }
        }

        protected abstract ITrie<string, int?> CreateTrie();

        public string[][] KeyWords = new[] {
                                            new string[] {"hello", "world"},
                                            new string[] {"hello", "world", "!"},
                                            new string[] {"hello", "there", "world"},
                                            new string[] {"hi", "world"},
                                            new string[] {"hi", "world", "!"},
                                        };

        [TestCase(new string[] { "hello", "world" }, new[] { 0, 1 })]
        [TestCase(new string[] { "hello", "world", "!" }, new[] { 1 })]
        [TestCase(new string[] { "hello", "there", "world" }, new[] { 2 })]
        [TestCase(new string[] { "hi", "world" }, new[] { 3, 4 })]
        public void Test(string[] query, IEnumerable<int> expected)
        {
            Console.WriteLine(string.Join(",", query));
            IEnumerable<int?> actual = Trie.Retrieve(query);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
