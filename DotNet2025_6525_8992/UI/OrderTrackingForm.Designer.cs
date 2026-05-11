namespace UI
{
    partial class OrderTrackingForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtOrderId;
        private System.Windows.Forms.Button btnTrack;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridView dgvOrderDetails;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtOrderId = new System.Windows.Forms.TextBox();
            this.btnTrack = new System.Windows.Forms.Button();
            this.lblId = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.dgvOrderDetails = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetails)).BeginInit();
            this.SuspendLayout();

            // txtOrderId
            this.txtOrderId.Location = new System.Drawing.Point(30, 30);
            this.txtOrderId.Name = "txtOrderId";
            this.txtOrderId.Size = new System.Drawing.Size(150, 23);
            this.txtOrderId.PlaceholderText = "הכנס מספר הזמנה";

            // btnTrack
            this.btnTrack.Location = new System.Drawing.Point(190, 28);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(100, 27);
            this.btnTrack.Text = "עקוב";
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);

            // lblId, lblDate, lblTotal (סידור בסיסי)
            this.lblId.Location = new System.Drawing.Point(30, 70);
            this.lblId.Size = new System.Drawing.Size(200, 20);
            this.lblId.Text = "מספר הזמנה: ---";

            this.lblDate.Location = new System.Drawing.Point(30, 100);
            this.lblDate.Size = new System.Drawing.Size(200, 20);
            this.lblDate.Text = "תאריך: ---";

            this.lblTotal.Location = new System.Drawing.Point(30, 130);
            this.lblTotal.Size = new System.Drawing.Size(200, 20);
            this.lblTotal.Text = "סכום: ---";

            // dgvOrderDetails
            this.dgvOrderDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderDetails.Location = new System.Drawing.Point(30, 170);
            this.dgvOrderDetails.Name = "dgvOrderDetails";
            this.dgvOrderDetails.Size = new System.Drawing.Size(350, 150);

            // OrderTrackingForm
            this.ClientSize = new System.Drawing.Size(420, 350);
            this.Controls.Add(this.dgvOrderDetails);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.btnTrack);
            this.Controls.Add(this.txtOrderId);
            this.Name = "OrderTrackingForm";
            this.Text = "מעקב הזמנה - Pet Store";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}