using System;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}

public abstract class ProductSearcher
{
    public abstract void Search(Product[] products, string value);
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

class Program
{
    static void Main()
    {
        Product[] products = {
            new Product { Name = "Phone", Category = "Electronics", Price = 8000 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 30000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 6000 }
        };

        Console.WriteLine("Enter search type ('name' or 'category'):");
        string searchType = Console.ReadLine();
        Console.WriteLine($"Enter the {searchType} to search:");
        string searchValue = Console.ReadLine();

        ProductSearcher searcher = searchType.Equals("name", StringComparison.OrdinalIgnoreCase)
            ? new SearchByName() as ProductSearcher
            : new SearchByCategory();

        searcher.Search(products, searchValue);
    }
}
