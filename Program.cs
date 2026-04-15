using System;
using System.Collections;

// Product class definition
class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    // Constructor to initialize product
    public Product(int id, string name, double price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        RemainingStock = stock;
    }

    // Display product details
    public void DisplayProduct()
    {
        Console.WriteLine($"[{Id}] {Name} - ₱{Price} (Stock: {RemainingStock})");
    }

    // Check if enough stock is available
    public bool HasEnoughStock(int quantity)
    {
        return quantity <= RemainingStock;
    }

    // Deduct stock after purchase
    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }

    // Get total price for selected quantity
    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }
}

// Cart Item class
class CartItem
{
    public Product Product;
    public int Quantity;
    public double SubTotal;
}

class Program
{
    static void Main()
    {
        // Store menu (array of products)
        Product[] store = new Product[]
        {
            new Product(1, "Shampoo", 120, 10),
            new Product(2, "Soap", 50, 20),
            new Product(3, "Toothpaste", 90, 15),
            new Product(4, "Lotion", 200, 5),
            new Product(5, "Perfume", 1500, 3)
        };

        // Cart (fixed size)
        CartItem[] cart = new CartItem[10];
        int cartCount = 0;

        bool continueShopping = true;

        while (continueShopping)
        {
            Console.WriteLine("\n=== STORE MENU ===");
            foreach (Product p in store)
            {
                p.DisplayProduct();
            }

            // Input product number
            Console.Write("Enter product number: ");
            string productInput = Console.ReadLine();
            if (!int.TryParse(productInput, out int productNumber))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            // Validate product number
            if (productNumber < 1 || productNumber > store.Length)
            {
                Console.WriteLine("Invalid product number.");
                continue;
            }

            Product selectedProduct = store[productNumber - 1];

            // Out of stock check
            if (selectedProduct.RemainingStock == 0)
            {
                Console.WriteLine("This product is out of stock.");
                continue;
            }

            // Input quantity
            Console.Write("Enter quantity: ");
            string qtyInput = Console.ReadLine();
            if (!int.TryParse(qtyInput, out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            // Stock validation
            if (!selectedProduct.HasEnoughStock(quantity))
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }

            // Check duplicate in cart
            bool found = false;
            for (int i = 0; i < cartCount; i++)
            {
                if (cart[i].Product.Id == selectedProduct.Id)
                {
                    cart[i].Quantity += quantity;
                    cart[i].SubTotal += selectedProduct.GetItemTotal(quantity);
                    found = true;
                    break;
                }
            }

            // Add new item if not duplicate
            if (!found)
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart is full.");
                    continue;
                }

                cart[cartCount] = new CartItem
                {
                    Product = selectedProduct,
                    Quantity = quantity,
                    SubTotal = selectedProduct.GetItemTotal(quantity)
                };
                cartCount++;
            }

            // Deduct stock
            selectedProduct.DeductStock(quantity);

            Console.WriteLine("Item added to cart successfully!");

            // Ask to continue
            Console.Write("Do you want to add more items? (Y/N): ");
            string choice = Console.ReadLine().ToUpper();
            if (choice == "N")
            {
                continueShopping = false;
            }
        }

        // Display receipt with detailed formatting
        Console.WriteLine("\n================ RECEIPT ================");
        Console.WriteLine("Item\tQty\tPrice\tSubtotal");

        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
        {
            double price = cart[i].Product.Price;
            double subtotal = cart[i].SubTotal;

            Console.WriteLine($"{cart[i].Product.Name}\t{cart[i].Quantity}\t₱{price}\t₱{subtotal}");
            grandTotal += subtotal;
        }

        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"Grand Total:\t\t\t₱{grandTotal}");

        // Discount computation (10% if >= 5000)
        double discount = 0;
        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
            Console.WriteLine($"Discount (10%):\t\t-₱{discount}");
        }
        else
        {
            Console.WriteLine("Discount:\t\t\t₱0 (Not eligible)");
        }

        double finalTotal = grandTotal - discount;
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"FINAL TOTAL:\t\t\t₱{finalTotal}");

        // Display updated stock after checkout
        Console.WriteLine("\n=========== UPDATED STOCK ===========");
        foreach (Product p in store)
        {
            p.DisplayProduct();
        }

        Console.WriteLine("\nThank you for shopping!");
    }
}
