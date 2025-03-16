using System;
public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}
public class ProductSearcher
{
    public virtual void Search(Product[] products, string value)
    {
        Console.WriteLine("Base search method. Override this in derived classes.");
    }
}
public class ProductPriceSearcher
{
    public virtual void Search(Product[] products, decimal min, decimal max)
    {
        Console.WriteLine("Base search method. Override this in derived classes.");
    }
}
public class SearchByName : ProductSearcher
{
    public override void Search(Product[] products, string name)
    {
        Console.WriteLine($"Searching for name: {name}");
        foreach (var product in products)
        {
            if (product.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
            }
        }
    }
}
public class SearchByCategory : ProductSearcher
{
    public override void Search(Product[] products, string category)
    {
        Console.WriteLine($"Searching for category: {category}");
        foreach (var product in products)
        {
            if (product.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
            }
        }
    }
}
public class SearchByPrice : ProductPriceSearcher
{
    public override void Search(Product[] products, decimal min, decimal max)
    {
        Console.WriteLine($"Searching for products with price between {min} and {max}");
        foreach (var product in products)
        {
            if (product.Price >= min && product.Price <= max)
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
            }
        }
    }
}
class Program
{
    static void Main()
    {
        Product[] products = {
            new Product { Name = "Phone", Category = "Electronics", Price = 8000 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 22000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 5000 }
        };
        Console.WriteLine("Enter search type ('name', 'category' or 'price'):");
        string searchType = Console.ReadLine();
        if (searchType.Equals("price", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter minimum price:");
            decimal min = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter maximum price:");
            decimal max = Convert.ToDecimal(Console.ReadLine());
            ProductPriceSearcher priceSearcher = new SearchByPrice();
            priceSearcher.Search(products, min, max);
        }
        else
        {
            Console.WriteLine($"Enter the {searchType} to search:");
            string searchValue = Console.ReadLine();
            ProductSearcher searcher = searchType.Equals("name", StringComparison.OrdinalIgnoreCase) ? new SearchByName() : new SearchByCategory();
            searcher.Search(products, searchValue);
        }
    }
}
