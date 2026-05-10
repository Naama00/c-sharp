using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BL.BlApi;
using BL.BO; // שינוי כאן ל-BO כדי למנוע כפילויות

namespace UI
{
    public partial class ProductListForm : Form
    {
        private readonly IBl _bl = Factory.Get();
        private DataGridView dgvProducts;

        public ProductListForm()
        {
            InitializeComponentManual();
            LoadData();
        }

        private void InitializeComponentManual()
        {
            this.Text = "ניהול מלאי מוצרים";
            this.Size = new Size(850, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgvProducts = new DataGridView
            {
                Location = new Point(20, 80),
                Size = new Size(790, 400),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            // כפתור הוספה
            Button btnAdd = new Button { Text = "➕ הוסף", Location = new Point(20, 500), Size = new Size(100, 40), BackColor = Color.LightGreen };
            btnAdd.Click += (s, e) => { new ProductWindow().ShowDialog(); LoadData(); };

            // כפתור עריכה
            Button btnEdit = new Button { Text = "✏️ ערוך", Location = new Point(130, 500), Size = new Size(100, 40), BackColor = Color.LightYellow };
            btnEdit.Click += (s, e) => {
                if (dgvProducts.SelectedRows.Count > 0)
                {
                    // שימוש ב-BO.Product כדי להיות בטוחים
                    Product p = (Product)dgvProducts.SelectedRows[0].DataBoundItem;
                    new ProductWindow(p).ShowDialog();
                    LoadData();
                }
            };

            // כפתור מחיקה
            Button btnDelete = new Button { Text = "🗑️ מחק", Location = new Point(240, 500), Size = new Size(100, 40), BackColor = Color.LightCoral };
            btnDelete.Click += (s, e) => {
                if (dgvProducts.SelectedRows.Count > 0)
                {
                    // תיקון ל-DataBoundItem
                    var p = (Product)dgvProducts.SelectedRows[0].DataBoundItem;
                    if (MessageBox.Show($"למחוק את {p.Name}?", "אישור", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            _bl.Product.Delete(p.Id);
                            LoadData();
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
            };

            // כפתור חזרה
            Button btnBack = new Button { Text = "🔙 חזרה", Location = new Point(710, 500), Size = new Size(100, 40) };
            btnBack.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { dgvProducts, btnAdd, btnEdit, btnDelete, btnBack });
        }

        private void LoadData()
        {
            try
            {
                if (dgvProducts != null)
                {
                    dgvProducts.DataSource = _bl.Product.ReadAll().ToList();
                }
            }
            catch (Exception ex) { MessageBox.Show("שגיאה בטעינה: " + ex.Message); }
        }
    }
}