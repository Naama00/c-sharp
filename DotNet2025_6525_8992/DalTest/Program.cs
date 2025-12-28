using DO;
using DalApi;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace DalTest
{
    internal class Program
    {
        private static void displayMenu()
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display all Sales");
            Console.WriteLine("2. Display all Products");
            Console.WriteLine("3. Display all Customers");
            Console.WriteLine("4. Exit");
            Console.Write("Please select an option (1-4): ");
            string userInput = Console.ReadLine();
            string entity = userInput switch
            {
                "1" => "Sales",
                "2" => "Products",
                "3" => "Customers",
                "4" => "Exit",
                _ => "Unknown" 
            };
            if (entity == "Unknown")
            {
                Console.WriteLine("Wrong choice. try again:");
                displayMenu();
            }
            else if(entity == "Exit")
            {
                Console.WriteLine("Exiting the program. Goodbye!");
                return;
            }
            else
            displaySubMenu(entity);
          }
        private static void displaySubMenu(string entity)
        {
            Console.WriteLine($"\n--- {entity} Management ---");
            Console.WriteLine($"1. Display all {entity}");
            Console.WriteLine($"2. Display specific {entity} by ID"); 
            Console.WriteLine($"3. Add new {entity}");
            Console.WriteLine($"4. Update {entity}");
            Console.WriteLine($"5. Delete {entity}");
            Console.WriteLine("6. Back to Main Menu");
            Console.Write("Select an action (1-6): ");

            string subInput = Console.ReadLine();

            switch (subInput)
            {
                case "1":
                    Console.WriteLine($"Showing all {entity}...");
                    // קריאה לפונקציה המציגה את כל הרשימה
                    break;

                case "2":
                    Console.Write($"Enter {entity} ID: ");
                    string idInput = Console.ReadLine();

                    // המרה של ה-ID למספר (בדיקה בסיסית)
                    if (int.TryParse(idInput, out int id))
                    {
                        Console.WriteLine($"Searching for {entity} with ID: {id}...");
                        // כאן תבוא קריאה לפונקציה שמחפשת לפי ID, למשל: GetById(id);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID. Please enter a number.");
                    }
                    break;

                case "3":
                    Console.WriteLine($"Adding new {entity}...");
                    break;

                case "4":
                    Console.WriteLine($"Updating {entity}...");
                    break;

                case "5":
                    Console.WriteLine($"Deleting {entity}...");
                    break;

                case "6":
                    displayMenu();
                    return;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    displaySubMenu(entity);
                    break;
            }
        }
       

        static void Main(string[] args)
        {
            try
            {
                SaleImplementation isale = new SaleImplementation();
                ProductImplementation iproduct = new ProductImplementation();
                CustomerImplementation icustomer = new CustomerImplementation();
                Initialization.Initialize(isale, icustomer, iproduct);

                //isale.PrintAll();
                //iproduct.PrintAll();
                //icustomer.PrintAll();



            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
