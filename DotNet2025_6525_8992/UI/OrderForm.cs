using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BL.BlApi;
using BL.BO;
using Dal;
namespace UI
{
    public partial class OrderForm : Form
    {
        private readonly IBl _bl = Factory.Get();
        private List<OrderItem> _cart = new List<OrderItem>();
        private double _totalPrice = 0;

        // הגדרת הפקדים (UI Components)
        private ComboBox cmbCustomers;
        private ComboBox cmbCategoryFilter;
        private DataGridView dgvAvailableProducts;
        private DataGridView dgvCart;
        private Label lblTotal;
        private Button btnAddToCart;
        private Button btnConfirm;

        public OrderForm()
        {
            InitializeComponentManual();
            LoadInitialData();
        }

        private void InitializeComponentManual()
        {
            // הגדרות חלון ראשי
            this.Text = "מערכת הזמנות חדשה";
            this.Size = new Size(950, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245); // רקע אפור בהיר נעים

            // --- חלק עליון: בחירת לקוח וסינון ---
            Label lblCust = new Label { Text = "👤 בחר לקוח:", Location = new Point(30, 25), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            cmbCustomers = new ComboBox { Location = new Point(130, 22), Size = new Size(200, 25), DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblCat = new Label { Text = "🔍 סינון קטגוריה:", Location = new Point(500, 25), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            cmbCategoryFilter = new ComboBox { Location = new Point(620, 22), Size = new Size(180, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategoryFilter.SelectedIndexChanged += (s, e) => LoadProducts();

            // --- חלק מרכזי: כותרות לטבלאות ---
            Label lblCartTitle = new Label { Text = "🛒 עגלת הקניות שלך", Location = new Point(30, 70), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkSlateBlue };
            Label lblStoreTitle = new Label { Text = "📦 מוצרים זמינים במלאי", Location = new Point(500, 70), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkSlateBlue };

            //dgvCart.DoubleClick += DgvCart_DoubleClick;


            // --- טבלאות ---
            dgvCart = new DataGridView
            {
                Location = new Point(30, 100),
                Size = new Size(430, 400),
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            dgvAvailableProducts = new DataGridView
            {
                Location = new Point(500, 100),
                Size = new Size(400, 400),
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            // --- כפתור הוספה (ממוקם בין הטבלאות) ---
            btnAddToCart = new Button
            {
                Text = "⬅️ הוסף לעגלה",
                Location = new Point(400, 510),
                Size = new Size(130, 45),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAddToCart.Click += BtnAddToCart_Click;

            // --- סיכום ואישור (חלק תחתון) ---
            lblTotal = new Label
            {
                Text = "סה\"כ לתשלום: ₪0",
                Location = new Point(30, 515),
                AutoSize = true, // קריטי כדי שלא ייחתך מעל 1,000 ש"ח
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.ForestGreen
            };

            btnConfirm = new Button
            {
                Text = "✅ אישור ושליחת הזמנה",
                Location = new Point(30, 580),
                Size = new Size(870, 55),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnConfirm.Click += BtnConfirm_Click;

            // הוספת כל הפקדים לטופס
            this.Controls.AddRange(new Control[] {
                lblCust, cmbCustomers, lblCat, cmbCategoryFilter,
                lblCartTitle, lblStoreTitle, dgvCart, dgvAvailableProducts,
                btnAddToCart, lblTotal, btnConfirm
            });
        }


        private void LoadInitialData()
        {
            try
            {
                // 1. טעינת רשימת הלקוחות הקיימים מה-BL
                var customerList = _bl.Customer.ReadAll().ToList();

                // 2. הוספת "לקוח מזדמן" לראש הרשימה באופן ידני
                // אנחנו יוצרים אובייקט זמני עם ID 0 שלא קיים ב-XML
               

                // 3. הצגת הרשימה (כולל המזדמן) ב-ComboBox
                if (customerList.Any())
                {
                    cmbCustomers.DataSource = customerList;
                    cmbCustomers.DisplayMember = "CustomerName";
                    cmbCustomers.ValueMember = "Id";

                    // בחירה ב"לקוח מזדמן" כברירת מחדל (הוא באינדקס 0)
                    cmbCustomers.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("שגיאה: רשימת הלקוחות ריקה.");
                }

                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה: {ex.Message}");
            }
        }


        private void LoadProducts()
        {
            try
            {
                var products = _bl.Product.ReadAll();

                // סינון לפי קטגוריה
                if (cmbCategoryFilter.SelectedItem != null)
                {
                    string selectedCat = cmbCategoryFilter.SelectedItem.ToString();
                    if (selectedCat != "הכל")
                    {
                        products = products.Where(p => p.Category.ToString() == selectedCat).ToList();
                    }
                }

                // הצגה בטבלה
                dgvAvailableProducts.DataSource = products.ToList();

                // שיפור תצוגת הטבלה (אופציונלי)
                if (dgvAvailableProducts.Columns.Count > 0)
                {
                    dgvAvailableProducts.Columns["Id"].HeaderText = "קוד מוצר";
                    dgvAvailableProducts.Columns["Name"].HeaderText = "שם מוצר";
                    dgvAvailableProducts.Columns["Price"].HeaderText = "מחיר";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בטעינת מוצרים: {ex.Message}");
            }
        }

        
        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvAvailableProducts.SelectedRows.Count > 0)
            {
                var p = (Product)dgvAvailableProducts.SelectedRows[0].DataBoundItem;

                // בדיקה האם נשאר מלאי
                if (p.Quantity <= 0)
                {
                    MessageBox.Show("מצטערים, המוצר אזל מהמלאי!", "אזל המלאי", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // הורדת יחידה אחת מהמלאי המוצג בטבלה
                p.Quantity--;

                var existing = _cart.FirstOrDefault(i => i.ProductId == p.Id);
                if (existing != null)
                {
                    existing.Quantity++;
                }
                else
                {
                    var cust = (Customer)cmbCustomers.SelectedItem;
                    bool isMember = cust != null && cust.IsClubMember;

                    // קריאה ל-BL לקבלת המחיר הנכון (אחרי הנחה אם יש)
                    double finalPrice = _bl.Sale.GetEffectivePrice(p.Id, isMember);
                    _cart.Add(new OrderItem
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        PricePerUnit = finalPrice,
                        Quantity = 1
                    });
                }

                RefreshCart();
                dgvAvailableProducts.Refresh(); // מעדכן ויזואלית את המספר בטבלה הימנית
            }
        }

        private void RefreshCart()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = _cart.ToList(); // יציג אוטומטית את TotalPrice מה-BO
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            _totalPrice = _cart.Sum(item => item.PricePerUnit * item.Quantity);
            lblTotal.Text = $"סה\"כ לתשלום: {_totalPrice:C2}";
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (cmbCustomers.SelectedItem == null) { MessageBox.Show("בבקשה בחר לקוח!"); return; }
            if (!_cart.Any()) { MessageBox.Show("העגלה ריקה!"); return; }

            try
            {
                var cust = (Customer)cmbCustomers.SelectedItem;
                _bl.Order.DoOrder(new Order
                {
                    CustomerId = cust.Id,
                    Items = _cart,
                    TotalPrice = _totalPrice,
                    OrderDate = DateTime.Now
                });

                MessageBox.Show("ההזמנה נקלטה בהצלחה!");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}