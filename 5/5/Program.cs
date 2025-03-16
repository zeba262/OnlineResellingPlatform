using System;
public class Product {
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}
// Service interface
public interface IProductSearcher {
    void Search(Product[] products, string value);
}

// Concrete implementation for searching by name
public class SearchByName : IProductSearcher {
    public void Search(Product[] products, string name) {
        Console.WriteLine($"Searching for name: {name}");
        foreach (var product in products) {
            if (product.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
            }
        }
    }
}

// Concrete implementation for searching by category
public class SearchByCategory : IProductSearcher { 
    public void Search(Product[] products, string category) { 
        Console.WriteLine($"Searching for category: {category}");
        foreach (var product in products) {
            if (product.Category.Equals(category, StringComparison.OrdinalIgnoreCase)) {
                Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
            }
        }
    }
}

// ProductSearchHandler that receives dependency via constructor
public class ProductSearchHandler {
    private readonly IProductSearcher _productSearcher;
    public ProductSearchHandler(IProductSearcher productSearcher) {
        _productSearcher = productSearcher;
    }
    public void PerformSearch(Product[] products, string value) {
        _productSearcher.Search(products, value);
    }
}
class Program {
    static void Main() {
        Product[] products = {
            new Product { Name = "Phone", Category = "Electronics", Price = 8000 },
            new Product { Name = "Laptop", Category = "Electronics", Price = 25000 },
            new Product { Name = "Shoes", Category = "Clothing", Price = 5000 }
        };
        Console.WriteLine("Enter search type ('name' or 'category'):");
        string searchType = Console.ReadLine();
        Console.WriteLine($"Enter the {searchType} to search:");
        string searchValue = Console.ReadLine();

        // Dependency Injection: Inject the appropriate implementation
        IProductSearcher searcher = searchType.Equals("name", StringComparison.OrdinalIgnoreCase)
            ? new SearchByName()
            : new SearchByCategory();

        // Injecting dependency into ProductSearchHandler
        ProductSearchHandler searchHandler = new ProductSearchHandler(searcher);
        searchHandler.PerformSearch(products, searchValue);
    }
}
