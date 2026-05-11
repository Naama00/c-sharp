using BL.BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class OrderTrackingForm : Form
    {
        private readonly IBl _bl = Factory.Get();

        public OrderTrackingForm()
        {
            InitializeComponent();
        }

        private void btnTrack_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtOrderId.Text, out int orderId))
            {
                MessageBox.Show("נא להזין מספר הזמנה תקין (מספרים בלבד)");
                return;
            }

            try
            {
                var order = _bl.Order.GetOrderDetails(orderId);

                if (order == null)
                {
                    MessageBox.Show("הזמנה לא נמצאה.");
                    ClearFields();
                    return;
                }

                // הצגת הנתונים
                lblId.Text = $"מספר הזמנה: {order.Id}";
                lblDate.Text = $"תאריך: {order.OrderDate:dd/MM/yyyy}";
                lblTotal.Text = $"סכום: {order.TotalPrice:C2}";

                // מילוי הטבלה בפרטים בסיסיים
                dgvOrderDetails.DataSource = new List<object>
                {
                    new { פרמטר = "קוד לקוח", ערך = order.CustomerId },
                    new { פרמטר = "סטטוס", ערך = "אושר" },
                    new { פרמטר = "תאריך יצירה", ערך = order.OrderDate }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה: {ex.Message}");
            }
        }

        private void ClearFields()
        {
            lblId.Text = "---";
            lblDate.Text = "---";
            lblTotal.Text = "---";
            dgvOrderDetails.DataSource = null;
        }
    }
}