using System;
using System.Linq;

namespace Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Создаем контекст базы данных
            using (var context = new ShopDbContext())
            {
                // Создаем сервис магазина
                var serviceShop = new ServiceShop(context);

                // Добавляем клиентов
                serviceShop.AddClient(new Client { Name = "Иван", Email = "ivan@example.com" });
                serviceShop.AddClient(new Client { Name = "Мария", Email = "maria@example.com" });

                // Добавляем продукты
                serviceShop.AddProduct(new Product { Name = "Ноутбук", Price = 1000 });
                serviceShop.AddProduct(new Product { Name = "Смартфон", Price = 500 });

                // Получаем всех клиентов
                var clients = serviceShop.GetAllClients().ToList();
                Console.WriteLine("Список клиентов:");
                foreach (var client in clients)
                {
                    Console.WriteLine($"ID: {client.Id}, Имя: {client.Name}, Email: {client.Email}");
                }
                Console.WriteLine();

                // Получаем всех продуктов
                var products = serviceShop.GetAllProducts().ToList();
                Console.WriteLine("Список продуктов:");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id}, Название: {product.Name}, Цена: {product.Price}");
                }
                Console.WriteLine();

                // Добавляем заказ
                var order = new Order
                {
                    ClientId = clients.First().Id,
                    Products = products
                };
                serviceShop.AddOrder(order);

                // Получаем все заказы с деталями
                var orders = serviceShop.GetAllOrdersWithDetails().ToList();
                Console.WriteLine("Список заказов с деталями:");
                foreach (var ord in orders)
                {
                    Console.WriteLine($"ID заказа: {ord.Id}, Клиент: {ord.Client.Name}, Продукты:");
                    foreach (var prod in ord.Products)
                    {
                        Console.WriteLine($"  - {prod.Name}, Цена: {prod.Price}");
                    }
                }
                Console.WriteLine("Enter some key for exit:");
                Console.ReadKey();

                // 1. Найти всех клиентов с "example.com" в email
                var clientsWithExampleDomain = context.Clients
                    .Where(c => c.Email.Contains("example.com"))
                    .ToList();

                // 2. Получить все продукты, цена которых меньше средней цены всех продуктов
                var averagePrice = context.Products.Average(p => p.Price);
                var cheaperProducts = context.Products
                    .Where(p => p.Price < averagePrice)
                    .ToList();

                // 3. Найти клиента с самым длинным именем
                var longestNameClient = context.Clients
                    .OrderByDescending(c => c.Name.Length)
                    .FirstOrDefault();

                // 4. Получить все заказы клиента с самым коротким именем
                var shortestNameClient = context.Clients
                    .OrderBy(c => c.Name.Length)
                    .FirstOrDefault();
                var ordersOfShortestNameClient = context.Orders
                    .Where(o => o.ClientId == shortestNameClient.Id)
                    .ToList();

                // 5. Найти продукт с наименьшей ценой
                var cheapestProduct = context.Products
                    .OrderBy(p => p.Price)
                    .FirstOrDefault();

                // 6. Получить всех клиентов, у которых количество заказов превышает среднее количество заказов всех клиентов
                var averageOrderCount = context.Orders
                    .GroupBy(o => o.ClientId)
                    .Average(g => g.Count());

                var clientsAboveAverageOrders = context.Clients
                    .Where(c => context.Orders.Count(o => o.ClientId == c.Id) > averageOrderCount)
                    .ToList();

                

                // 8. Получить среднюю цену заказа
                var averageOrderPrice = context.Orders
                    .Select(o => o.Products.Sum(p => p.Price))
                    .Average();

                // 9. Найти клиента, который сделал последний заказ
                var lastOrder = context.Orders
                    .OrderByDescending(o => o.Id)
                    .FirstOrDefault();
                var lastOrderClient = lastOrder?.Client;

                // 10. Получить все заказы, содержащие продукты с ценой больше 800
                var ordersWithExpensiveProducts = context.Orders
                    .Where(o => o.Products.Any(p => p.Price > 800))
                    .ToList();

                // Вывод результатов (по желанию)
                Console.WriteLine("Клиенты с 'example.com':");
                clientsWithExampleDomain.ForEach(c => Console.WriteLine($"ID: {c.Id}, Имя: {c.Name}, Email: {c.Email}"));

                Console.WriteLine("\nПродукты с ценой ниже средней:");
                cheaperProducts.ForEach(p => Console.WriteLine($"ID: {p.Id}, Название: {p.Name}, Цена: {p.Price}"));

                Console.WriteLine("\nКлиент с самым длинным именем:");
                Console.WriteLine($"ID: {longestNameClient.Id}, Имя: {longestNameClient.Name}");

                Console.WriteLine("\nЗаказы клиента с самым коротким именем:");
                ordersOfShortestNameClient.ForEach(o => Console.WriteLine($"ID: {o.Id}, Клиент: {o.Client.Name}"));

                Console.WriteLine("\nСамый дешевый продукт:");
                Console.WriteLine($"ID: {cheapestProduct.Id}, Название: {cheapestProduct.Name}, Цена: {cheapestProduct.Price}");

                Console.WriteLine("\nКлиенты с количеством заказов выше среднего:");
                clientsAboveAverageOrders.ForEach(c => Console.WriteLine($"ID: {c.Id}, Имя: {c.Name}"));

                Console.WriteLine($"\nСредняя цена заказа: {averageOrderPrice}");

                Console.WriteLine("\nКлиент, который сделал последний заказ:");
                Console.WriteLine($"ID: {lastOrderClient.Id}, Имя: {lastOrderClient.Name}");

                Console.WriteLine("\nЗаказы с продуктами дороже 800:");
                ordersWithExpensiveProducts.ForEach(o => Console.WriteLine($"ID: {o.Id}, Клиент: {o.Client.Name}"));

            }

        }
    }
}

