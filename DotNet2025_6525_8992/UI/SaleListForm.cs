using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BL.BlApi;
using BL.BO;

namespace UI
{
    public partial class SaleListForm : Form
    {
        private readonly IBl _bl = Factory.Get();
        private DataGridView dgvSales;

        public SaleListForm()
        {
            InitializeComponentManual();
            LoadData();
        }

        private void InitializeComponentManual()
        {
            this.Text = "🏷️ ניהול מבצעים והנחות";
            this.Size = new Size(850, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            dgvSales = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(790, 350),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };

            // כפתור הוספת מבצע חדש
            Button btnAdd = new Button
            {
                Text = "➕ הוסף מבצע",
                Location = new Point(20, 430),
                Size = new Size(150, 45),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.Click += (s, e) => {
                if (new SaleWindow().ShowDialog() == DialogResult.OK)
                {
                    LoadData(); // רענון הטבלה כדי לראות את המבצע החדש
                }
            };

            // כפתור מחיקת מבצע
            Button btnDelete = new Button
            {
                Text = "🗑️ מחק מבצע",
                Location = new Point(180, 430),
                Size = new Size(150, 45),
                BackColor = Color.MistyRose,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.Click += (s, e) => {
                if (dgvSales.SelectedRows.Count > 0)
                {
                    var sale = (Sale)dgvSales.SelectedRows[0].DataBoundItem;
                    if (MessageBox.Show($"האם למחוק את המבצע עבור מוצר {sale.ProductId}?", "אישור מחיקה", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            _bl.Sale.Delete(sale.Id);
                            LoadData();
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
            };

            Button btnClose = new Button
            {
                Text = "יציאה",
                Location = new Point(710, 430),
                Size = new Size(100, 45)
            };
            btnClose.Click += (s, e) => this.Close();

            Label lblTitle = new Label
            {
                Text = "רשימת מבצעים פעילים במערכת",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            this.Controls.AddRange(new Control[] { dgvSales, btnAdd, btnDelete, btnClose, lblTitle });
        }

        private void LoadData()
        {
            try
            {
                // טעינת המבצעים מה-BL
                dgvSales.DataSource = _bl.Sale.ReadAll().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בטעינת מבצעים: " + ex.Message);
            }
        }
    }
}