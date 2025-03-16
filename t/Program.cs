using System;
using System.Collections.Generic;
using System.Linq;

// Models
public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // "1" for Seller, "2" for Buyer
    public string SubscriptionType { get; set; } // "Basic", "Premium", or null
    public string ContactNumber { get; set; } // Seller's contact number

    public User(string username, string password, string role, string contactNumber = null)
    {
        Username = username;
        Password = password;
        Role = role;
        SubscriptionType = null; // Default to no subscription
        ContactNumber = contactNumber; // Seller's contact number
    }
}

public class Product
{
    private static int _nextId = 1;
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string Category { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string Description { get; set; }
    public string Owner { get; set; }
    public List<string> Reviews { get; set; } = new List<string>();
    public double Rating { get; set; } = 0;
    public int Quantity { get; set; } // Quantity of the product
    public bool IsSoldOut { get; set; } // Indicates if the product is sold out

    public Product(string name, string model, string category, decimal originalPrice, decimal discountedPrice, string description, string owner, int quantity)
    {
        Id = _nextId++;
        Name = name;
        Model = model;
        Category = category;
        OriginalPrice = originalPrice;
        DiscountedPrice = discountedPrice;
        Description = description;
        Owner = owner;
        Quantity = quantity;
        IsSoldOut = quantity <= 0; // Mark as sold out if quantity is zero or less
    }
}

// Order Class
public class Order
{
    private static int _nextId = 1;
    public int OrderId { get; private set; }
    public string BuyerUsername { get; set; }
    public int ProductId { get; set; }
    public string Status { get; set; } // "Placed", "Shipped", "Delivered", "Cancelled"
    public DateTime OrderDate { get; set; }

    public Order(string buyerUsername, int productId)
    {
        OrderId = _nextId++;
        BuyerUsername = buyerUsername;
        ProductId = productId;
        Status = "Placed";
        OrderDate = DateTime.Now;
    }
}

// Seller Feedback on Software
public class SellerFeedback
{
    public string SellerUsername { get; set; }
    public string Feedback { get; set; }
    public int Rating { get; set; } // Rating out of 5

    public SellerFeedback(string sellerUsername, string feedback, int rating)
    {
        SellerUsername = sellerUsername;
        Feedback = feedback;
        Rating = rating;
    }
}

// Buyer Feedback on Products
public class BuyerFeedback
{
    public string BuyerUsername { get; set; }
    public int ProductId { get; set; }
    public string Feedback { get; set; }
    public int Rating { get; set; } // Rating out of 5

    public BuyerFeedback(string buyerUsername, int productId, string feedback, int rating)
    {
        BuyerUsername = buyerUsername;
        ProductId = productId;
        Feedback = feedback;
        Rating = rating;
    }
}

// Interfaces for SOLID
public interface ISellerAction
{
    void SExecute(); // Unique method name for Seller
}

public interface IBuyerAction
{
    void BExecute(); // Unique method name for Buyer
}

public interface IAdminAction
{
    void AExecute(); // Unique method name for Admin
}

public interface IOrderAction
{
    void Execute(); // Common method for order-related actions
}

public interface ISearchAction
{
    void Execute(List<Product> products); // Common method for search-related actions
}

// View Products Class
public class ViewProducts : ISellerAction, IBuyerAction, IAdminAction
{
    private List<Product> products;
    private List<User> users;

    public ViewProducts(List<Product> products, List<User> users)
    {
        this.products = products;
        this.users = users;
    }

    public void SExecute() => DisplayAvailableProducts();
    public void BExecute() => DisplayAvailableProducts();
    public void AExecute() => DisplayAvailableProducts();

    private void DisplayAvailableProducts()
    {
        Console.WriteLine("\nAvailable Products:");
        foreach (var product in products)
        {
            if (!product.IsSoldOut)
            {
                var seller = users.FirstOrDefault(u => u.Username == product.Owner);
                DisplayProductDetails(product, seller);
            }
            else
            {
                Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} (Sold Out)");
            }
        }
    }

    private void DisplayProductDetails(Product product, User seller)
    {
        Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} , Model:{product.Model} , Category: {product.Category} , {product.DiscountedPrice}rs ,  Sold By: {product.Owner}, Contact: {seller?.ContactNumber}");
        Console.WriteLine($"Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
        DisplayProductReviews(product);
    }

    private void DisplayProductReviews(Product product)
    {
        foreach (var review in product.Reviews)
        {
            Console.WriteLine($"- {review}");
        }
    }
}

// Add Product Class
public class AddProduct : ISellerAction
{
    private List<Product> products;
    private string sellerUsername;

    public AddProduct(List<Product> products, string sellerUsername)
    {
        this.products = products;
        this.sellerUsername = sellerUsername;
    }

    public void SExecute()
    {
        var product = GetProductDetailsFromUser();
        products.Add(product);
        Console.WriteLine("\nProduct added successfully!");
    }

    private Product GetProductDetailsFromUser()
    {
        Console.Write("Enter Product Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Model: ");
        string model = Console.ReadLine();
        Console.Write("Enter Category: ");
        string category = Console.ReadLine();
        Console.Write("Enter Original Price (INR): ");
        decimal originalPrice = Convert.ToDecimal(Console.ReadLine());
        Console.Write("Enter Discounted Price (INR): ");
        decimal discountedPrice = Convert.ToDecimal(Console.ReadLine());
        Console.Write("Enter Description: ");
        string description = Console.ReadLine();
        Console.Write("Enter Quantity: ");
        int quantity = Convert.ToInt32(Console.ReadLine());

        return new Product(name, model, category, originalPrice, discountedPrice, description, sellerUsername, quantity);
    }
}

// Update Product Class
public class UpdateProduct : ISellerAction
{
    private List<Product> products;

    public UpdateProduct(List<Product> products)
    {
        this.products = products;
    }

    public void SExecute()
    {
        Console.Write("Enter Product ID to Update: ");
        int productId = Convert.ToInt32(Console.ReadLine());
        Product product = products.FirstOrDefault(p => p.Id == productId);
        if (product != null)
        {
            UpdateProductDetails(product);
            Console.WriteLine("Product updated successfully!");
        }
        else
        {
            Console.WriteLine("\nProduct not found!");
        }
    }

    private void UpdateProductDetails(Product product)
    {
        Console.Write("Enter New Product Name: ");
        product.Name = Console.ReadLine();
        Console.Write("Enter New Model: ");
        product.Model = Console.ReadLine();
        Console.Write("Enter New Category: ");
        product.Category = Console.ReadLine();
        Console.Write("Enter New Discounted Price: ");
        product.DiscountedPrice = Convert.ToDecimal(Console.ReadLine());
        Console.Write("Enter New Quantity: ");
        product.Quantity = Convert.ToInt32(Console.ReadLine());
        product.IsSoldOut = product.Quantity <= 0; // Update sold out status
    }
}

// Delete Product Class
public class DeleteProduct : ISellerAction
{
    private List<Product> products;

    public DeleteProduct(List<Product> products)
    {
        this.products = products;
    }

    public void SExecute()
    {
        Console.Write("Enter Product ID to Delete: ");
        int productId = Convert.ToInt32(Console.ReadLine());

        Product product = products.FirstOrDefault(p => p.Id == productId);
        if (product != null)
        {
            products.Remove(product);
            Console.WriteLine("\nProduct deleted successfully!");
        }
        else
        {
            Console.WriteLine("\nProduct not found!");
        }
    }
}

// Place Order Class
public class PlaceOrder : IOrderAction
{
    private List<Order> orders;
    private List<Product> products;
    private string buyerUsername;

    public PlaceOrder(List<Order> orders, List<Product> products, string buyerUsername)
    {
        this.orders = orders;
        this.products = products;
        this.buyerUsername = buyerUsername;
    }

    public void Execute()
    {
        Console.Write("Enter Product ID to Order: ");
        int productId = Convert.ToInt32(Console.ReadLine());
        Product product = products.FirstOrDefault(p => p.Id == productId);
        if (product != null)
        {
            if (product.Quantity > 0)
            {
                orders.Add(new Order(buyerUsername, productId));
                product.Quantity--; // Decrease quantity
                if (product.Quantity == 0)
                {
                    product.IsSoldOut = true; // Mark as sold out
                }
                Console.WriteLine("Order placed successfully!");
            }
            else
            {
                Console.WriteLine("Product is sold out!");
            }
        }
        else
        {
            Console.WriteLine("Invalid Product ID!");
        }
    }
}

// Cancel Order Class
public class CancelOrder : IOrderAction
{
    private List<Order> orders;
    private List<Product> products;
    private string buyerUsername;

    public CancelOrder(List<Order> orders, List<Product> products, string buyerUsername)
    {
        this.orders = orders;
        this.products = products;
        this.buyerUsername = buyerUsername;
    }

    public void Execute()
    {
        Console.Write("Enter Order ID to Cancel: ");
        int orderId = Convert.ToInt32(Console.ReadLine());
        Order order = orders.FirstOrDefault(o => o.OrderId == orderId && o.BuyerUsername == buyerUsername);
        if (order != null)
        {
            order.Status = "Cancelled";
            var product = products.FirstOrDefault(p => p.Id == order.ProductId);
            if (product != null)
            {
                product.Quantity++; // Increase quantity
                product.IsSoldOut = false; // Mark as available
            }
            Console.WriteLine("Order cancelled successfully!");
        }
        else
        {
            Console.WriteLine("Invalid Order ID or you do not have permission to cancel this order!");
        }
    }
}

// Track Order Class
public class TrackOrder : IOrderAction
{
    private List<Order> orders;
    private string buyerUsername;

    public TrackOrder(List<Order> orders, string buyerUsername)
    {
        this.orders = orders;
        this.buyerUsername = buyerUsername;
    }

    public void Execute()
    {
        Console.Write("Enter Order ID to Track: ");
        int orderId = Convert.ToInt32(Console.ReadLine());
        Order order = orders.FirstOrDefault(o => o.OrderId == orderId && o.BuyerUsername == buyerUsername);
        if (order != null)
        {
            Console.WriteLine($"Order ID: {order.OrderId}, Status: {order.Status}, Order Date: {order.OrderDate}");
        }
        else
        {
            Console.WriteLine("Invalid Order ID or you do not have permission to track this order!");
        }
    }
}

// View Order History Class
public class ViewOrderHistory : IOrderAction
{
    private List<Order> orders;
    private string buyerUsername;

    public ViewOrderHistory(List<Order> orders, string buyerUsername)
    {
        this.orders = orders;
        this.buyerUsername = buyerUsername;
    }

    public void Execute()
    {
        var buyerOrders = orders.Where(o => o.BuyerUsername == buyerUsername).ToList();
        if (buyerOrders.Any())
        {
            Console.WriteLine("\nOrder History:");
            foreach (var order in buyerOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Product ID: {order.ProductId}, Status: {order.Status}, Order Date: {order.OrderDate}");
            }
        }
        else
        {
            Console.WriteLine("\nNo orders found!");
        }
    }
}

// Display Order Menu Class
public class DisplayOrderMenu
{
    public void Display()
    {
        Console.WriteLine("\nOrder Menu:");
        Console.WriteLine("1. Place Order");
        Console.WriteLine("2. Cancel Order");
        Console.WriteLine("3. Track Order");
        Console.WriteLine("4. View Order History");
        Console.WriteLine("5. Back to Main Menu");
        Console.Write("Enter your choice: ");
    }
}

// Order Actions Class (Coordinator)
public class OrderActions : IBuyerAction
{
    private List<IOrderAction> orderActions;
    private DisplayOrderMenu displayOrderMenu;

    public OrderActions(List<IOrderAction> orderActions, DisplayOrderMenu displayOrderMenu)
    {
        this.orderActions = orderActions;
        this.displayOrderMenu = displayOrderMenu;
    }

    public void BExecute()
    {
        string choice;
        do
        {
            displayOrderMenu.Display();
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    orderActions[0].Execute(); // PlaceOrder
                    break;
                case "2":
                    orderActions[1].Execute(); // CancelOrder
                    break;
                case "3":
                    orderActions[2].Execute(); // TrackOrder
                    break;
                case "4":
                    orderActions[3].Execute(); // ViewOrderHistory
                    break;
                case "5":
                    Console.WriteLine("Returning to main menu...");
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        } while (choice != "5");
    }
}

// Feedback Service Class
public class FeedbackService
{
    private List<BuyerFeedback> buyerFeedbacks;
    private List<SellerFeedback> sellerFeedbacks;

    public FeedbackService(List<BuyerFeedback> buyerFeedbacks, List<SellerFeedback> sellerFeedbacks)
    {
        this.buyerFeedbacks = buyerFeedbacks;
        this.sellerFeedbacks = sellerFeedbacks;
    }

    public void SubmitBuyerFeedback(string buyerUsername, int productId, string feedback, int rating)
    {
        buyerFeedbacks.Add(new BuyerFeedback(buyerUsername, productId, feedback, rating));
        Console.WriteLine("\nFeedback submitted successfully!");
    }

    public void SubmitSellerFeedback(string sellerUsername, string feedback, int rating)
    {
        sellerFeedbacks.Add(new SellerFeedback(sellerUsername, feedback, rating));
        Console.WriteLine("\nFeedback submitted successfully!");
    }

    public void DisplayAverageRating(string username, string role)
    {
        if (role == "1") // Seller
        {
            var sellerFeedback = sellerFeedbacks.Where(f => f.SellerUsername == username).ToList();
            if (sellerFeedback.Any())
            {
                double averageRating = sellerFeedback.Average(f => f.Rating);
                Console.WriteLine($"Average Software Rating: {averageRating:F1} stars");
            }
            else
            {
                Console.WriteLine("No feedback available yet.");
            }
        }
        else if (role == "2") // Buyer
        {
            var buyerFeedback = buyerFeedbacks.Where(f => f.BuyerUsername == username).ToList();
            if (buyerFeedback.Any())
            {
                double averageRating = buyerFeedback.Average(f => f.Rating);
                Console.WriteLine($"Average Product Rating: {averageRating:F1} stars");
            }
            else
            {
                Console.WriteLine("No feedback available yet.");
            }
        }
    }
}

// Give Buyer Feedback Class
public class GiveBuyerFeedback : IBuyerAction
{
    private FeedbackService feedbackService;
    private List<Product> products;
    private List<Order> orders;
    private string buyerUsername;

    public GiveBuyerFeedback(FeedbackService feedbackService, List<Product> products, List<Order> orders, string buyerUsername)
    {
        this.feedbackService = feedbackService;
        this.products = products;
        this.orders = orders;
        this.buyerUsername = buyerUsername;
    }

    public void BExecute()
    {
        Console.Write("Enter Product ID to Review: ");
        int productId = Convert.ToInt32(Console.ReadLine());

        if (!HasPurchasedProduct(productId))
        {
            Console.WriteLine("\nYou can only review purchased products!");
            return;
        }

        int rating = GetRatingFromUser();
        if (rating < 1 || rating > 5)
        {
            Console.WriteLine("\nInvalid rating! Please enter a value between 1 and 5.");
            return;
        }

        Console.Write("Enter Your Review: ");
        string feedback = Console.ReadLine();

        feedbackService.SubmitBuyerFeedback(buyerUsername, productId, feedback, rating);
        feedbackService.DisplayAverageRating(buyerUsername, "2");
    }

    private bool HasPurchasedProduct(int productId)
    {
        return orders.Any(o => o.BuyerUsername == buyerUsername && o.ProductId == productId);
    }

    private int GetRatingFromUser()
    {
        Console.Write("Enter Your Rating (1 to 5 stars): ");
        return Convert.ToInt32(Console.ReadLine());
    }
}

// Give Seller Feedback Class
public class GiveSellerFeedback : ISellerAction
{
    private FeedbackService feedbackService;
    private string sellerUsername;

    public GiveSellerFeedback(FeedbackService feedbackService, string sellerUsername)
    {
        this.feedbackService = feedbackService;
        this.sellerUsername = sellerUsername;
    }

    public void SExecute()
    {
        Console.Write("Enter Your Feedback on the Software: ");
        string feedback = Console.ReadLine();

        int rating = GetRatingFromUser();
        if (rating < 1 || rating > 5)
        {
            Console.WriteLine("\nInvalid rating! Please enter a value between 1 and 5.");
            return;
        }

        feedbackService.SubmitSellerFeedback(sellerUsername, feedback, rating);
        feedbackService.DisplayAverageRating(sellerUsername, "1");
    }

    private int GetRatingFromUser()
    {
        Console.Write("Enter Your Rating (1 to 5 stars): ");
        return Convert.ToInt32(Console.ReadLine());
    }
}

// Payment Service Class
public class PaymentService
{
    private List<User> users;

    public PaymentService(List<User> users)
    {
        this.users = users;
    }

    public void ProcessPayment(User user)
    {
        string planChoice = GetSubscriptionPlanChoice();
        if (planChoice == null) return;

        decimal amount = planChoice == "1" ? 500 : 1000;
        user.SubscriptionType = planChoice == "1" ? "Basic" : "Premium";

        Console.WriteLine($"\nAmount to Pay: ₹{amount}");
        ProcessPaymentMethod(amount);
        Console.WriteLine($"\nSubscription Activated: {user.SubscriptionType} Account");
    }

    private string GetSubscriptionPlanChoice()
    {
        Console.WriteLine("\nSubscription Plans:");
        Console.WriteLine("1. Basic Account - ₹500/month");
        Console.WriteLine("2. Premium Account - ₹1000/month");
        Console.Write("Choose a subscription plan (1 or 2): ");
        string planChoice = Console.ReadLine();

        if (planChoice != "1" && planChoice != "2")
        {
            Console.WriteLine("Invalid choice!");
            return null;
        }

        return planChoice;
    }

    private void ProcessPaymentMethod(decimal amount)
    {
        Console.WriteLine("Select Payment Method:");
        Console.WriteLine("1. GPay");
        Console.WriteLine("2. Credit Card");
        Console.Write("Enter your choice (1 or 2): ");
        string paymentChoice = Console.ReadLine();

        if (paymentChoice == "1")
        {
            Console.WriteLine("\nRedirecting to GPay...");
            Console.WriteLine("Payment successful via GPay!");
        }
        else if (paymentChoice == "2")
        {
            ProcessCreditCardPayment();
        }
        else
        {
            Console.WriteLine("Invalid payment method!");
        }
    }

    private void ProcessCreditCardPayment()
    {
        Console.WriteLine("\nEnter Credit Card Details:");
        Console.Write("Card Number: ");
        string cardNumber = Console.ReadLine();
        Console.Write("Expiry Date (MM/YY): ");
        string expiryDate = Console.ReadLine();
        Console.Write("CVV: ");
        string cvv = Console.ReadLine();
        Console.WriteLine("Payment successful via Credit Card!");
    }
}

// Payment Module Class
public class PaymentModule : IBuyerAction, ISellerAction
{
    private PaymentService paymentService;
    private User user;

    public PaymentModule(PaymentService paymentService, User user)
    {
        this.paymentService = paymentService;
        this.user = user;
    }

    public void BExecute() => paymentService.ProcessPayment(user);
    public void SExecute() => paymentService.ProcessPayment(user);
}

// Search Products Class (Coordinator)
public class SearchProducts : IBuyerAction
{
    private List<Product> products;
    private List<ISearchAction> searchActions;

    public SearchProducts(List<Product> products, List<ISearchAction> searchActions)
    {
        this.products = products;
        this.searchActions = searchActions;
    }

    public void BExecute()
    {
        Console.WriteLine("\nSearch Products By:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Category");
        Console.WriteLine("3. Price");
        Console.Write("Enter your choice (1, 2, or 3): ");
        string searchChoice = Console.ReadLine();

        switch (searchChoice)
        {
            case "1":
                searchActions[0].Execute(products); // SearchByName
                break;
            case "2":
                searchActions[1].Execute(products); // SearchByCategory
                break;
            case "3":
                searchActions[2].Execute(products); // SearchByPrice
                break;
            default:
                Console.WriteLine("Invalid choice!");
                break;
        }
    }
}

// Search By Name Class
public class SearchByName : ISearchAction
{
    public void Execute(List<Product> products)
    {
        Console.Write("Enter Product Name: ");
        string name = Console.ReadLine();
        var results = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        DisplaySearchResults(results);
    }

    private void DisplaySearchResults(List<Product> results)
    {
        if (results.Any())
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var product in results)
            {
                if (!product.IsSoldOut)
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} , Model:{product.Model} , Category: {product.Category} , {product.DiscountedPrice}rs ,  Sold By: {product.Owner}");
                    Console.WriteLine($"Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
                    foreach (var review in product.Reviews)
                    {
                        Console.WriteLine($"- {review}");
                    }
                }
                else
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} (Sold Out)");
                }
            }
        }
        else
        {
            Console.WriteLine("\nNo products found matching your criteria.");
        }
    }
}

// Search By Category Class
public class SearchByCategory : ISearchAction
{
    public void Execute(List<Product> products)
    {
        Console.Write("Enter Product Category: ");
        string category = Console.ReadLine();
        var results = products.Where(p => p.Category.Contains(category, StringComparison.OrdinalIgnoreCase)).ToList();
        DisplaySearchResults(results);
    }

    private void DisplaySearchResults(List<Product> results)
    {
        if (results.Any())
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var product in results)
            {
                if (!product.IsSoldOut)
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} , Model:{product.Model} , Category: {product.Category} , {product.DiscountedPrice}rs ,  Sold By: {product.Owner}");
                    Console.WriteLine($"Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
                    foreach (var review in product.Reviews)
                    {
                        Console.WriteLine($"- {review}");
                    }
                }
                else
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} (Sold Out)");
                }
            }
        }
        else
        {
            Console.WriteLine("\nNo products found matching your criteria.");
        }
    }
}

// Search By Price Class
public class SearchByPrice : ISearchAction
{
    public void Execute(List<Product> products)
    {
        Console.Write("Enter Maximum Price (INR): ");
        decimal maxPrice = Convert.ToDecimal(Console.ReadLine());
        var results = products.Where(p => p.DiscountedPrice <= maxPrice).ToList();
        DisplaySearchResults(results);
    }

    private void DisplaySearchResults(List<Product> results)
    {
        if (results.Any())
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var product in results)
            {
                if (!product.IsSoldOut)
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} , Model:{product.Model} , Category: {product.Category} , {product.DiscountedPrice}rs ,  Sold By: {product.Owner}");
                    Console.WriteLine($"Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
                    foreach (var review in product.Reviews)
                    {
                        Console.WriteLine($"- {review}");
                    }
                }
                else
                {
                    Console.WriteLine($"PId:{product.Id}  ->  Product Name: {product.Name} (Sold Out)");
                }
            }
        }
        else
        {
            Console.WriteLine("\nNo products found matching your criteria.");
        }
    }
}

// Register User Class
public class RegisterUser : IAdminAction
{
    private List<User> users;

    public RegisterUser(List<User> users)
    {
        this.users = users;
    }

    public void AExecute()
    {
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();
        Console.Write("Enter Role (1.Seller  2.Buyer): ");
        string role = Console.ReadLine();

        string contactNumber = null;
        if (role == "1") // Seller
        {
            Console.Write("Enter Contact Number: ");
            contactNumber = Console.ReadLine();
        }

        users.Add(new User(username, password, role, contactNumber));
        Console.WriteLine("Registration successful!");
    }
}

// Admin Menu Class
public class AdminMenu : IAdminAction
{
    private List<User> users;
    private List<Product> products;
    private List<SellerFeedback> sellerFeedbacks;

    public AdminMenu(List<User> users, List<Product> products, List<SellerFeedback> sellerFeedbacks)
    {
        this.users = users;
        this.products = products;
        this.sellerFeedbacks = sellerFeedbacks;
    }
    
    public void AExecute()
    {
        string choice;
        do
        {
            Console.WriteLine("\nAdmin Menu:");
            Console.WriteLine("1. View User Details");
            Console.WriteLine("2. View Total Product Count");
            Console.WriteLine("3. View Product Feedback and Ratings");
            Console.WriteLine("4. View Seller Feedback on Software");
            Console.WriteLine("5. View All Products");
            Console.WriteLine("6. Logout");
            Console.Write("Enter choice: ");
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewUserDetails();
                    break;
                case "2":
                    ViewTotalProductCount();
                    break;
                case "3":
                    ViewProductFeedbackAndRatings();
                    break;
                case "4":
                    ViewSellerFeedbackOnSoftware();
                    break;
                case "5":
                    new ViewProducts(products, users).AExecute();
                    break;
                case "6":
                    Console.WriteLine("Logged out successfully!");
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        } while (choice != "6");
    }

    private void ViewUserDetails()
    {
        Console.WriteLine("\nUser Details:");
        Console.WriteLine("Sellers:");
        foreach (var user in users.Where(u => u.Role == "1"))
        {
            Console.WriteLine($"Username: {user.Username}, Contact: {user.ContactNumber}, Subscription: {user.SubscriptionType ?? "None"}");
        }
        Console.WriteLine("\nBuyers:");
        foreach (var user in users.Where(u => u.Role == "2"))
        {
            Console.WriteLine($"Username: {user.Username}, Subscription: {user.SubscriptionType ?? "None"}");
        }
    }

    private void ViewTotalProductCount()
    {
        Console.WriteLine($"\nTotal Products: {products.Count}");
    }

    private void ViewProductFeedbackAndRatings()
    {
        Console.WriteLine("\nProduct Feedback and Ratings:");
        foreach (var product in products)
        {
            Console.WriteLine($"Product: {product.Name}, Rating: {product.Rating:F1}, Reviews: {product.Reviews.Count}, Quantity: {product.Quantity}");
            foreach (var review in product.Reviews)
            {
                Console.WriteLine($"- {review}");
            }
        }
    }

    private void ViewSellerFeedbackOnSoftware()
    {
        Console.WriteLine("\nSeller Feedback on Software:");
        foreach (var feedback in sellerFeedbacks)
        {
            Console.WriteLine($"Seller: {feedback.SellerUsername}, Feedback: {feedback.Feedback}, Rating: {feedback.Rating}");
        }

        if (sellerFeedbacks.Any())
        {
            double averageRating = sellerFeedbacks.Average(f => f.Rating);
            Console.WriteLine($"\nAverage Software Rating: {averageRating:F1} stars");
        }
        else
        {
            Console.WriteLine("No feedback available yet.");
        }
    }
}

// User Menu Class
public class UserMenu
{
    private User user;
    private List<Product> products;
    private List<Order> orders;
    private List<SellerFeedback> sellerFeedbacks;
    private List<User> users;
    private FeedbackService feedbackService;
    private PaymentService paymentService;

    public UserMenu(User user, List<Product> products, List<Order> orders, List<SellerFeedback> sellerFeedbacks, List<User> users, FeedbackService feedbackService, PaymentService paymentService)
    {
        this.user = user;
        this.products = products;
        this.orders = orders;
        this.sellerFeedbacks = sellerFeedbacks;
        this.users = users;
        this.feedbackService = feedbackService;
        this.paymentService = paymentService;
    }

    public void Execute()
    {
        string choice;

        if (user.Role == "1") // Seller
        {
            do
            {
                Console.WriteLine("\nSeller Menu:");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Delete Product");
                Console.WriteLine("5. Give Feedback on Software");
                Console.WriteLine("6. Subscription Payment");
                Console.WriteLine("7. Logout");
                Console.Write("Enter your choice: ");
                choice = Console.ReadLine();

                if (choice == "1")
                    new ViewProducts(products, users).SExecute();
                else if (choice == "2")
                    new AddProduct(products, user.Username).SExecute();
                else if (choice == "3")
                    new UpdateProduct(products).SExecute();
                else if (choice == "4")
                    new DeleteProduct(products).SExecute();
                else if (choice == "5")
                    new GiveSellerFeedback(feedbackService, user.Username).SExecute();
                else if (choice == "6")
                    new PaymentModule(paymentService, user).SExecute();
                else if (choice == "7")
                    Console.WriteLine("Logged out successfully!");
                else
                    Console.WriteLine("Invalid choice, please try again.");

            } while (choice != "7");
        }
        else if (user.Role == "2") // Buyer
        {
            do
            {
                Console.WriteLine("\nBuyer Menu:");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Place Order");
                Console.WriteLine("3. Give Feedback");
                Console.WriteLine("4. Subscription Payment");
                Console.WriteLine("5. Search Products");
                Console.WriteLine("6. Logout");
                Console.Write("Enter your choice: ");
                choice = Console.ReadLine();

                if (choice == "1")
                    new ViewProducts(products, users).BExecute();
                else if (choice == "2")
                {
                    var orderActions = new List<IOrderAction>
                    {
                        new PlaceOrder(orders, products, user.Username),
                        new CancelOrder(orders, products, user.Username),
                        new TrackOrder(orders, user.Username),
                        new ViewOrderHistory(orders, user.Username)
                    };
                    var displayOrderMenu = new DisplayOrderMenu();
                    new OrderActions(orderActions, displayOrderMenu).BExecute();
                }
                else if (choice == "3")
                    new GiveBuyerFeedback(feedbackService, products, orders, user.Username).BExecute();
                else if (choice == "4")
                    new PaymentModule(paymentService, user).BExecute();
                else if (choice == "5")
                {
                    var searchActions = new List<ISearchAction>
                    {
                        new SearchByName(),
                        new SearchByCategory(),
                        new SearchByPrice()
                    };
                    new SearchProducts(products, searchActions).BExecute();
                }
                else if (choice == "6")
                    Console.WriteLine("Logged out successfully!");
                else
                    Console.WriteLine("Invalid choice, please try again.");

            } while (choice != "6");
        }
        else
        {
            Console.WriteLine("Invalid role.");
        }
    }
}

// Main Program
class Program
{
    static List<Order> orders = new List<Order>();
    static List<User> users = new List<User>();
    static List<Product> products = new List<Product>();
    static List<SellerFeedback> sellerFeedbacks = new List<SellerFeedback>();
    static List<BuyerFeedback> buyerFeedbacks = new List<BuyerFeedback>();

    static void Main()
    {
        string adminUsername = "admin";
        string adminPassword = "admin123";

        var feedbackService = new FeedbackService(buyerFeedbacks, sellerFeedbacks);
        var paymentService = new PaymentService(users);

        while (true)
        {
            Console.WriteLine("Welcome to the Online Reselling Platform");
            Console.WriteLine("1. Register\n2. Login\n3. Exit\n");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
                new RegisterUser(users).AExecute();
            else if (choice == "2")
            {
                Console.Write("Are you an 1. Admin, 2. Seller, 3. Buyer?: ");
                string userType = Console.ReadLine();

                Console.Write("Enter Username: ");
                string username = Console.ReadLine();
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                if (userType == "1") // Admin
                {
                    if (username == adminUsername && password == adminPassword)
                        new AdminMenu(users, products, sellerFeedbacks).AExecute();
                    else
                        Console.WriteLine("Invalid Admin credentials!");
                }
                else if (userType == "2" || userType == "3") // Seller (1) or Buyer (2)
                {
                    string mappedRole = userType == "2" ? "1" : "2";
                    User user = users.FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == mappedRole);
                    if (user != null)
                        new UserMenu(user, products, orders, sellerFeedbacks, users, feedbackService, paymentService).Execute();
                    else
                        Console.WriteLine("\nInvalid credentials or role! Try again.");
                }
                else
                {
                    Console.WriteLine("\nInvalid choice! Try again.");
                }
            }
            else if (choice == "3")
                return;
            else
                Console.WriteLine("\nInvalid choice! Try again.");


        }
        Console.ReadKey();
    }
}