using System;
using System.Drawing;
using System.Windows.Forms;
using BL.BlApi;
using BL.BO; 

namespace UI
{
    public partial class ProductWindow : Form
    {
        private readonly IBl _bl = Factory.Get();
        private Product currentProduct = null;

        // הגדרת פקדים
        private TextBox txtName, txtPrice, txtQuantity;
        private ComboBox cmbCategory;
        private Button btnSave;

        // בנאי להוספה
        public ProductWindow()
        {
            InitializeComponentManual();
            btnSave.Text = "➕ הוסף מוצר";
        }

        // בנאי לעדכון
        public ProductWindow(Product product)
        {
            InitializeComponentManual();
            currentProduct = product;

            txtName.Text = product.Name;
            txtPrice.Text = product.Price.ToString();
            txtQuantity.Text = product.Quantity.ToString();
            cmbCategory.SelectedItem = product.Category.ToString();

            btnSave.Text = "💾 עדכן מוצר";
            this.Text = "עדכון מוצר: " + product.Name;
        }

        private void InitializeComponentManual()
        {
            this.Size = new Size(350, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            var lblName = new Label { Text = "שם מוצר:", Location = new Point(20, 30), AutoSize = true };
            txtName = new TextBox { Location = new Point(20, 50), Size = new Size(280, 25) };

            var lblCategory = new Label { Text = "קטגוריה:", Location = new Point(20, 90), AutoSize = true };
            cmbCategory = new ComboBox { Location = new Point(20, 110), Size = new Size(280, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategory.Items.AddRange(Enum.GetNames(typeof(Categories)));

            var lblPrice = new Label { Text = "מחיר:", Location = new Point(20, 150), AutoSize = true };
            txtPrice = new TextBox { Location = new Point(20, 170), Size = new Size(280, 25) };

            var lblQuantity = new Label { Text = "כמות:", Location = new Point(20, 210), AutoSize = true };
            txtQuantity = new TextBox { Location = new Point(20, 230), Size = new Size(280, 25) };

            btnSave = new Button { Location = new Point(20, 300), Size = new Size(280, 50), BackColor = Color.LightSkyBlue, FlatStyle = FlatStyle.Flat };
            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { lblName, txtName, lblCategory, cmbCategory, lblPrice, txtPrice, lblQuantity, txtQuantity, btnSave });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // בדיקות תקינות
            if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("חובה להזין שם!"); return; }
            if (cmbCategory.SelectedItem == null) { MessageBox.Show("חובה לבחור קטגוריה!"); return; }
            if (!double.TryParse(txtPrice.Text, out double price) || price <= 0) { MessageBox.Show("מחיר לא תקין!"); return; }
            if (!int.TryParse(txtQuantity.Text, out int qty) || qty < 0) { MessageBox.Show("כמות לא תקינה!"); return; }

            try
            {
                if (currentProduct == null) // הוספה
                {
                    _bl.Product.Create(new Product
                    {
                        Name = txtName.Text,
                        Price = price,
                        Quantity = qty,
                        Category = (Categories)Enum.Parse(typeof(Categories), cmbCategory.Text)
                    });
                    MessageBox.Show("המוצר נוסף!");
                }
                else // עדכון
                {
                    currentProduct.Name = txtName.Text;
                    currentProduct.Price = price;
                    currentProduct.Quantity = qty;
                    currentProduct.Category = (Categories)Enum.Parse(typeof(Categories), cmbCategory.Text);
                    _bl.Product.Update(currentProduct);
                    MessageBox.Show("המוצר עודכן!");
                }
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("שגיאה: " + ex.Message); }
        }
    }
}