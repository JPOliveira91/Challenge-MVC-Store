using Challenge_MVC_Store.Data.Models;

namespace Challenge_MVC_Store.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                List<Product> products =
                [
                    new() { Name = "Laptop", Price = 10000.00m },
                    new() { Name = "Smartphone", Price = 1000.00m },
                    new() { Name = "Headphones", Price = 100.00m },
                    new() { Name = "Case", Price = 10.00m },
                    new() { Name = "Gum", Price = 0.99m },
                ];

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            if (!context.Customers.Any())
            {
                List<Customer> customers =
                [
                    new() { Name = "Juliano Oliveira", Email = "jp_oliveira1991@hotmail.com", CreationDate = DateTime.Now },
                    new() { Name = "Abalberto Gomes", Email = "agomes@teste.com", CreationDate = DateTime.Now },
                    new() { Name = "Rafael Brazão", Email = "rbrazao@teste.com", CreationDate = DateTime.Now },
                    new() { Name = "Luiza Santos", Email = "lsantos@teste.com", CreationDate = DateTime.Now },
                    new() { Name = "Maria Abraão", Email = "mabraao@teste.com", CreationDate = DateTime.Now },
                    new() { Name = "Rogério Ceni", Email = "rceni@teste.com", CreationDate = DateTime.Now },
                ];

                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                int customerA_Id = context.Customers.ElementAt(0).Id;
                int customerB_Id = context.Customers.ElementAt(1).Id;
                int customerC_Id = context.Customers.ElementAt(2).Id;
                int customerD_Id = context.Customers.ElementAt(3).Id;
                int customerE_Id = context.Customers.ElementAt(4).Id;

                List<Order> orders =
                [
                    new() { CustomerId = customerA_Id, Date = DateTime.Now },
                    new() { CustomerId = customerB_Id, Date = DateTime.Now },
                    new() { CustomerId = customerC_Id, Date = DateTime.Now },
                    new() { CustomerId = customerD_Id, Date = DateTime.Now },
                    new() { CustomerId = customerD_Id, Date = DateTime.Now },
                ];

                context.Orders.AddRange(orders);
                context.SaveChanges();
            }

            if (!context.OrderProducts.Any())
            {
                int orderA_Id = context.Orders.ElementAt(0).Id;
                int orderB_Id = context.Orders.ElementAt(1).Id;
                int orderC_Id = context.Orders.ElementAt(2).Id;
                int orderD_Id = context.Orders.ElementAt(3).Id;
                int orderE_Id = context.Orders.ElementAt(4).Id;

                int productA_Id = context.Products.ElementAt(0).Id;
                int productB_Id = context.Products.ElementAt(1).Id;
                int productC_Id = context.Products.ElementAt(2).Id;
                int productD_Id = context.Products.ElementAt(3).Id;
                int productE_Id = context.Products.ElementAt(4).Id;

                List<OrderProduct> orderProducts =
                [
                    new() { OrderId = orderA_Id, ProductId = productA_Id, Quantity = 1, UnitPrice = context.Products.ElementAt(0).Price },
                    new() { OrderId = orderA_Id, ProductId = productB_Id, Quantity = 2, UnitPrice = context.Products.ElementAt(1).Price },
                    new() { OrderId = orderA_Id, ProductId = productC_Id, Quantity = 3, UnitPrice = context.Products.ElementAt(2).Price },
                    new() { OrderId = orderA_Id, ProductId = productD_Id, Quantity = 4, UnitPrice = context.Products.ElementAt(3).Price },
                    new() { OrderId = orderA_Id, ProductId = productE_Id, Quantity = 5, UnitPrice = context.Products.ElementAt(4).Price },

                    new() { OrderId = orderB_Id, ProductId = productB_Id, Quantity = 4, UnitPrice = context.Products.ElementAt(1).Price },
                    new() { OrderId = orderB_Id, ProductId = productC_Id, Quantity = 3, UnitPrice = context.Products.ElementAt(2).Price },
                    new() { OrderId = orderB_Id, ProductId = productD_Id, Quantity = 2, UnitPrice = context.Products.ElementAt(3).Price },
                    new() { OrderId = orderB_Id, ProductId = productE_Id, Quantity = 1, UnitPrice = context.Products.ElementAt(4).Price },

                    new() { OrderId = orderC_Id, ProductId = productC_Id, Quantity = 1, UnitPrice = context.Products.ElementAt(2).Price },
                    new() { OrderId = orderC_Id, ProductId = productD_Id, Quantity = 2, UnitPrice = context.Products.ElementAt(3).Price },
                    new() { OrderId = orderC_Id, ProductId = productE_Id, Quantity = 3, UnitPrice = context.Products.ElementAt(4).Price },

                    new() { OrderId = orderD_Id, ProductId = productD_Id, Quantity = 2, UnitPrice = context.Products.ElementAt(3).Price },
                    new() { OrderId = orderD_Id, ProductId = productE_Id, Quantity = 1, UnitPrice = context.Products.ElementAt(4).Price },

                    new() { OrderId = orderE_Id, ProductId = productE_Id, Quantity = 1, UnitPrice = context.Products.ElementAt(4).Price },
                ];

                context.OrderProducts.AddRange(orderProducts);
                context.SaveChanges();
            }
        }
    }
}