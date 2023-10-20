using System;
using System.Collections.Generic;

class Product
{
    public string Name { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public decimal CalculatePrice()
    {
        return Price * Quantity;
    }
}

class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public bool IsInUSA()
    {
        return Country == "USA";
    }

    public string GetFullAddress()
    {
        return $"{Street}\n{City}, {State}\n{Country}";
    }
}

class Customer
{
    public string Name { get; set; }
    public Address CustomerAddress { get; set; }

    public bool IsInUSA()
    {
        return CustomerAddress.IsInUSA();
    }
}

class Order
{
    private List<Product> Products { get; set; }
    private Customer Customer { get; set; }

    public Order(List<Product> products, Customer customer)
    {
        Products = products;
        Customer = customer;
    }

    public decimal CalculateTotal()
    {
        decimal total = 0;
        foreach (var product in Products)
        {
            total += product.CalculatePrice();
        }

        total += Customer.IsInUSA() ? 5 : 35;
        return total;
    }

    public string GetPackingLabel()
    {
        string packingLabel = "Packing Label:\n";
        foreach (var product in Products)
        {
            packingLabel += $"{product.Name} (ID: {product.ProductId})\n";
        }
        return packingLabel;
    }

    public string GetShippingLabel()
    {
        string shippingLabel = "Shipping Label:\n";
        shippingLabel += $"Name: {Customer.Name}\n";
        shippingLabel += "Address:\n";
        shippingLabel += Customer.CustomerAddress.GetFullAddress();
        return shippingLabel;
    }
}

class Program
{
    static void Main()
    {
        // Create products
        var product1 = new Product { Name = "Laptop", ProductId = 1, Price = 999.99m, Quantity = 2 };
        var product2 = new Product { Name = "Mouse", ProductId = 2, Price = 19.99m, Quantity = 5 };

        // Create customer
        var customerAddress = new Address { Street = "123 Main St", City = "Anytown", State = "CA", Country = "USA" };
        var customer = new Customer { Name = "John Doe", CustomerAddress = customerAddress };

        // Create an order
        var order = new Order(new List<Product> { product1, product2 }, customer);

        // Display order information
        Console.WriteLine("Order Details:");
        Console.WriteLine(order.GetPackingLabel());
        Console.WriteLine(order.GetShippingLabel());
        Console.WriteLine($"Total Price: {order.CalculateTotal():C}");
    }
}
