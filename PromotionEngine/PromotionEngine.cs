using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromotionEngine
{
    public class PromotionEngine
    {
        private Dictionary<Tuple<string, string>, int> productPairs = new Dictionary<Tuple<string, string>, int>();
        private Dictionary<Tuple<int, string>, int> productPack = new Dictionary<Tuple<int, string>, int>();
        private long total = 0;
        private List<ProductPrice> productPrices = new List<ProductPrice>();

        public void ReadPromotionsFromFile()
        {
            string line;
            using (StreamReader streamReader = new StreamReader("promotion.txt"))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] items = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
                    if (items.Length == 2 && items[0].Contains("*"))
                    {
                        string[] itemSplit = items[0].Split("*", StringSplitOptions.RemoveEmptyEntries);
                        productPack.Add(new Tuple<int, string>(int.Parse(itemSplit[0]), itemSplit[1]), int.Parse(items[1]));
                    }
                    else if (items.Length == 2 && items[0].Contains("+"))
                    {
                        string[] itemSplit = items[0].Split("+", StringSplitOptions.RemoveEmptyEntries);
                        productPairs.Add(new Tuple<string, string>(itemSplit[0], itemSplit[1]), int.Parse(items[1]));
                    }
                }
            }
        }

        public long CalculateTotal(List<Product> products, List<ProductPrice> allProductPrices)
        {
            productPrices = allProductPrices;
            List<Tuple<string, string>> pairs = productPairs.Keys?.ToList();
            if (pairs != null && pairs.Count > 0)
            {
                pairs.ForEach(x =>
                {
                    CalculateSumForPairProduct(products, x);
                    products.Where(y => y.Name == x.Item1).ToList().ForEach(y => y.IsProcessed = true);
                    Console.WriteLine($"Product {x.Item1} processed");
                    products.Where(y => y.Name == x.Item2).ToList().ForEach(y => y.IsProcessed = true);
                    Console.WriteLine($"Product {x.Item2} processed");
                });
            }

            List<Tuple<int, string>> pack = productPack.Keys?.ToList();
            if (pack != null && pack.Count > 0)
            {
                pack.ForEach(x =>
                {
                    CalculateSumForPackProduct(products, x);
                    products.Where(y => y.Name == x.Item2).ToList().ForEach(y => y.IsProcessed = true);
                    Console.WriteLine($"Product {x.Item2} processed");
                });
            }

            List<Product> productsUnprocessedByPromotion = products.Where(x => x.IsProcessed.Equals(false)).ToList();
            productsUnprocessedByPromotion.ForEach(x =>
            {
                int fixedPrice = productPrices.Where(y => y.Name == x.Name).Select(y => y.FixedPrice).FirstOrDefault();
                total += (fixedPrice * x.Quantity);
                Console.WriteLine($"Product {x.Name} processed");
            });
            return total;
        }

        private void CalculateSumForPackProduct(List<Product> products, Tuple<int, string> pack)
        {
            int productQuantity = products.Where(y => y.Name == pack.Item2 && y.Quantity >= 1).Select(y => y.Quantity).FirstOrDefault();
            int fixedPrice = productPrices.Where(x => x.Name == pack.Item2).Select(x => x.FixedPrice).FirstOrDefault();
            if (productQuantity >= pack.Item1)
            {
                int discountedProductQuantity = Math.DivRem(productQuantity, pack.Item1, out int rem);
                productPack.TryGetValue(pack, out int value);
                total += (discountedProductQuantity * value);
                if (rem != 0)
                {
                    total += (rem * fixedPrice);
                }
            }
            else if (productQuantity > 0 && productQuantity < pack.Item1)
            {
                total += (productQuantity * fixedPrice);
            }
        }

        private void CalculateSumForPairProduct(List<Product> products, Tuple<string, string> pair)
        {
            Product pairProduct1 = products.Where(y => y.Name == pair.Item1 && y.Quantity >= 1).FirstOrDefault();
            Product pairProduct2 = products.Where(y => y.Name == pair.Item2 && y.Quantity >= 1).FirstOrDefault();
            if (pairProduct1 != null && pairProduct2 != null)
            {
                productPairs.TryGetValue(pair, out int value);
                total += value;
                products.Where(y => y.Name == pair.Item1).ToList().ForEach(y => y.Quantity -= 1);
                products.Where(y => y.Name == pair.Item2).ToList().ForEach(y => y.Quantity -= 1);
                CalculateSumForPairProduct(products, pair);
            }
            else if (pairProduct1 != null)
            {
                int productLeft = products.Where(y => y.Name == pair.Item1 && y.Quantity >= 1).Select(y => y.Quantity).FirstOrDefault();
                int productPrice = productPrices.Where(x => x.Name == pair.Item1).Select(x => x.FixedPrice).FirstOrDefault();
                total += (productLeft * productPrice);
                products.Where(y => y.Name == pair.Item1).ToList().ForEach(y => y.Quantity -= productLeft);
            }
            else if (pairProduct2 != null)
            {
                int productLeft = products.Where(y => y.Name == pair.Item2 && y.Quantity >= 1).Select(y => y.Quantity).FirstOrDefault();
                int productPrice = productPrices.Where(x => x.Name == pair.Item2).Select(x => x.FixedPrice).FirstOrDefault();
                total += (productLeft * productPrice);
                products.Where(y => y.Name == pair.Item2).ToList().ForEach(y => y.Quantity -= productLeft);
            }
        }
    }
}
