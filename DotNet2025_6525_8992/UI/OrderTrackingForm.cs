using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BL.BlApi;
using BL.BO;

namespace UI
{
    public partial class OrderTrackingForm : Form
    {
        // קבלת מופע של שכבת ה-BL
        private readonly IBl _bl = Factory.Get();

        // הגדרת הפקדים ידנית כדי למנוע התנגשויות עם ה-Designer
        private ComboBox _cmbOrderIds;
        private Label _lblOrderDetails;
        private DataGridView _dgvOrderItems;
        private Button _btnRefresh;

        public OrderTrackingForm()
        {
            InitializeComponentManual();
            LoadOrderIds(); // טעינת ההזמנות לרשימה בטעינת הטופס
        }

        private void InitializeComponentManual()
        {
            this.Text = "📦 מעקב הזמנות לקוחות";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // כותרת ובחירת הזמנה
            Label lblSelect = new Label { Text = "בחר מספר הזמנה:", Location = new Point(20, 25), AutoSize = true, Font = new Font("Segoe UI", 10) };
            _cmbOrderIds = new ComboBox
            {
                Location = new Point(140, 22),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbOrderIds.SelectedIndexChanged += (s, e) => DisplayOrderDetails();

            _btnRefresh = new Button
            {
                Text = "רענן רשימה",
                Location = new Point(300, 20),
                Size = new Size(100, 30),
                BackColor = Color.LightGray
            };
            _btnRefresh.Click += (s, e) => LoadOrderIds();

            // אזור פרטי הזמנה
            _lblOrderDetails = new Label
            {
                Text = "בחר הזמנה כדי לראות פרטים...",
                Location = new Point(20, 70),
                Size = new Size(540, 80),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            // טבלת פריטי ההזמנה
            _dgvOrderItems = new DataGridView
            {
                Location = new Point(20, 170),
                Size = new Size(540, 270),
                ReadOnly = true,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };

            // השתקת שגיאות תצוגה אוטומטית
            _dgvOrderItems.DataError += (s, e) => { e.ThrowException = false; };

            this.Controls.AddRange(new Control[] { lblSelect, _cmbOrderIds, _btnRefresh, _lblOrderDetails, _dgvOrderItems });
        }

        private void LoadOrderIds()
        {
            try
            {
                // קריאת כל ההזמנות מה-BL
                var orders = _bl.Order.ReadAllOrders().ToList();
                _cmbOrderIds.DataSource = orders;
                _cmbOrderIds.DisplayMember = "Id";
                _cmbOrderIds.ValueMember = "Id";

                if (orders.Count == 0)
                {
                    _lblOrderDetails.Text = "לא נמצאו הזמנות במערכת.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בטעינת הזמנות: {ex.Message}");
            }
        }

        private void DisplayOrderDetails()
        {
            // בודקים אם נבחר משהו ברשימה
            if (_cmbOrderIds.SelectedItem == null) return;

            try
            {
                // במקום להשתמש ב-SelectedValue, אנחנו לוקחים את האובייקט שנבחר
                // ומוציאים ממנו ישירות את ה-Id
                var selectedOrder = (Order)_cmbOrderIds.SelectedItem;
                int orderId = selectedOrder.Id;

                var order = _bl.Order.GetOrderDetails(orderId);

                if (order != null)
                {
                    MessageBox.Show($"נמצאו {order.Items.Count()} פריטים להזמנה זו");
                    _lblOrderDetails.Text = $"הזמנה מספר: {order.Id}\n" +
                                           $"תאריך: {order.OrderDate:dd/MM/yyyy HH:mm}\n" +                  
                                           $"סה\"כ לתשלום: ₪{order.TotalPrice:N2}";

                    _dgvOrderItems.DataSource = null;
                    _dgvOrderItems.DataSource = order.Items.Select(item => new
                    {
                        קוד = item.ProductId,
                        מוצר = item.ProductName,
                        כמות = item.Quantity,
                        מחיר = $"₪{item.PricePerUnit:N2}",
                        סה_כ = $"₪{(item.Quantity * item.PricePerUnit):N2}"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בהצגת פרטי הזמנה: {ex.Message}");
            }
        }
    }
}