using System;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}

public interface ISearchByName
{
    void Search(Product[] products, string name);
}
public interface ISearchByCategory
{
    void Search(Product[] products, string category);
}
public class SearchByName : ISearchByName
{
    public void Search(Product[] products, string name)
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
public class SearchByCategory : ISearchByCategory
{
    public void Search(Product[] products, string category)
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
            new Product { Name = "Laptop", Category = "Electronics", Price = 25000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 5000 }
        };

        Console.WriteLine("Enter search type ('name' or 'category'):");
        string searchType = Console.ReadLine();
        Console.WriteLine($"Enter the {searchType} to search:");
        string searchValue = Console.ReadLine();

        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            ISearchByName searcher = new SearchByName();
            searcher.Search(products, searchValue);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            ISearchByCategory searcher = new SearchByCategory();
            searcher.Search(products, searchValue);
        }
    }
}