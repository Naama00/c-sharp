using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BL.BlApi;
using BL.BO;

namespace UI
{
    public partial class CustomerListForm : Form
    {
        private readonly IBl _bl = Factory.Get();
        private DataGridView dgvCustomers;

        public CustomerListForm()
        {
            InitializeComponentManual();
            LoadData();
        }

        private void InitializeComponentManual()
        {
            this.Text = "ניהול מועדון לקוחות";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgvCustomers = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(640, 300),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            Button btnAdd = new Button { Text = "👤 צרף לקוח", Location = new Point(20, 380), Size = new Size(120, 40), BackColor = Color.LightGreen };
            btnAdd.Click += (s, e) => {
                if (new CustomerWindow().ShowDialog() == DialogResult.OK)
                {
                    LoadData(); // רענון הטבלה לאחר הוספה
                }
            };
            
         

            // כפתור הסרת לקוח (מחיקה מהמועדון)
            Button btnDelete = new Button { Text = "🗑️ הסר לקוח", Location = new Point(150, 380), Size = new Size(120, 40), BackColor = Color.LightCoral };
            btnDelete.Click += (s, e) => {
                if (dgvCustomers.SelectedRows.Count > 0)
                {
                    var cust = (Customer)dgvCustomers.SelectedRows[0].DataBoundItem;
                    if (MessageBox.Show($"האם להסיר את {cust.CustomerName} מהמועדון?", "אישור", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            _bl.Customer.Delete(cust.Id);
                            LoadData();
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
            };

            Button btnBack = new Button { Text = "🔙 חזרה", Location = new Point(540, 380), Size = new Size(120, 40) };
            btnBack.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { dgvCustomers, btnAdd, btnDelete, btnBack });
        }

        private void LoadData()
        {
            try
            {
                dgvCustomers.DataSource = _bl.Customer.ReadAll().ToList();
            }
            catch (Exception ex) { MessageBox.Show("שגיאה בטעינה: " + ex.Message); }
        }
    }
}