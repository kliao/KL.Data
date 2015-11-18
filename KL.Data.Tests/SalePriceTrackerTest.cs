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
        
        protected ProductKey[] ProductKeys =
        {
            new ProductKey { BrandName = "levis", ProductCategory = "jeans", ProductId = "1234"}
        };

        [TestCase("levis:jeans:1234", 50, 40)]
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