namespace WalletLoader
{
    partial class frmAddressChecker
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTargetBalance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.txtInputAddresses = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtResultsZero = new System.Windows.Forms.TextBox();
            this.txtResultsLow = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtResultsOK = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtResultsOver = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.lblSummaryInfo = new System.Windows.Forms.Label();
            this.chkIncludePendingBalance = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Balance";
            // 
            // txtTargetBalance
            // 
            this.txtTargetBalance.Location = new System.Drawing.Point(98, 12);
            this.txtTargetBalance.Name = "txtTargetBalance";
            this.txtTargetBalance.Size = new System.Drawing.Size(100, 20);
            this.txtTargetBalance.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "API Key";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(293, 12);
            this.txtApiKey.MaxLength = 32;
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(162, 20);
            this.txtApiKey.TabIndex = 3;
            // 
            // txtInputAddresses
            // 
            this.txtInputAddresses.Font = new System.Drawing.Font("Courier New", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputAddresses.Location = new System.Drawing.Point(12, 65);
            this.txtInputAddresses.Multiline = true;
            this.txtInputAddresses.Name = "txtInputAddresses";
            this.txtInputAddresses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInputAddresses.Size = new System.Drawing.Size(207, 331);
            this.txtInputAddresses.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Addresses To Check";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(223, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Balance = 0";
            // 
            // txtResultsZero
            // 
            this.txtResultsZero.Font = new System.Drawing.Font("Courier New", 7F);
            this.txtResultsZero.Location = new System.Drawing.Point(225, 63);
            this.txtResultsZero.Multiline = true;
            this.txtResultsZero.Name = "txtResultsZero";
            this.txtResultsZero.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultsZero.Size = new System.Drawing.Size(207, 331);
            this.txtResultsZero.TabIndex = 7;
            // 
            // txtResultsLow
            // 
            this.txtResultsLow.Font = new System.Drawing.Font("Courier New", 7F);
            this.txtResultsLow.Location = new System.Drawing.Point(438, 63);
            this.txtResultsLow.Multiline = true;
            this.txtResultsLow.Name = "txtResultsLow";
            this.txtResultsLow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultsLow.Size = new System.Drawing.Size(207, 331);
            this.txtResultsLow.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(441, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Balance < Target (but > 0)";
            // 
            // txtResultsOK
            // 
            this.txtResultsOK.Font = new System.Drawing.Font("Courier New", 7F);
            this.txtResultsOK.Location = new System.Drawing.Point(651, 63);
            this.txtResultsOK.Multiline = true;
            this.txtResultsOK.Name = "txtResultsOK";
            this.txtResultsOK.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultsOK.Size = new System.Drawing.Size(207, 331);
            this.txtResultsOK.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(648, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Balance = Target";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(861, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Balance > Target";
            // 
            // txtResultsOver
            // 
            this.txtResultsOver.Font = new System.Drawing.Font("Courier New", 7F);
            this.txtResultsOver.Location = new System.Drawing.Point(864, 63);
            this.txtResultsOver.Multiline = true;
            this.txtResultsOver.Name = "txtResultsOver";
            this.txtResultsOver.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultsOver.Size = new System.Drawing.Size(186, 331);
            this.txtResultsOver.TabIndex = 11;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(117, 402);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(80, 31);
            this.btnGo.TabIndex = 12;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblSummaryInfo
            // 
            this.lblSummaryInfo.AutoSize = true;
            this.lblSummaryInfo.Location = new System.Drawing.Point(212, 411);
            this.lblSummaryInfo.Name = "lblSummaryInfo";
            this.lblSummaryInfo.Size = new System.Drawing.Size(254, 13);
            this.lblSummaryInfo.TabIndex = 13;
            this.lblSummaryInfo.Text = "[Summary info shown here when processing is done]";
            // 
            // chkIncludePendingBalance
            // 
            this.chkIncludePendingBalance.AutoSize = true;
            this.chkIncludePendingBalance.Location = new System.Drawing.Point(474, 14);
            this.chkIncludePendingBalance.Name = "chkIncludePendingBalance";
            this.chkIncludePendingBalance.Size = new System.Drawing.Size(331, 17);
            this.chkIncludePendingBalance.TabIndex = 14;
            this.chkIncludePendingBalance.Text = "Include Pending balance value when evaluating against Target?";
            this.chkIncludePendingBalance.UseVisualStyleBackColor = true;
            // 
            // frmAddressChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 443);
            this.Controls.Add(this.chkIncludePendingBalance);
            this.Controls.Add(this.lblSummaryInfo);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.txtResultsOver);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtResultsOK);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtResultsLow);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtResultsZero);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtInputAddresses);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetBalance);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmAddressChecker";
            this.Text = "Address Checker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTargetBalance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.TextBox txtInputAddresses;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtResultsZero;
        private System.Windows.Forms.TextBox txtResultsLow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtResultsOK;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtResultsOver;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label lblSummaryInfo;
        private System.Windows.Forms.CheckBox chkIncludePendingBalance;
    }
}