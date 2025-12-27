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
        //public static void displayMenu()
        //{
        //    Console.WriteLine("choose category: 1-product, 2-sales ,3-customers");
        //    displaySubMenu(Console.ReadLine());
        //}

        public static void displaySubMenu(int choice)
        {

        }
        public static void manageProduct(int choice)
        {
            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    Console.WriteLine("invalid input");
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
