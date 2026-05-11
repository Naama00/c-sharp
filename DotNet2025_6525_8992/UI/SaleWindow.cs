using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BL.BlApi;
using BL.BO;

namespace UI
{
    public partial class SaleWindow : Form
    {
        private readonly IBl _bl = Factory.Get();

        // פקדים
        private ComboBox cmbProducts;
        private NumericUpDown numDiscountPrice;
        private DateTimePicker dtpStart, dtpEnd;
        private CheckBox chkIsClubMember;
        private Button btnSave;

        public SaleWindow()
        {
            InitializeComponentManual();
            LoadProducts();
        }

        private void InitializeComponentManual()
        {
            this.Text = "➕ הוספת מבצע חדש";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblProd = new Label { Text = "בחר מוצר למבצע:", Location = new Point(30, 20), AutoSize = true };
            cmbProducts = new ComboBox { Location = new Point(30, 45), Size = new Size(320, 25), DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblPrice = new Label { Text = "מחיר לאחר הנחה (₪):", Location = new Point(30, 90), AutoSize = true };
            numDiscountPrice = new NumericUpDown { Location = new Point(30, 115), Size = new Size(320, 25), Maximum = 10000 };

            Label lblStart = new Label { Text = "תאריך התחלה:", Location = new Point(30, 160), AutoSize = true };
            dtpStart = new DateTimePicker { Location = new Point(30, 185), Size = new Size(320, 25) };

            Label lblEnd = new Label { Text = "תאריך סיום:", Location = new Point(30, 230), AutoSize = true };
            dtpEnd = new DateTimePicker { Location = new Point(30, 255), Size = new Size(320, 25) };

            chkIsClubMember = new CheckBox { Text = "לחברי מועדון בלבד?", Location = new Point(30, 300), AutoSize = true };

            btnSave = new Button
            {
                Text = "שמור מבצע",
                Location = new Point(30, 360),
                Size = new Size(320, 45),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { lblProd, cmbProducts, lblPrice, numDiscountPrice, lblStart, dtpStart, lblEnd, dtpEnd, chkIsClubMember, btnSave });
        }

        private void LoadProducts()
        {
            cmbProducts.DataSource = _bl.Product.ReadAll().ToList();
            cmbProducts.DisplayMember = "ProductName";
            cmbProducts.ValueMember = "Id";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dtpEnd.Value <= dtpStart.Value)
            {
                MessageBox.Show("תאריך הסיום חייב להיות אחרי תאריך ההתחלה!");
                return;
            }

            try
            {
                var selectedProd = (Product)cmbProducts.SelectedItem;

                // 1. בדיקה האם כבר קיים מבצע ב-XML עבור המוצר שנבחר
                var existingSale = _bl.Sale.ReadAll(s => s.ProductId == selectedProd.Id).FirstOrDefault();

                if (existingSale != null)
                {
                    // 2. אם קיים - מעדכנים את האובייקט הקיים בנתונים החדשים
                    existingSale.DiscountedPrice = (double)numDiscountPrice.Value;
                    existingSale.SaleStartDate = dtpStart.Value;
                    existingSale.SaleEndDate = dtpEnd.Value;
                    existingSale.IsForClubMembers = chkIsClubMember.Checked;

                    _bl.Sale.Update(existingSale);
                    MessageBox.Show("המבצע למוצר זה עודכן בהצלחה!");
                }
                else
                {
                    // 3. אם לא קיים - יוצרים מבצע חדש
                    _bl.Sale.Create(new Sale
                    {
                        ProductId = selectedProd.Id,
                        DiscountedPrice = (double)numDiscountPrice.Value,
                        SaleStartDate = dtpStart.Value,
                        SaleEndDate = dtpEnd.Value,
                        IsForClubMembers = chkIsClubMember.Checked,
                        RequiredQuantity = 1
                    });
                    MessageBox.Show("המבצע נוסף בהצלחה!");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה: {ex.Message}");
            }
        }
    }
}