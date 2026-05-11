using System;
using System.Drawing;
using System.Windows.Forms;
using BL.BlApi;
using BL.BO;

namespace UI
{
    public partial class CustomerWindow : Form
    {
        private readonly IBl _bl = Factory.Get();
        private Customer _currentCustomer;
        private bool _isUpdate = false;

        // פקדים
        private TextBox txtName, txtPhone, txtAddress;
        private CheckBox chkIsMember;
        private Button btnSave;

        // בנאי להוספת לקוח חדש
        public CustomerWindow()
        {
            _currentCustomer = new Customer();
            InitializeComponentManual();
            this.Text = "➕ צירוף לקוח חדש למועדון";
        }

        // בנאי לעדכון לקוח קיים
        public CustomerWindow(Customer customer)
        {
            _currentCustomer = customer;
            _isUpdate = true;
            InitializeComponentManual();
            FillData();
            this.Text = "✏️ עדכון פרטי לקוח";
        }

        private void InitializeComponentManual()
        {
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblName = new Label { Text = "שם מלא:", Location = new Point(30, 30), AutoSize = true };
            txtName = new TextBox { Location = new Point(30, 55), Size = new Size(320, 25) };

            Label lblPhone = new Label { Text = "טלפון:", Location = new Point(30, 100), AutoSize = true };
            txtPhone = new TextBox { Location = new Point(30, 125), Size = new Size(320, 25) };

            Label lblAddress = new Label { Text = "כתובת:", Location = new Point(30, 170), AutoSize = true };
            txtAddress = new TextBox { Location = new Point(30, 195), Size = new Size(320, 25) };

            chkIsMember = new CheckBox
            {
                Text = "האם חבר מועדון?",
                Location = new Point(30, 250),
                AutoSize = true,
                Checked = true // ברירת מחדל כשמצרפים לקוח חדש
            };

            btnSave = new Button
            {
                Text = _isUpdate ? "עדכן פרטים" : "אשר וצרף למועדון",
                Location = new Point(30, 310),
                Size = new Size(320, 45),
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { lblName, txtName, lblPhone, txtPhone, lblAddress, txtAddress, chkIsMember, btnSave });
        }

        private void FillData()
        {
            txtName.Text = _currentCustomer.CustomerName;
            txtPhone.Text = _currentCustomer.PhoneNumber;
            txtAddress.Text = _currentCustomer.Address;
            chkIsMember.Checked = _currentCustomer.IsClubMember;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // בדיקת תקינות בסיסית
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("חובה להזין שם וטלפון!", "מידע חסר", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // עדכון האובייקט מהשדות בטופס
                _currentCustomer.CustomerName = txtName.Text;
                _currentCustomer.PhoneNumber = txtPhone.Text;
                _currentCustomer.Address = txtAddress.Text;
                _currentCustomer.IsClubMember = chkIsMember.Checked;

                if (_isUpdate)
                {
                    _bl.Customer.Update(_currentCustomer);
                    MessageBox.Show("פרטי הלקוח עודכנו בהצלחה!");
                }
                else
                {
                    _bl.Customer.Create(_currentCustomer);
                    MessageBox.Show("הלקוח צורף למועדון בהצלחה!");
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