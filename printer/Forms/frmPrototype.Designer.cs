namespace DogeAddress.Forms
{
    partial class frmPrototype
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnFirstPrototype = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTemplateFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowseTemplate = new System.Windows.Forms.Button();
            this.ofdLoadBundle = new System.Windows.Forms.OpenFileDialog();
            this.lblTemplateInfo = new System.Windows.Forms.Label();
            this.txtTemplateDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.chkOpenAfterGenerating = new System.Windows.Forms.CheckBox();
            this.chkWipeOutputAfterViewing = new System.Windows.Forms.CheckBox();
            this.sfdOutputFile = new System.Windows.Forms.SaveFileDialog();
            this.btnOpenEditor = new System.Windows.Forms.Button();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutDebuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(499, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // btnFirstPrototype
            // 
            this.btnFirstPrototype.Enabled = false;
            this.btnFirstPrototype.Location = new System.Drawing.Point(284, 198);
            this.btnFirstPrototype.Name = "btnFirstPrototype";
            this.btnFirstPrototype.Size = new System.Drawing.Size(203, 37);
            this.btnFirstPrototype.TabIndex = 1;
            this.btnFirstPrototype.Text = "Print Page of Wallet(s)";
            this.btnFirstPrototype.UseVisualStyleBackColor = true;
            this.btnFirstPrototype.Click += new System.EventHandler(this.btnFirstPrototype_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Template File";
            // 
            // txtTemplateFilePath
            // 
            this.txtTemplateFilePath.Location = new System.Drawing.Point(84, 35);
            this.txtTemplateFilePath.Name = "txtTemplateFilePath";
            this.txtTemplateFilePath.ReadOnly = true;
            this.txtTemplateFilePath.Size = new System.Drawing.Size(327, 20);
            this.txtTemplateFilePath.TabIndex = 3;
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Location = new System.Drawing.Point(417, 30);
            this.btnBrowseTemplate.Name = "btnBrowseTemplate";
            this.btnBrowseTemplate.Size = new System.Drawing.Size(70, 28);
            this.btnBrowseTemplate.TabIndex = 4;
            this.btnBrowseTemplate.Text = "Browse...";
            this.btnBrowseTemplate.UseVisualStyleBackColor = true;
            this.btnBrowseTemplate.Click += new System.EventHandler(this.btnBrowseTemplate_Click);
            // 
            // ofdLoadBundle
            // 
            this.ofdLoadBundle.DefaultExt = "walletprint.zip";
            this.ofdLoadBundle.FileName = "openFileDialog1";
            this.ofdLoadBundle.Filter = "Wallet Print Bundles|* .walletprint.zip|All Files|*.*";
            this.ofdLoadBundle.SupportMultiDottedExtensions = true;
            // 
            // lblTemplateInfo
            // 
            this.lblTemplateInfo.AutoSize = true;
            this.lblTemplateInfo.Location = new System.Drawing.Point(12, 58);
            this.lblTemplateInfo.Name = "lblTemplateInfo";
            this.lblTemplateInfo.Size = new System.Drawing.Size(112, 13);
            this.lblTemplateInfo.TabIndex = 5;
            this.lblTemplateInfo.Text = "(no template loaded...)";
            // 
            // txtTemplateDescription
            // 
            this.txtTemplateDescription.Location = new System.Drawing.Point(12, 78);
            this.txtTemplateDescription.Multiline = true;
            this.txtTemplateDescription.Name = "txtTemplateDescription";
            this.txtTemplateDescription.ReadOnly = true;
            this.txtTemplateDescription.Size = new System.Drawing.Size(475, 51);
            this.txtTemplateDescription.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Output File";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(84, 135);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(327, 20);
            this.txtOutputFile.TabIndex = 8;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Enabled = false;
            this.btnBrowseOutput.Location = new System.Drawing.Point(417, 130);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(70, 28);
            this.btnBrowseOutput.TabIndex = 4;
            this.btnBrowseOutput.Text = "Browse...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // chkOpenAfterGenerating
            // 
            this.chkOpenAfterGenerating.AutoSize = true;
            this.chkOpenAfterGenerating.Checked = true;
            this.chkOpenAfterGenerating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpenAfterGenerating.Location = new System.Drawing.Point(84, 164);
            this.chkOpenAfterGenerating.Name = "chkOpenAfterGenerating";
            this.chkOpenAfterGenerating.Size = new System.Drawing.Size(181, 17);
            this.chkOpenAfterGenerating.TabIndex = 9;
            this.chkOpenAfterGenerating.Text = "Open Output File when complete";
            this.chkOpenAfterGenerating.UseVisualStyleBackColor = true;
            this.chkOpenAfterGenerating.CheckedChanged += new System.EventHandler(this.chkOpenAfterGenerating_CheckedChanged);
            // 
            // chkWipeOutputAfterViewing
            // 
            this.chkWipeOutputAfterViewing.AutoSize = true;
            this.chkWipeOutputAfterViewing.Checked = true;
            this.chkWipeOutputAfterViewing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWipeOutputAfterViewing.Location = new System.Drawing.Point(284, 164);
            this.chkWipeOutputAfterViewing.Name = "chkWipeOutputAfterViewing";
            this.chkWipeOutputAfterViewing.Size = new System.Drawing.Size(168, 17);
            this.chkWipeOutputAfterViewing.TabIndex = 10;
            this.chkWipeOutputAfterViewing.Text = "Wipe Output File after viewing";
            this.chkWipeOutputAfterViewing.UseVisualStyleBackColor = true;
            // 
            // sfdOutputFile
            // 
            this.sfdOutputFile.DefaultExt = "pdf";
            this.sfdOutputFile.Filter = "PDF files|* .pdf";
            // 
            // btnOpenEditor
            // 
            this.btnOpenEditor.Location = new System.Drawing.Point(11, 210);
            this.btnOpenEditor.Name = "btnOpenEditor";
            this.btnOpenEditor.Size = new System.Drawing.Size(209, 26);
            this.btnOpenEditor.TabIndex = 11;
            this.btnOpenEditor.Text = "Go To Template Editor";
            this.btnOpenEditor.UseVisualStyleBackColor = true;
            this.btnOpenEditor.Click += new System.EventHandler(this.btnOpenEditor_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layoutDebuggingToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // layoutDebuggingToolStripMenuItem
            // 
            this.layoutDebuggingToolStripMenuItem.Name = "layoutDebuggingToolStripMenuItem";
            this.layoutDebuggingToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.layoutDebuggingToolStripMenuItem.Text = "Layout &Debugging";
            this.layoutDebuggingToolStripMenuItem.ToolTipText = "When enabled, boxes are drawn around text elements to help refine their position";
            this.layoutDebuggingToolStripMenuItem.Click += new System.EventHandler(this.layoutDebuggingToolStripMenuItem_Click);
            // 
            // frmPrototype
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 248);
            this.Controls.Add(this.btnOpenEditor);
            this.Controls.Add(this.chkWipeOutputAfterViewing);
            this.Controls.Add(this.chkOpenAfterGenerating);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTemplateDescription);
            this.Controls.Add(this.lblTemplateInfo);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.btnBrowseTemplate);
            this.Controls.Add(this.txtTemplateFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFirstPrototype);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmPrototype";
            this.Text = "Multi-Coin Wallet Printer";
            this.Load += new System.EventHandler(this.frmPrototype_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnFirstPrototype;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTemplateFilePath;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.OpenFileDialog ofdLoadBundle;
        private System.Windows.Forms.Label lblTemplateInfo;
        private System.Windows.Forms.TextBox txtTemplateDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.CheckBox chkOpenAfterGenerating;
        private System.Windows.Forms.CheckBox chkWipeOutputAfterViewing;
        private System.Windows.Forms.SaveFileDialog sfdOutputFile;
        private System.Windows.Forms.Button btnOpenEditor;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layoutDebuggingToolStripMenuItem;
    }
}