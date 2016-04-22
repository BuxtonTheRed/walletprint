namespace WalletLoader
{
    partial class frmSender
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
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSendAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddresses = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSecretPin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkIncludeWithdrawFee = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addressCheckerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCoinType = new System.Windows.Forms.Label();
            this.ddlCoinFraction = new System.Windows.Forms.ComboBox();
            this.lblApiKeyInvalidFormat = new System.Windows.Forms.Label();
            this.ddlTakeFromLabel = new System.Windows.Forms.ComboBox();
            this.lblSetApiKey = new System.Windows.Forms.Label();
            this.btnLoadAddressesFromLog = new System.Windows.Forms.Button();
            this.ofdLoadPrinterLog = new System.Windows.Forms.OpenFileDialog();
            this.tsmiGoToWikiPage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(272, 482);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(137, 48);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Block.IO API Key";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(107, 28);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(144, 20);
            this.txtApiKey.TabIndex = 1;
            this.txtApiKey.Validating += new System.ComponentModel.CancelEventHandler(this.txtApiKey_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Amount to Send";
            // 
            // txtSendAmount
            // 
            this.txtSendAmount.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSendAmount.Location = new System.Drawing.Point(107, 127);
            this.txtSendAmount.Name = "txtSendAmount";
            this.txtSendAmount.Size = new System.Drawing.Size(144, 22);
            this.txtSendAmount.TabIndex = 4;
            this.txtSendAmount.TextChanged += new System.EventHandler(this.txtSendAmount_TextChanged);
            this.txtSendAmount.Validating += new System.ComponentModel.CancelEventHandler(this.txtSendAmount_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Recipient Addresses";
            // 
            // txtAddresses
            // 
            this.txtAddresses.Location = new System.Drawing.Point(11, 210);
            this.txtAddresses.Multiline = true;
            this.txtAddresses.Name = "txtAddresses";
            this.txtAddresses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAddresses.Size = new System.Drawing.Size(398, 266);
            this.txtAddresses.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Block.io Secret PIN";
            // 
            // txtSecretPin
            // 
            this.txtSecretPin.Location = new System.Drawing.Point(107, 80);
            this.txtSecretPin.Name = "txtSecretPin";
            this.txtSecretPin.Size = new System.Drawing.Size(144, 20);
            this.txtSecretPin.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Take From Label";
            // 
            // chkIncludeWithdrawFee
            // 
            this.chkIncludeWithdrawFee.AutoSize = true;
            this.chkIncludeWithdrawFee.Location = new System.Drawing.Point(107, 155);
            this.chkIncludeWithdrawFee.Name = "chkIncludeWithdrawFee";
            this.chkIncludeWithdrawFee.Size = new System.Drawing.Size(147, 17);
            this.chkIncludeWithdrawFee.TabIndex = 12;
            this.chkIncludeWithdrawFee.Text = "Add Withdraw Fee on top";
            this.chkIncludeWithdrawFee.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(421, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addressCheckerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "&Menu";
            // 
            // addressCheckerToolStripMenuItem
            // 
            this.addressCheckerToolStripMenuItem.Name = "addressCheckerToolStripMenuItem";
            this.addressCheckerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.addressCheckerToolStripMenuItem.Text = "Address &Checker";
            this.addressCheckerToolStripMenuItem.Click += new System.EventHandler(this.addressCheckerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(159, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.tsmiGoToWikiPage});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // lblCoinType
            // 
            this.lblCoinType.AutoSize = true;
            this.lblCoinType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCoinType.Location = new System.Drawing.Point(18, 112);
            this.lblCoinType.Name = "lblCoinType";
            this.lblCoinType.Size = new System.Drawing.Size(299, 13);
            this.lblCoinType.TabIndex = 15;
            this.lblCoinType.Text = "Note: coin type (btc/ltc/doge) is driven by the block.io api key";
            // 
            // ddlCoinFraction
            // 
            this.ddlCoinFraction.DisplayMember = "denominator";
            this.ddlCoinFraction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCoinFraction.Enabled = false;
            this.ddlCoinFraction.FormattingEnabled = true;
            this.ddlCoinFraction.Location = new System.Drawing.Point(257, 127);
            this.ddlCoinFraction.Name = "ddlCoinFraction";
            this.ddlCoinFraction.Size = new System.Drawing.Size(153, 21);
            this.ddlCoinFraction.TabIndex = 16;
            this.ddlCoinFraction.ValueMember = "denominator";
            this.ddlCoinFraction.SelectedValueChanged += new System.EventHandler(this.ddlCoinFraction_SelectedValueChanged);
            // 
            // lblApiKeyInvalidFormat
            // 
            this.lblApiKeyInvalidFormat.AutoSize = true;
            this.lblApiKeyInvalidFormat.ForeColor = System.Drawing.Color.Red;
            this.lblApiKeyInvalidFormat.Location = new System.Drawing.Point(250, 31);
            this.lblApiKeyInvalidFormat.Name = "lblApiKeyInvalidFormat";
            this.lblApiKeyInvalidFormat.Size = new System.Drawing.Size(70, 13);
            this.lblApiKeyInvalidFormat.TabIndex = 17;
            this.lblApiKeyInvalidFormat.Text = "Invalid format";
            this.lblApiKeyInvalidFormat.Visible = false;
            // 
            // ddlTakeFromLabel
            // 
            this.ddlTakeFromLabel.DisplayMember = "label";
            this.ddlTakeFromLabel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTakeFromLabel.FormattingEnabled = true;
            this.ddlTakeFromLabel.Location = new System.Drawing.Point(107, 54);
            this.ddlTakeFromLabel.Name = "ddlTakeFromLabel";
            this.ddlTakeFromLabel.Size = new System.Drawing.Size(224, 21);
            this.ddlTakeFromLabel.TabIndex = 18;
            this.ddlTakeFromLabel.ValueMember = "label";
            // 
            // lblSetApiKey
            // 
            this.lblSetApiKey.AutoSize = true;
            this.lblSetApiKey.Location = new System.Drawing.Point(337, 57);
            this.lblSetApiKey.Name = "lblSetApiKey";
            this.lblSetApiKey.Size = new System.Drawing.Size(73, 26);
            this.lblSetApiKey.TabIndex = 19;
            this.lblSetApiKey.Text = "Set API Key\r\nto load Labels\r\n";
            // 
            // btnLoadAddressesFromLog
            // 
            this.btnLoadAddressesFromLog.Location = new System.Drawing.Point(220, 178);
            this.btnLoadAddressesFromLog.Name = "btnLoadAddressesFromLog";
            this.btnLoadAddressesFromLog.Size = new System.Drawing.Size(190, 26);
            this.btnLoadAddressesFromLog.TabIndex = 20;
            this.btnLoadAddressesFromLog.Text = "Load from Wallet Printer log file...";
            this.btnLoadAddressesFromLog.UseVisualStyleBackColor = true;
            this.btnLoadAddressesFromLog.Click += new System.EventHandler(this.btnLoadAddressesFromLog_Click);
            // 
            // ofdLoadPrinterLog
            // 
            this.ofdLoadPrinterLog.DefaultExt = "txt";
            this.ofdLoadPrinterLog.Filter = ".txt files|*.txt|All files|*.*";
            this.ofdLoadPrinterLog.SupportMultiDottedExtensions = true;
            this.ofdLoadPrinterLog.Title = "Select a logfile produced by Wallet Printer";
            // 
            // tsmiGoToWikiPage
            // 
            this.tsmiGoToWikiPage.Name = "tsmiGoToWikiPage";
            this.tsmiGoToWikiPage.Size = new System.Drawing.Size(158, 22);
            this.tsmiGoToWikiPage.Text = "Go to Wiki page";
            this.tsmiGoToWikiPage.Click += new System.EventHandler(this.tsmiGoToWikiPage_Click);
            // 
            // frmSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 540);
            this.Controls.Add(this.btnLoadAddressesFromLog);
            this.Controls.Add(this.lblSetApiKey);
            this.Controls.Add(this.ddlTakeFromLabel);
            this.Controls.Add(this.lblApiKeyInvalidFormat);
            this.Controls.Add(this.ddlCoinFraction);
            this.Controls.Add(this.lblCoinType);
            this.Controls.Add(this.chkIncludeWithdrawFee);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSecretPin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAddresses);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSendAmount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmSender";
            this.Text = "Wallet Stuffer";
            this.Load += new System.EventHandler(this.frmSender_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSendAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAddresses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSecretPin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkIncludeWithdrawFee;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addressCheckerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblCoinType;
        private System.Windows.Forms.ComboBox ddlCoinFraction;
        private System.Windows.Forms.Label lblApiKeyInvalidFormat;
        private System.Windows.Forms.ComboBox ddlTakeFromLabel;
        private System.Windows.Forms.Label lblSetApiKey;
        private System.Windows.Forms.Button btnLoadAddressesFromLog;
        private System.Windows.Forms.OpenFileDialog ofdLoadPrinterLog;
        private System.Windows.Forms.ToolStripMenuItem tsmiGoToWikiPage;
    }
}

