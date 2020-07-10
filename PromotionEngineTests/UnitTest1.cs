using NUnit.Framework;
using PromotionEngine;
using System.Collections.Generic;

namespace PromotionEngineTests
{
    public class Tests
    {
        private List<ProductPrice> productFixedPrices;

        [OneTimeSetUp]
        public void Setup()
        {
            productFixedPrices = new List<ProductPrice>
            {
                new ProductPrice
                {
                    Name = "A",
                    FixedPrice = 50
                },
                new ProductPrice
                {
                    Name = "B",
                    FixedPrice = 30
                },
                new ProductPrice
                {
                    Name = "C",
                    FixedPrice = 20
                },
                new ProductPrice
                {
                    Name = "D",
                    FixedPrice = 15
                }
            };
        }

        [Test]
        public void GetProductTotal1()
        {
            List<Product> productsSelected = new List<Product>
            {
                new Product
                {
                    Name = "A",
                    Quantity = 3
                },
                new Product
                {
                    Name = "B",
                    Quantity = 5
                },
                new Product
                {
                    Name = "C",
                    Quantity = 1
                },
                new Product
                {
                    Name = "D",
                    Quantity = 1
                }
            };

            PromotionEngine.PromotionEngine promotionEngine = new PromotionEngine.PromotionEngine();
            promotionEngine.ReadPromotionsFromFile();
            long total = promotionEngine.CalculateTotal(productsSelected, productFixedPrices);
            Assert.AreEqual(280, total);
        }

        [Test]
        public void GetProductTotal2()
        {
            List<Product> productsSelected = new List<Product>
            {
                new Product
                {
                    Name = "A",
                    Quantity = 5
                },
                new Product
                {
                    Name = "B",
                    Quantity = 5
                },
                new Product
                {
                    Name = "C",
                    Quantity = 1
                },
                new Product
                {
                    Name = "D",
                    Quantity = 0
                }
            };

            PromotionEngine.PromotionEngine promotionEngine = new PromotionEngine.PromotionEngine();
            promotionEngine.ReadPromotionsFromFile();
            long total = promotionEngine.CalculateTotal(productsSelected, productFixedPrices);
            Assert.AreEqual(370, total);
        }

        [Test]
        public void GetProductTotal3()
        {
            List<Product> productsSelected = new List<Product>
            {
                new Product
                {
                    Name = "A",
                    Quantity = 1
                },
                new Product
                {
                    Name = "B",
                    Quantity = 1
                },
                new Product
                {
                    Name = "C",
                    Quantity = 1
                },
                new Product
                {
                    Name = "D",
                    Quantity = 0
                }
            };

            PromotionEngine.PromotionEngine promotionEngine = new PromotionEngine.PromotionEngine();
            promotionEngine.ReadPromotionsFromFile();
            long total = promotionEngine.CalculateTotal(productsSelected, productFixedPrices);
            Assert.AreEqual(100, total);
        }

    }
}