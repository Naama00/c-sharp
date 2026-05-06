using BL.BlApi;
using System;
using BO = BL.BO;

namespace BlTest
{
    internal class Program
    {
       
        static readonly IBl s_bl = Factory.Get();

        static void Main(string[] args)
        {
            try
            {
               
                DalTest.Initialization.Initialize();

                DisplayMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical Error: {ex.Message}");
            }
        }

        #region Entity Input Logic (Returns BO Objects)
        private static BO.Customer InputCustomer(BO.Customer? oldCustomer = null)
        {
            // אם זה עדכון, נשמור על ה-ID המקורי (שבדרך כלל הוא תעודת זהות בלקוחות)
            // אם זה לקוח חדש, נקלוט ID מהמשתמש
            int id;
            if (oldCustomer == null)
            {
                Console.Write("Enter Customer ID (ID number): ");
                int.TryParse(Console.ReadLine(), out id);
            }
            else
            {
                id = oldCustomer.Id;
                Console.WriteLine($"Updating Customer ID: {id}");
            }

            // 1. קליטת שם
            Console.Write($"Enter Name (current: '{oldCustomer?.CustomerName}'): ");
            string nameInput = Console.ReadLine() ?? "";
            string name = string.IsNullOrWhiteSpace(nameInput) ? (oldCustomer?.CustomerName ?? "") : nameInput;

            // 2. קליטת פלאפון
            Console.Write($"Enter Phone Number (current: '{oldCustomer?.PhoneNumber}'): ");
            string phoneInput = Console.ReadLine() ?? "";
            string phone = string.IsNullOrWhiteSpace(phoneInput) ? (oldCustomer?.PhoneNumber ?? "") : phoneInput;

            // 3. קליטת כתובת
            Console.Write($"Enter Address (current: '{oldCustomer?.Address}'): ");
            string addressInput = Console.ReadLine() ?? "";
            string address = string.IsNullOrWhiteSpace(addressInput) ? (oldCustomer?.Address ?? "") : addressInput;

            return new BO.Customer
            {
                Id = id,
                CustomerName = name,
                PhoneNumber = phone,
                Address = address
            };
        }

        private static BO.Product InputProduct(BO.Product? oldProduct = null)
        {
            // אם אנחנו בעדכון, נשמור על ה-ID המקורי. אם בהוספה, ה-ID יהיה 0.
            int id = oldProduct?.Id ?? 0;

            // 1. קליטת שם
            Console.Write($"Enter Product Name (current: '{oldProduct?.Name}'): ");
            string nameInput = Console.ReadLine() ?? "";
            string name = (string.IsNullOrWhiteSpace(nameInput)) ? (oldProduct?.Name ?? "Unknown") : nameInput;

            // 2. קליטת מחיר
            Console.Write($"Enter Price (current: {oldProduct?.Price}): ");
            string priceInput = Console.ReadLine() ?? "";
            double price;
            if (string.IsNullOrWhiteSpace(priceInput))
                price = oldProduct?.Price ?? 0;
            else
                double.TryParse(priceInput, out price);

            // 3. קליטת כמות
            Console.Write($"Enter Quantity in stock (current: {oldProduct?.Quantity}): ");
            string qtyInput = Console.ReadLine() ?? "";
            int quantity;
            if (string.IsNullOrWhiteSpace(qtyInput))
                quantity = oldProduct?.Quantity ?? 0;
            else
                int.TryParse(qtyInput, out quantity);

            // 4. קליטת Enum (Category)
            Console.WriteLine("Choose Category:");
            // מדפיס למשתמש את האפשרויות: DOGS, FISH, CATS...
            var categories = Enum.GetValues(typeof(BO.Categories));
            foreach (var cat in categories)
            {
                Console.WriteLine($" - {cat}");
            }

            Console.Write($"Enter Category (current: {oldProduct?.Category}): ");
            string catInput = Console.ReadLine() ?? "";
            BO.Categories category;

            if (string.IsNullOrWhiteSpace(catInput))
            {
                category = oldProduct?.Category ?? BO.Categories.DOGS; // ברירת מחדל
            }
            else
            {
                // מנסה להמיר את הטקסט ל-Enum. אם המשתמש טעה, הוא יקבל את הערך הראשון (DOGS)
                if (!Enum.TryParse(catInput, true, out category))
                {
                    Console.WriteLine("Invalid category, setting to DOGS by default.");
                    category = BO.Categories.DOGS;
                }
            }

            // החזרת האובייקט החדש/מעודכן
            return new BO.Product
            {
                Id = id,
                Name = name,
                Price = price,
                Quantity = quantity,
                Category = category
            };
        }
        private static BO.Sale InputSale(BO.Sale? oldSale = null)
        {
            // שמירה על ה-ID המקורי אם אנחנו בעדכון
            int id = oldSale?.Id ?? 0;

            // 1. קליטת Product ID
            Console.Write($"Enter Product ID (current: {oldSale?.ProductId}): ");
            string prodIdInput = Console.ReadLine() ?? "";
            int productId = string.IsNullOrWhiteSpace(prodIdInput) ? (oldSale?.ProductId ?? 0) : int.Parse(prodIdInput);

            // 2. קליטת כמות נדרשת
            Console.Write($"Enter Required Quantity (current: {oldSale?.RequiredQuantity}): ");
            string qtyInput = Console.ReadLine() ?? "";
            int requiredQuantity = string.IsNullOrWhiteSpace(qtyInput) ? (oldSale?.RequiredQuantity ?? 0) : int.Parse(qtyInput);

            // 3. קליטת מחיר מבצע
            Console.Write($"Enter Discounted Price (current: {oldSale?.DiscountedPrice}): ");
            string priceInput = Console.ReadLine() ?? "";
            double discountedPrice = string.IsNullOrWhiteSpace(priceInput) ? (oldSale?.DiscountedPrice ?? 0) : double.Parse(priceInput);

            // 4. חברי מועדון (y/n)
            Console.Write($"Is for Club Members only? (y/n, current: {(oldSale?.IsForClubMembers == true ? "y" : "n")}): ");
            string clubInput = Console.ReadLine()?.ToLower() ?? "";
            bool isForClubMembers;
            if (string.IsNullOrWhiteSpace(clubInput))
                isForClubMembers = oldSale?.IsForClubMembers ?? false;
            else
                isForClubMembers = clubInput == "y";

            // 5. תאריך התחלה
            Console.Write($"Enter Sale Start Date (yyyy-mm-dd, current: {oldSale?.SaleStartDate:d}): ");
            string startInput = Console.ReadLine() ?? "";
            DateTime startDate;
            if (string.IsNullOrWhiteSpace(startInput))
                startDate = oldSale?.SaleStartDate ?? DateTime.Now;
            else
                DateTime.TryParse(startInput, out startDate);

            // 6. תאריך סיום
            Console.Write($"Enter Sale End Date (yyyy-mm-dd, current: {oldSale?.SaleEndDate:d}): ");
            string endInput = Console.ReadLine() ?? "";
            DateTime endDate;
            if (string.IsNullOrWhiteSpace(endInput))
                endDate = oldSale?.SaleEndDate ?? DateTime.Now.AddDays(7);
            else
                DateTime.TryParse(endInput, out endDate);

            return new BO.Sale
            {
                Id = id,
                ProductId = productId,
                RequiredQuantity = requiredQuantity,
                DiscountedPrice = discountedPrice,
                IsForClubMembers = isForClubMembers,
                SaleStartDate = startDate,
                SaleEndDate = endDate
            };
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
                        case "1": // View All
                            if (api is BL.BlApi.IProduct productApi)
                            {
                                var items = productApi.ReadAll();
                                foreach (var item in items) Console.WriteLine(item);
                            }
                            else if (api is BL.BlApi.ICustomer customerApi)
                            {
                                var items = customerApi.ReadAll();
                                foreach (var item in items) Console.WriteLine(item);
                            }
                            else if (api is BL.BlApi.ISale saleApi)
                            {
                                var items = saleApi.ReadAll();
                                foreach (var item in items) Console.WriteLine(item);
                            }
                            break;

                        case "2": // Get by ID
                            Console.Write($"Enter {entityName} ID: ");
                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                object? found = null;
                                if (api is BL.BlApi.IProduct pApi) found = pApi.Read(id);
                                else if (api is BL.BlApi.ICustomer cApi) found = cApi.Read(id);
                                else if (api is BL.BlApi.ISale sApi) found = sApi.Read(id);
                                    Console.WriteLine(found ?? "Not found");
                            }
                            break;

                        case "3": // Add
                            if (api is BL.BlApi.IProduct pApiAdd)
                            {
                                var item = InputProduct();
                                int newId = pApiAdd.Create(item);
                                Console.WriteLine($"Created with ID: {newId}");
                            }
                            else if(api is BL.BlApi.ICustomer cApiAdd)
                            {
                                var item = InputCustomer();
                                int newId = cApiAdd.Create(item);
                                Console.WriteLine($"Created with ID: {newId}");
                            }
                            else if (api is BL.BlApi.ISale sApiAdd)
                            {
                                var item = InputSale();
                                int newId = sApiAdd.Create(item);
                                Console.WriteLine($"Created with ID: {newId}");
                            }
                            break;
                        case "4": // Update
                            Console.Write($"Enter {entityName} ID to update: ");
                            if (int.TryParse(Console.ReadLine(), out int updateId))
                            {
                                if (api is BL.BlApi.IProduct pApiUpd)
                                {
                                    var oldProduct = pApiUpd.Read(updateId);
                                    Console.WriteLine("Current data: " + oldProduct);
                                    var updatedProduct = InputProduct(oldProduct); // שולחים את הישן כבסיס
                                    pApiUpd.Update(updatedProduct);
                                    Console.WriteLine("Product updated!");
                                }
                                else if (api is BL.BlApi.ICustomer cApiUpd)
                                {
                                    var oldCustomer = cApiUpd.Read(updateId);
                                    Console.WriteLine("Current data: " + oldCustomer);
                                    var updatedCustomer = InputCustomer(oldCustomer); // שולחים את הישן כבסיס
                                    cApiUpd.Update(updatedCustomer);
                                    Console.WriteLine("Customer updated!");
                                }
                                else if (api is BL.BlApi.ISale sApiUpd)
                                {
                                    var oldSale = sApiUpd.Read(updateId);
                                    Console.WriteLine("Current data: " + oldSale);
                                    var updatedSale = InputSale(oldSale); // שולחים את הישן כבסיס
                                    sApiUpd.Update(updatedSale);
                                    Console.WriteLine("Sale updated!");
                                }
                            }
                            break;
                        case "5": // Delete
                            Console.Write($"Enter {entityName} ID to delete: ");
                            if (int.TryParse(Console.ReadLine(), out int delId))
                            {
                                if (api is BL.BlApi.IProduct pApiDel) pApiDel.Delete(delId);
                                else if (api is BL.BlApi.ICustomer cApiDel) cApiDel.Delete(delId);
                                else if (api is BL.BlApi.ISale sApiDel) sApiDel.Delete(delId);
                                Console.WriteLine("Deleted successfully.");
                            }
                            break;

                        case "6": back = true; break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    if (ex.InnerException != null) Console.WriteLine($"Inner: {ex.InnerException.Message}");
                }
            }
        }
        #endregion
    }
}