using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class LoginForm : Form
    {
        public bool IsAdmin { get; private set; }
        public string CustomerName { get; private set; } = "";

        private TextBox txtInput;
        private Label lblInputHint;
        private Button btnConfirm;
        private const string AdminPassword = "1234";
        public LoginForm()
        {
            InitializeLoginCustom();
        }

        private void InitializeLoginCustom()
        {
            this.Text = "כניסה למערכת";
            this.Size = new Size(350, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblSelect = new Label
            {
                Text = "בחר סוג משתמש:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnCustomer = new Button
            {
                Text = "לקוח",
                Size = new Size(130, 40),
                Location = new Point(30, 70),
                BackColor = Color.LightGreen
            };

            Button btnAdminChoice = new Button
            {
                Text = "מנהל מערכת",
                Size = new Size(130, 40),
                Location = new Point(180, 70),
                BackColor = Color.LightCoral
            };

            // שדה קלט שיופיע לאחר הבחירה
            lblInputHint = new Label
            {
                Text = "בחר סוג משתמש למעלה",
                Location = new Point(30, 130),
                Size = new Size(280, 25),
                Visible = false
            };

            txtInput = new TextBox
            {
                Location = new Point(30, 160),
                Size = new Size(280, 30),
                Visible = false,
                Font = new Font("Segoe UI", 12)
            };

            btnConfirm = new Button
            {
                Text = "המשך",
                Size = new Size(280, 45),
                Location = new Point(30, 210),
                Visible = false,
                BackColor = Color.LightBlue
            };

            // אירועים לבחירת סוג משתמש
            btnCustomer.Click += (s, e) => {
                IsAdmin = false;
                lblInputHint.Text = "נא להזין שם לקוח:";
                lblInputHint.Visible = true;
                txtInput.Visible = true;
                txtInput.PasswordChar = '\0'; // טקסט גלוי
                txtInput.Clear();
                btnConfirm.Visible = true;
            };

            btnAdminChoice.Click += (s, e) => {
                IsAdmin = true;
                lblInputHint.Text = "נא להזין סיסמת מנהל:";
                lblInputHint.Visible = true;
                txtInput.Visible = true;
                txtInput.PasswordChar = '*'; // הסתרת סיסמה
                txtInput.Clear();
                btnConfirm.Visible = true;
            };

            // אירוע אישור סופי
            btnConfirm.Click += (s, e) => {
                if (IsAdmin)
                {
                    if (txtInput.Text == AdminPassword)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("סיסמה שגויה! הגישה נדחתה.", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(txtInput.Text))
                    {
                        CustomerName = txtInput.Text;
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("נא להזין שם כדי להמשיך.", "מידע חסר", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            };

            this.Controls.AddRange(new Control[] { lblSelect, btnCustomer, btnAdminChoice, lblInputHint, txtInput, btnConfirm });
        }
    }
}