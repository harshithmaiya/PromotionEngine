using System;
using System.Collections.Generic;

namespace PromotionEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Code to take user inputs for the products and its prices to be added here and logic to calculate
            //the order total based on the promotion types to be called from here
            int qty = 1;
            List<ProductPrice> productPrices = new List<ProductPrice>();
            List<Product> products = new List<Product>();

            Console.WriteLine("Enter the number of products");
            int productQty = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the products fixed price");
            while (qty <= productQty)
            {
                Console.WriteLine("Enter the product name. Please make sure to use the same product names as used in promotion.txt");
                string name = Console.ReadLine();
                Console.WriteLine("Enter the product fixed price");
                int price = int.Parse(Console.ReadLine());
                productPrices.Add(new ProductPrice
                {
                    Name = name,
                    FixedPrice = price
                });
                qty++;
            }

            Console.WriteLine("Enter the products to be purchased with quantity");
            qty = 1;
            while (qty <= productQty)
            {
                Console.WriteLine("Enter the product name");
                string name = Console.ReadLine();
                Console.WriteLine("Enter the desired product qty to purchase");
                int prodQtyToPurchase = int.Parse(Console.ReadLine());
                products.Add(new Product
                {
                    Name = name,
                    Quantity = prodQtyToPurchase
                });
                qty++;
            }

            PromotionEngine promotionEngine = new PromotionEngine();
            promotionEngine.ReadPromotionsFromFile();
            long total  = promotionEngine.CalculateTotal(products, productPrices);
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Total price after applying promotion: {total}");
            Console.WriteLine("--------------------------------------");
            Console.Read();
        }
    }
}
