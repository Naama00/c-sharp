using Dal;
using DalApi;
using DO;
using tools;
using System.Reflection;

namespace DalTest
{
    internal class Program
    {
        private static IDal s_dal = DalList.Instance;

        static void Main(string[] args)
        {
            try
            {      
            
                Initialization.Initialize(s_dal);
                displayMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                       MethodBase.GetCurrentMethod().Name, $"Exception: {ex}");
            }
        }

        #region Entity Input Logic
        private static Customer InputCustomer(int id = 0)
        {
            Console.WriteLine($"\n--- {(id == 0 ? "New" : "Update")} Customer Details ---");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Address: ");
            string addr = Console.ReadLine() ?? "";
            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? "";
            return new Customer(id, name, addr, phone);
        }

        private static Product InputProduct(int id = 0)
        {
            Console.WriteLine($"\n--- {(id == 0 ? "New" : "Update")} Product Details ---");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Category (0-dogs, 1-fish, 2-cats, 3-parrots, 4-rabbits, 5-hamsters): ");
            Categories cat = (Categories)int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Price: ");
            double price = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("Initial Quantity: ");
            int qty = int.Parse(Console.ReadLine() ?? "0");

            return new Product(id, name, cat, price, qty);
        }

        private static Sale InputSale(int id = 0)
        {
            Console.WriteLine($"\n--- {(id == 0 ? "New" : "Update")} Sale Details ---");

            Console.Write("Product ID: ");
            int prodId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Required Quantity: ");
            int qty = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Discounted Price: ");
            double price = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("Club Member? (y/n): ");
            bool isClub = Console.ReadLine()?.ToLower() == "y";

            Console.Write("Start Date (yyyy-mm-dd): ");
            DateTime start = DateTime.Parse(Console.ReadLine() ?? "");

            Console.Write("End Date (yyyy-mm-dd): ");
            DateTime end = DateTime.Parse(Console.ReadLine() ?? "");

            return new Sale(id, prodId, qty, price, isClub, start, end);
        }
        #endregion

        #region Menus
        private static void displayMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n======= Main Menu =======");
                Console.WriteLine("1. Sales Management");
                Console.WriteLine("2. Products Management");
                Console.WriteLine("3. Customers Management");
                Console.WriteLine("4. delete old log files");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");

                string input = Console.ReadLine() ?? "";
                switch (input)
                {
                    case "1": displaySubMenu("Sales", s_dal.Sale); break;
                    case "2": displaySubMenu("Products", s_dal.Product); break;
                    case "3": displaySubMenu("Customers", s_dal.Customer); break;
                    case "4": LogManager.deleteOldLogs(); Console.WriteLine("Old log files deleted."); break;
                    case "5": exit = true; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        private static void displaySubMenu<T>(string entityName, ICrud<T> dal) where T : class
        {
            bool backToMain = false;
            while (!backToMain)
            {
                Console.WriteLine($"\n--- {entityName} Management ---");
                Console.WriteLine("1. Display All | 2. Find by ID | 3. Add | 4. Update | 5. Delete | 6. Back");
                Console.Write("Select an action: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        foreach (var item in dal.ReadAll()) Console.WriteLine(item);
                        break;

                    case "2":
                        Console.Write("Enter ID: ");
                        int idToFind = int.Parse(Console.ReadLine() ?? "0");
                        var found = dal.Read(idToFind);
                        Console.WriteLine(found != null ? found : "Not found.");
                        break;

                    case "3":
                    case "4":
                        {
                            int targetId = 0;
                            if (choice == "4")
                            {
                                Console.Write("Enter ID to update: ");
                                targetId = int.Parse(Console.ReadLine() ?? "0");
                                if (dal.Read(targetId) == null)
                                {
                                    Console.WriteLine("Item not found.");
                                    break;
                                }
                            }

                            T? item = null;
                            if (typeof(T) == typeof(Customer)) item = InputCustomer(targetId) as T;
                            else if (typeof(T) == typeof(Product)) item = InputProduct(targetId) as T;
                            else if (typeof(T) == typeof(Sale)) item = InputSale(targetId) as T;

                            if (item != null)
                            {
                                try
                                {
                                    if (choice == "3")
                                    {
                                        int newId = dal.Create(item);
                                        Console.WriteLine($"Added successfully! New ID: {newId}");
                                    }
                                    else
                                    {
                                        dal.Update(item);
                                        Console.WriteLine("Updated successfully!");
                                    }
                                }
                                catch (Exception ex) {
                                    Console.WriteLine($"Error: {ex.Message}");
                                    LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                         MethodBase.GetCurrentMethod().Name, $"Exception: {ex}");
                                }
                            }
                        }
                        break;

                    case "5":
                        try
                        {
                            Console.Write("Enter ID to delete: ");
                            dal.Delete(int.Parse(Console.ReadLine() ?? "0"));
                            Console.WriteLine("Deleted successfully.");
                        }
                        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}");
                            LogManager.Log(MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                          MethodBase.GetCurrentMethod().Name, $"Exception: {ex}");
                        }
                        break;

                    case "6": backToMain = true; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }
        #endregion
    }
}