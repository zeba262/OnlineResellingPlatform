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
            new Product { Name = "Phone", Category = "Electronics", Price = 26000 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 12000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 5600 }
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