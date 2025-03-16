using System;

// Product Class
public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public double Price { get; set; }
}

// Abstract Product Interface
public abstract class ProductSearcher
{
    public abstract void Search(Product[] products, string value);
}

// Concrete Products
public class SearchByName : ProductSearcher
{
    public override void Search(Product[] products, string name)
    {
        Console.WriteLine($"Searching for Name: {name}");
        foreach (var product in products)
        {
            if (product.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: ${product.Price}");
            }
        }
    }
}

public class SearchByCategory : ProductSearcher
{
    public override void Search(Product[] products, string category)
    {
        Console.WriteLine($"Searching for Category: {category}");
        foreach (var product in products)
        {
            if (product.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: ${product.Price}");
            }
        }
    }
}

// Factory Interface
public interface ProductFactory
{
    ProductSearcher CreateSearcher();
}

// Concrete Factories
public class NameSearcherFactory : ProductFactory
{
    public ProductSearcher CreateSearcher()
    {
        return new SearchByName();
    }
}

public class CategorySearcherFactory : ProductFactory
{
    public ProductSearcher CreateSearcher()
    {
        return new SearchByCategory();
    }
}

// Client Class
public class Client
{
    private ProductSearcher searcher;

    public Client(ProductFactory factory)
    {
        searcher = factory.CreateSearcher();
    }

    public void Search(Product[] products, string value)
    {
        searcher.Search(products, value);
    }
}

// Driver Code
class Program
{
    static void Main()
    {
        Product[] products = {
            new Product { Name = "Phone", Category = "Electronics", Price = 500 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 1000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 50 },
            new Product { Name = "T-shirt", Category = "Clothing", Price = 20 },
            new Product { Name = "Watch", Category = "Accessories", Price = 150 }
        };

        Console.WriteLine("Available Products:");
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}");
        }

        Console.WriteLine("\nSelect search type:");
        Console.WriteLine("1. Search by Name");
        Console.WriteLine("2. Search by Category");

        int choice = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter search value:");
        string value = Console.ReadLine();

        ProductFactory factory = null;
        if (choice == 1)
        {
            factory = new NameSearcherFactory();
        }
        else if (choice == 2)
        {
            factory = new CategorySearcherFactory();
        }

        if (factory != null)
        {
            Client client = new Client(factory);
            client.Search(products, value);
        }
        else
        {
            Console.WriteLine("Invalid choice");
        }
    }
}
