using BL.BlApi;
using BL.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
namespace UI
{
    public partial class OrderForm : Form
    {
        private System.Windows.Forms.ComboBox cmbCustomers;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnConfirmOrder;
        private System.Windows.Forms.Button btnAddToCart;
        private readonly IBl _bl = Factory.Get(); // קבלת ה-Logic Layer

        // רשימה דינמית לסל הקניות שתעדכן את הטבלה אוטומטית
        private BindingList<OrderItem> _cart = new BindingList<OrderItem>();
        private double _totalPrice = 0;

        public OrderForm()
        {
         
            InitializeComponent();
            LoadInitialData();
            SetupCartGrid();
        }

        private void LoadInitialData()
        {
            try
            {
                // טעינת לקוחות ל-ComboBox
                cmbCustomers.DataSource = _bl.Customer.ReadAll().ToList();
                cmbCustomers.DisplayMember = "Name";
                cmbCustomers.ValueMember = "Id";

                // טעינת מוצרים לקטלוג
                dgvProducts.DataSource = _bl.Product.ReadAll().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בטעינת נתונים: {ex.Message}");
            }
        }

        private void SetupCartGrid()
        {
            dgvCart.DataSource = _cart;
            // ניתן להגדיר כאן אילו עמודות להציג ב-Grid
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem is Product selectedProduct)
            {
                // לוגיקה להוספה לסל (בדיקה אם כבר קיים, עדכון כמות וכו')
                var item = _cart.FirstOrDefault(i => i.ProductId == selectedProduct.Id);
                if (item != null)
                {
                    item.Quantity++;
                    // רענון התצוגה כיוון ששינינו שדה פנימי
                    _cart.ResetBindings();
                }
                else
                {
                    _cart.Add(new OrderItem { ProductId = selectedProduct.Id, ProductName = selectedProduct.Name, PricePerUnit = selectedProduct.Price, Quantity = 1 });
                }
                UpdateSummary();
            }
        }

        private void UpdateSummary()
        {
            _totalPrice = _cart.Sum(i => i.PricePerUnit * i.Quantity);
            lblTotal.Text = $"סה\"כ לתשלום: {_totalPrice:C}";
        }

        private void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCustomers.SelectedValue == null)
                {
                    MessageBox.Show("אנא בחרי לקוח מהרשימה לפני אישור ההזמנה.");
                    return;
                }
                Order newOrder = new Order
                {
                    CustomerId = (int)cmbCustomers.SelectedValue,
                    Items = _cart.ToList(),
                    OrderDate = DateTime.Now
                };

                _bl.Order.DoOrder(newOrder);

                MessageBox.Show("ההזמנה בוצעה בהצלחה!", "אישור", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (BLOutOfStockException ex)
            {
                MessageBox.Show($"חוסר במלאי: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (BLIdNotFoundException ex)
            {
                MessageBox.Show($"לקוח לא נמצא: {ex.Message}", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה כללית: {ex.Message}");
            }
        }
    }
}