using System;
using KL.Data.Examples;
using NUnit.Framework;

namespace KL.Data.Tests
{
    public class SalePriceTrackerTest
    {
        protected SalePriceTracker tracker { get; private set; }

        [TestFixtureSetUp]
        public virtual void Setup()
        {
            tracker = new SalePriceTracker();
            for (int i = 0; i < ProductKeys.Length; i++)
            {
                var productKey = ProductKeys[i].Item1;
                tracker.Add(ProductKeys[i].Item2, productKey.BrandName, productKey.ProductCategory, productKey.ProductId);
            }
        }

        public struct ProductKey
        {
            public string BrandName;
            public string ProductCategory;
            public string ProductId ;
        }
        
        protected Tuple<ProductKey,ISalePrice>[] ProductKeys =
        {
            Tuple.Create(new ProductKey { BrandName = "levis", ProductCategory = "jeans", ProductId = "1234"}, new PctSale(0.8)),
            Tuple.Create(new ProductKey { BrandName = "levis", ProductCategory = "jeans" }, new PctSale(0.8)),
            Tuple.Create(new ProductKey { BrandName = "levis", ProductCategory = "jeans", ProductId = "2222" }, new ExcludeFromSale()),
        };

        [TestCase("levis:jeans:1111", 50, 40)]
        [TestCase("levis:jeans:1234", 50, 32)]
        [TestCase("levis:jeans:2222", 150, 150)]
        [TestCase("nudie:jeans:1111", 100, 100)]
        public void Test(string productCode, double originalPrice, double expectedSalePrice)
        {
            //Console.WriteLine(string.Join(",", productKey));
            var productKey = GetProductKey(productCode);
            //IEnumerable<ISalePrice> actual = tracker.Retrieve(productKey.BrandName, productKey.ProductCategory, productKey.ProductId);
            var actual = tracker.Fold(productKey.BrandName, productKey.ProductCategory, productKey.ProductId, originalPrice, 
                (acc, data) => data == null ? acc : data.GetSalePrice(acc), x => x);
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
}