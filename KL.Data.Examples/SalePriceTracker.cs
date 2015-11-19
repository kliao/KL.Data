using System;
using System.Collections.Generic;
using KL.Data.Trie;

namespace KL.Data.Examples
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

    public class ExcludeFromSale : ISalePrice
    {
        // get an "inverse" function from the class that we want to negate.
        // inspect the function chain and cancel out the 2 functions
    }

    public class SalePriceTracker
    {
        private FoldingTrie<string, ISalePrice> productTree;
        public SalePriceTracker()
        {
            productTree = new FoldingTrie<string, ISalePrice>();
        }

        public void Add(ISalePrice salePrice, string brandName, string productCategory, string productId)
        {
            List<string> key = new List<string> { brandName, productCategory, productId };
            key.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            productTree.Add(key.ToArray(), salePrice);
        }

        public IEnumerable<ISalePrice> Retrieve(string brandName, string productCategory, string productId)
        {
            return productTree.Retrieve(new[] { brandName, productCategory, productId });
        }

        public TResult Fold<TAccumulate, TResult>(string brandName, string productCategory, string productId,
            TAccumulate seed,
            Func<TAccumulate, ISalePrice, TAccumulate> folder,
            Func<TAccumulate, TResult> resultSelector)
        {
            return productTree.Fold(new[] { brandName, productCategory, productId }, seed,
                folder, resultSelector);
        }

    }
}
