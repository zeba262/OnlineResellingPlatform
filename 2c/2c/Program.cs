using System;
public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}

public abstract class ProductViewer
{
    public abstract void DisplayProducts(Product[] products);
}

public abstract class ProductSearcher
{
    public abstract void SearchByName(Product[] products, string name);
    public abstract void SearchByCategory(Product[] products, string category);
    public abstract void SearchByPrice(Product[] products, decimal minPrice, decimal maxPrice);
}

public class ViewProducts : ProductViewer
{
    public override void DisplayProducts(Product[] products)
    {
        Console.WriteLine("Product List:");
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
        }
    }
}

public class SearchProducts : ProductSearcher
{
    public override void SearchByName(Product[] products, string name)
    {
        Console.WriteLine($"Searching for name: {name}");
        foreach (var product in products)
        {
            if (product.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
            }
        }
    }

    public override void SearchByCategory(Product[] products, string category)
    {
        Console.WriteLine($"Searching for category: {category}");
        foreach (var product in products)
        {
            if (product.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
            }
        }
    }

    public override void SearchByPrice(Product[] products, decimal minPrice, decimal maxPrice)
    {
        Console.WriteLine($"Searching for products with price between {minPrice:C} and {maxPrice:C}");
        foreach (var product in products)
        {
            if (product.Price >= minPrice && product.Price <= maxPrice)
            {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price:C}");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Product[] products = {
            new Product { Name = "Phone", Category = "Electronics", Price = 500 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 1000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 50 }
        };

        ProductViewer viewer = new ViewProducts();
        viewer.DisplayProducts(products);
        Console.WriteLine("Enter search type ('name', 'category', or 'price'):");
        string searchType = Console.ReadLine();
        ProductSearcher searcher = new SearchProducts();

        if (searchType.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter the name to search:");
            string searchValue = Console.ReadLine();
            searcher.SearchByName(products, searchValue);
        }
        else if (searchType.Equals("category", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter the category to search:");
            string searchValue = Console.ReadLine();
            searcher.SearchByCategory(products, searchValue);
        }
        else if (searchType.Equals("price", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter the minimum price:");
            decimal minPrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter the maximum price:");
            decimal maxPrice = decimal.Parse(Console.ReadLine());
            searcher.SearchByPrice(products, minPrice, maxPrice);
        }
        else
        {
            Console.WriteLine("Invalid search type.");
        }
    }
}
