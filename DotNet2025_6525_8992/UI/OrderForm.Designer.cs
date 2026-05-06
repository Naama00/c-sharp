namespace UI
{
    partial class OrderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbCustomers = new System.Windows.Forms.ComboBox();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnConfirmOrder = new System.Windows.Forms.Button();
            this.btnAddToCart = new System.Windows.Forms.Button();
           

           
            this.cmbCustomers.Location = new System.Drawing.Point(20, 20);
            this.cmbCustomers.Name = "cmbCustomers";
            this.cmbCustomers.Size = new System.Drawing.Size(200, 25);
            this.cmbCustomers.TabIndex = 0;
            this.Controls.Add(this.cmbCustomers);
            this.dgvCart.Location = new System.Drawing.Point(20, 60);
            this.dgvCart.Size = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.dgvCart);
            this.dgvProducts.Location = new System.Drawing.Point(440, 60);
            this.dgvProducts.Size = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.dgvProducts);
            this.btnAddToCart.Location = new System.Drawing.Point(20, 270);
            this.btnAddToCart.Size = new System.Drawing.Size(100, 30);
            this.btnAddToCart.Text = "Add to Cart";
            this.btnAddToCart.Click += new System.EventHandler(this.btnAddToCart_Click);
            this.Controls.Add(this.btnAddToCart);
            this.lblTotal.Location = new System.Drawing.Point(20, 310);
            this.lblTotal.Size = new System.Drawing.Size(200, 25);
            this.lblTotal.Text = "Total: $0.00";
            this.Controls.Add(this.lblTotal);
            this.btnConfirmOrder.Location = new System.Drawing.Point(20, 350);
            this.btnConfirmOrder.Size = new System.Drawing.Size(100, 30);
            this.btnConfirmOrder.Text = "Confirm Order";
            this.btnConfirmOrder.Click += new System.EventHandler(this.btnConfirmOrder_Click);
            this.Controls.Add(this.btnConfirmOrder);



            SuspendLayout();
            // 
            // OrderForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "OrderForm";
            Text = "OrderForm";
            ResumeLayout(false);
        }

        #endregion
    }
}