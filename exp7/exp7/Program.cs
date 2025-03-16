using System;
using System.Collections.Generic;

// Component Interface
public interface IProduct
{
    string GetDetails();
    double GetPrice();
}

// Concrete Component
public class Product : IProduct
{
    public string Name;
    public string Category;
    public double Price;

    public Product(string name, string category, double price)
    {
        Name = name;
        Category = category;
        Price = price;
    }

    public string GetDetails()
    {
        return $"Name: {Name}, Category: {Category}, Price: {Price}";
    }

    public double GetPrice()
    {
        return Price;
    }
}

// Decorator Interface
public abstract class ProductDecorator : IProduct
{
    protected IProduct product;

    public ProductDecorator(IProduct product)
    {
        this.product = product;
    }

    public virtual string GetDetails()
    {
        return product.GetDetails();
    }

    public virtual double GetPrice()
    {
        return product.GetPrice();
    }
}

// Concrete Decorator
public class DiscountDecorator : ProductDecorator
{
    private double discount;

    public DiscountDecorator(IProduct product) : base(product)
    {
        discount = GetDiscount(((Product)product).Category);
    }

    public override string GetDetails()
    {
        return base.GetDetails() + $", Discount: {discount}%, Discounted Price: {GetPrice()}";
    }

    public override double GetPrice()
    {
        return base.GetPrice() * (1 - discount / 100);
    }

    private double GetDiscount(string category)
    {
        return category.ToLower() switch
        {
            "electronics" => 10,
            "clothing" => 5,
            "furniture" => 15,
            _ => 0
        };
    }
}

// Main Class
class Program
{
    static void Main()
    {
        List<IProduct> products = new List<IProduct>
        {
            new Product("Laptop", "Electronics", 800),
            new Product("Phone", "Electronics", 500),
            new Product("Shirt", "Clothing", 20),
            new Product("Pants", "Clothing", 30),
            new Product("Table", "Furniture", 100)
        };

        // Display Products
        Console.WriteLine("Available Products:");
        for (int i = 0; i < products.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {((Product)products[i]).Name}");
        }

        // User Search
        Console.WriteLine("\nSearch Type:");
        Console.WriteLine("1. Search by Name");
        Console.WriteLine("2. Search by Category");
        Console.Write("Enter choice: ");
        int choice = int.Parse(Console.ReadLine());

        Console.Write("Enter search value: ");
        string searchValue = Console.ReadLine().ToLower();

        Console.WriteLine("\nSearch Results:");
        bool found = false;

        foreach (var product in products)
        {
            Product p = (Product)product;

            if ((choice == 1 && p.Name.ToLower().Contains(searchValue)) ||
                (choice == 2 && p.Category.ToLower().Contains(searchValue)))
            {
                found = true;

                // Automatically apply discount using Decorator
                IProduct discountedProduct = new DiscountDecorator(product);

                Console.WriteLine(discountedProduct.GetDetails());
            }
        }

        if (!found)
            Console.WriteLine("No products found.");
    }
}
