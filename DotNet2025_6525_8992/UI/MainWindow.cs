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
        private Button btnAdmin;
        private Button btnTrackOrder;

        public MainWindow()
        {
            InitializeComponentCustom(); // קריאה לפונקציית העיצוב שלנו
        }

        private void InitializeComponentCustom()
        {
            // הגדרות הטופס עצמו
            this.Text = "Pet Store System - Main";
            this.Size = new Size(450, 500);
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
                Location = new Point(25, 40),
                ForeColor = Color.DarkSlateBlue
            };

            // 2. כפתור הזמנה חדשה (ללקוח)
            btnNewOrder = new Button
            {
                Text = "🛒 הזמנה חדשה",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Size = new Size(300, 70),
                Location = new Point(75, 140),
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
                Location = new Point(75, 230),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnTrackOrder.Click += (s, e) => MessageBox.Show("אופציית מעקב הזמנות תתווסף בקרוב!");

            // 4. כפתור כניסת מנהל
            btnAdmin = new Button
            {
                Text = "⚙️ אזור ניהול",
                Font = new Font("Segoe UI", 12),
                Size = new Size(300, 60),
                Location = new Point(75, 320),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };
            btnAdmin.Click += (s, e) => MessageBox.Show("כניסה למנהל מערכת...");

            // הוספת הפקדים לטופס
            this.Controls.Add(lblTitle);
            this.Controls.Add(btnNewOrder);
            this.Controls.Add(btnTrackOrder);
            this.Controls.Add(btnAdmin);
        }
    }
}