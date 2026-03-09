using BL.BlApi;
using System;
using BO = BL.BO;

namespace BlTest
{
    internal class Program
    {
       
        static readonly IBl s_bl = Factory.Get;

        static void Main(string[] args)
        {
            try
            {
               
                //DalTest.Initialization.Initialize();

                DisplayMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical Error: {ex.Message}");
            }
        }

        #region Entity Input Logic (Returns BO Objects)
        private static BO.Customer InputCustomer(int id = 0)
        {
            Console.WriteLine($"\n--- {(id == 0 ? "New" : "Update")} BO Customer Details ---");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Address: ");
            string addr = Console.ReadLine() ?? "";
            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? "";

            return new BO.Customer { Id = id, CustomerName = name, Address = addr, PhoneNumber = phone };
        }

        private static BO.Product InputProduct(int id = 0)
        {
            Console.WriteLine($"\n--- {(id == 0 ? "New" : "Update")} BO Product Details ---");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Category (0-dogs, 1-fish, 2-cats, 3-parrots, 4-rabbits, 5-hamsters): ");
            BO.Categories cat = (BO.Categories)int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Price: ");
            double price = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Quantity: ");
            int qty = int.Parse(Console.ReadLine() ?? "0");

            return new BO.Product { Id = id, Name = name, Category = cat, Price = price, Quantity = qty };
        }
        #endregion

        #region Menus
        private static void DisplayMainMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n======= BL Main Menu =======");
                Console.WriteLine("1. Products Management");
                Console.WriteLine("2. Customers Management");
                Console.WriteLine("3. Sales Management");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        // ציון מפורש של הטיפוסים <הישות, הממשק>
                        DisplaySubMenu<BO.Product, BL.BlApi.IProduct>("Products", s_bl.Product);
                        break;

                    case "2":
                        DisplaySubMenu<BO.Customer, BL.BlApi.ICustomer>("Customers", s_bl.Customer);
                        break;

                    case "3":
                        // אם יש לך מימוש ל-Sales
                        DisplaySubMenu<BO.Sale, BL.BlApi.ISale>("Sales", s_bl.Sale);
                        break;

                    case "4":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private static void DisplaySubMenu<T, TInterface>(string entityName, TInterface api)
            where T : class
        {
            // כאן המבנה דומה ל-DalTest, אך הקריאות הן ל-api שקיבלנו מה-BL
            // לדוגמה עבור Product: api יהיה s_bl.Product
            bool back = false;
            while (!back)
            {
                Console.WriteLine($"\n--- {entityName} (BO) Management ---");
                Console.WriteLine("1. View All | 2. Get by ID | 3. Add | 4. Update | 5. Delete | 6. Back");
                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            // שימוש ב-ReadAll של ה-BL (מחזיר BO)
                            // הערה: תצטרכי להתאים את הקריאה לפי הממשק הספציפי
                            break;
                        case "3":
                            if (typeof(T) == typeof(BO.Product))
                            {
                                var item = InputProduct();
                                int id = (api as BL.BlApi.IProduct)!.Create(item);
                                Console.WriteLine($"Created BO Product with ID: {id}");
                            }
                            break;
                        case "6": back = true; break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BL Error: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"Source Error (DAL): {ex.InnerException.Message}");
                }
            }
        }
        #endregion
    }
}