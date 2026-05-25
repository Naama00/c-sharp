using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class MainWindow : Form
    {
        // הגדרת הפקדים כמשתני מחלקה
        private Label lblTitle;
        private Button btnNewOrder;
        private Button btnTrackOrder;
        private Button btnAdminProducts; // ניהול מוצרים
        private Button btnCustomers;      // ניהול לקוחות
        private Button btnSales;          // ניהול מבצעים

        private bool _isAdminMode;

        public MainWindow(bool isAdmin)
        {
            _isAdminMode = isAdmin;
            InitializeComponentCustom();
            ApplyPermissions();
        }

        private void ApplyPermissions()
        {
            if (_isAdminMode)
            {
                // מצב מנהל: כפתורי הקופאי/לקוח פעילים, וכפתורי הניהול גלויים
                btnNewOrder.Visible = false;
                btnAdminProducts.Visible = true;
                btnCustomers.Visible = true;
                btnSales.Visible = true;
            }
            else
            {
                // מצב לקוח/קופאי רגיל: מסתירים את כל אפשרויות הניהול
                btnAdminProducts.Visible = false;
                btnCustomers.Visible = false;
                btnSales.Visible = false;
            }
        }

        private void InitializeComponentCustom()
        {
            // הגדרות הטופס
            this.Text = "Pet Store System - Main";
            this.Size = new Size(450, 650); // הגדלתי מעט את הגובה כדי להכיל את כל הכפתורים
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // 1. כותרת ראשית
            lblTitle = new Label
            {
                Text = "🐾 חנות החיות שלי 🐾",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 60),
                Location = new Point(25, 30),
                ForeColor = Color.DarkSlateBlue
            };

            // 2. כפתור הזמנה חדשה
            btnNewOrder = new Button
            {
                Text = "🛒 הזמנה חדשה",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Size = new Size(300, 70),
                Location = new Point(75, 110),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            btnNewOrder.Click += (s, e) => new OrderForm().Show();

            // 3. כפתור מעקב הזמנה
            btnTrackOrder = new Button
            {
                Text = "🔍 מעקב הזמנה",
                Font = new Font("Segoe UI", 12),
                Size = new Size(300, 60),
                Location = new Point(75, 210),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnTrackOrder.Click += (s, e) => new OrderTrackingForm().ShowDialog();

            // --- אזור ניהול (רק למנהל) ---

            // 4. כפתור ניהול מוצרים
            btnAdminProducts = new Button
            {
                Text = "📦 ניהול מוצרים",
                Font = new Font("Segoe UI", 12),
                Size = new Size(300, 60),
                Location = new Point(75, 300),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };
            btnAdminProducts.Click += (s, e) => new ProductListForm().Show();

            // 5. כפתור ניהול לקוחות
            btnCustomers = new Button
            {
                Text = "👥 ניהול לקוחות",
                Font = new Font("Segoe UI", 12),
                Size = new Size(300, 60),
                Location = new Point(75, 370),
                BackColor = Color.Khaki,
                FlatStyle = FlatStyle.Flat
            };
            btnCustomers.Click += (s, e) => new CustomerListForm().ShowDialog();

            // 6. כפתור ניהול מבצעים
            btnSales = new Button
            {
                Text = "🏷️ ניהול מבצעים",
                Font = new Font("Segoe UI", 12),
                Size = new Size(300, 60),
                Location = new Point(75, 440),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnSales.Click += (s, e) => new SaleListForm().ShowDialog();

            // הוספת הפקדים לטופס
            this.Controls.Add(lblTitle);
            this.Controls.Add(btnNewOrder);
            this.Controls.Add(btnTrackOrder);
            this.Controls.Add(btnAdminProducts);
            this.Controls.Add(btnCustomers);
            this.Controls.Add(btnSales);
        }
    }
}