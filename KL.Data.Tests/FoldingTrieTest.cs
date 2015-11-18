using KL.Data.Trie;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace KL.Data.Tests
{
    public class ProductLookup
    {
        public string Id { get; private set; }
        public string BrandName { get; private set; }
        public string Category { get; private set; }
    }

    public interface ISalePrice
    {
        double GetSalePrice(double originalPrice);
    }

    public class PctSale : ISalePrice
    {
        private readonly double pctSaleAmt;
        public PctSale(double pctSaleAmt)
        {
            this.pctSaleAmt = pctSaleAmt;
        }

        public double GetSalePrice(double originalPrice)
        {
            return pctSaleAmt * originalPrice;
        }
    }

    public class SalePriceTracker
    {
        private FoldingTrie<string,ISalePrice> productTree;
        public SalePriceTracker()
        {
            productTree = new FoldingTrie<string,ISalePrice>();
        }

        public void Add(ISalePrice salePrice, string brandName, string productCategory, string productId)
        {
            string[] key = new string[] { brandName, productCategory, productId };
            productTree.Add(key, salePrice);
        }

        public IEnumerable<ISalePrice> Retrieve(string brandName, string productCategory, string productId)
        {
            return productTree.Retrieve(new string[] { brandName, productCategory, productId });
        }

        public TResult Fold<TAccumulate, TResult>(string brandName, string productCategory, string productId, 
            TAccumulate seed,
            Func<TAccumulate, ISalePrice, TAccumulate> folder,
            Func<TAccumulate, TResult> resultSelector)
        {
            return productTree.Fold<TAccumulate, TResult>(new string[] { brandName, productCategory, productId }, seed,
                folder, resultSelector);
        }

    }

    public class SalesPriceTest
    {
        protected SalePriceTracker tracker { get; private set; }

        [OneTimeSetUpAttribute]
        public virtual void Setup()
        {
            tracker = new SalePriceTracker();
            for (int i = 0; i < ProductKeys.Length; i++)
            {
                var productKey = ProductKeys[i];
                tracker.Add(new PctSale(0.8), productKey.BrandName, productKey.ProductCategory, productKey.ProductId);
            }
        }

        public struct ProductKey
        {
            public string BrandName;
            public string ProductCategory;
            public string ProductId;
        }
        
        protected ProductKey[] ProductKeys = new ProductKey[] {
                                            new ProductKey { BrandName = "levis", ProductCategory = "jeans", ProductId = "1234"}
                                        };

        [TestCase("levis:jeans:1234", 50, 40)]
        public void Test(string productCode, double originalPrice, double expectedSalePrice)
        {
            //Console.WriteLine(string.Join(",", productKey));
            var productKey = GetProductKey(productCode);
            //IEnumerable<ISalePrice> actual = tracker.Retrieve(productKey.BrandName, productKey.ProductCategory, productKey.ProductId);
            var actual = tracker.Fold<double, double>(productKey.BrandName, productKey.ProductCategory, productKey.ProductId, originalPrice, 
                (acc, data) => data.GetSalePrice(acc), x => x);
            Assert.AreEqual(expectedSalePrice, actual);
        }

        private ProductKey GetProductKey(string code)
        {
            string[] codeArr = code.Split(':');
            switch (codeArr.Length)
            {
                case 0: return new ProductKey();
                case 1: return new ProductKey { BrandName = codeArr[0] };
                case 2: return new ProductKey { BrandName = codeArr[0], ProductCategory = codeArr[1] };
                case 3: return new ProductKey { BrandName = codeArr[0], ProductCategory = codeArr[1], ProductId = codeArr[2] };
                default: throw new InvalidOperationException();
            }
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
        [TestCase(new string[] { "hi", "world", "!" }, 7)]
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
            TestFunction<int, int, int>(expected, query, 1, (acc, data) => acc * data, x => x);
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
