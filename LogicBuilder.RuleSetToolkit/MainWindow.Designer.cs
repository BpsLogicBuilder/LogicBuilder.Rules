namespace LogicBuilder.RuleSetToolkit
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.rulesGroupBox = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rulesListView = new System.Windows.Forms.ListView();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.priorityColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.reevaluationCountColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.activeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rulePreviewColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxEditRuleSets = new System.Windows.Forms.GroupBox();
            this.groupBoxErrors = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelErrors = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.platformComboBox = new System.Windows.Forms.ComboBox();
            this.lblActivityClass = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.txtActivityClass = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtActivityAssembly = new System.Windows.Forms.TextBox();
            this.btnBrowseRuleset = new System.Windows.Forms.Button();
            this.lblActivityAssembly = new System.Windows.Forms.Label();
            this.lblRuleset = new System.Windows.Forms.Label();
            this.btnBrowseActivityAssembly = new System.Windows.Forms.Button();
            this.txtRuleset = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.rulesGroupBox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBoxEditRuleSets.SuspendLayout();
            this.groupBoxErrors.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(809, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 554);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(809, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rulesGroupBox);
            this.panel1.Controls.Add(this.groupBoxEditRuleSets);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 477);
            this.panel1.TabIndex = 1;
            // 
            // rulesGroupBox
            // 
            this.rulesGroupBox.Controls.Add(this.panel2);
            this.rulesGroupBox.Location = new System.Drawing.Point(12, 254);
            this.rulesGroupBox.Name = "rulesGroupBox";
            this.rulesGroupBox.Size = new System.Drawing.Size(785, 194);
            this.rulesGroupBox.TabIndex = 1;
            this.rulesGroupBox.TabStop = false;
            this.rulesGroupBox.Text = "Rule Set";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rulesListView);
            this.panel2.Location = new System.Drawing.Point(10, 17);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(758, 169);
            this.panel2.TabIndex = 0;
            // 
            // rulesListView
            // 
            this.rulesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.priorityColumnHeader,
            this.reevaluationCountColumnHeader,
            this.activeColumnHeader,
            this.rulePreviewColumnHeader});
            this.rulesListView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rulesListView.FullRowSelect = true;
            this.rulesListView.HideSelection = false;
            this.rulesListView.Location = new System.Drawing.Point(0, 30);
            this.rulesListView.MultiSelect = false;
            this.rulesListView.Name = "rulesListView";
            this.rulesListView.Size = new System.Drawing.Size(758, 139);
            this.rulesListView.TabIndex = 0;
            this.rulesListView.UseCompatibleStateImageBehavior = false;
            this.rulesListView.View = System.Windows.Forms.View.Details;
            this.rulesListView.SizeChanged += new System.EventHandler(this.RulesListView_SizeChanged);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Name = "nameColumnHeader";
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 102;
            // 
            // priorityColumnHeader
            // 
            this.priorityColumnHeader.Text = "Priority";
            // 
            // reevaluationCountColumnHeader
            // 
            this.reevaluationCountColumnHeader.Text = "Reevaluation";
            this.reevaluationCountColumnHeader.Width = 92;
            // 
            // activeColumnHeader
            // 
            this.activeColumnHeader.Text = "Active";
            this.activeColumnHeader.Width = 67;
            // 
            // rulePreviewColumnHeader
            // 
            this.rulePreviewColumnHeader.Text = "Rule Preview";
            this.rulePreviewColumnHeader.Width = 413;
            // 
            // groupBoxEditRuleSets
            // 
            this.groupBoxEditRuleSets.Controls.Add(this.btnNew);
            this.groupBoxEditRuleSets.Controls.Add(this.groupBoxErrors);
            this.groupBoxEditRuleSets.Controls.Add(this.btnSave);
            this.groupBoxEditRuleSets.Controls.Add(this.btnLoad);
            this.groupBoxEditRuleSets.Controls.Add(this.lblPlatform);
            this.groupBoxEditRuleSets.Controls.Add(this.platformComboBox);
            this.groupBoxEditRuleSets.Controls.Add(this.lblActivityClass);
            this.groupBoxEditRuleSets.Controls.Add(this.btnEdit);
            this.groupBoxEditRuleSets.Controls.Add(this.txtActivityClass);
            this.groupBoxEditRuleSets.Controls.Add(this.panel3);
            this.groupBoxEditRuleSets.Controls.Add(this.txtActivityAssembly);
            this.groupBoxEditRuleSets.Controls.Add(this.btnBrowseRuleset);
            this.groupBoxEditRuleSets.Controls.Add(this.lblActivityAssembly);
            this.groupBoxEditRuleSets.Controls.Add(this.lblRuleset);
            this.groupBoxEditRuleSets.Controls.Add(this.btnBrowseActivityAssembly);
            this.groupBoxEditRuleSets.Controls.Add(this.txtRuleset);
            this.groupBoxEditRuleSets.Location = new System.Drawing.Point(12, 3);
            this.groupBoxEditRuleSets.Name = "groupBoxEditRuleSets";
            this.groupBoxEditRuleSets.Size = new System.Drawing.Size(785, 245);
            this.groupBoxEditRuleSets.TabIndex = 0;
            this.groupBoxEditRuleSets.TabStop = false;
            this.groupBoxEditRuleSets.Text = "Edit Rule Set";
            // 
            // groupBoxErrors
            // 
            this.groupBoxErrors.Controls.Add(this.flowLayoutPanel1);
            this.groupBoxErrors.Location = new System.Drawing.Point(12, 120);
            this.groupBoxErrors.Name = "groupBoxErrors";
            this.groupBoxErrors.Size = new System.Drawing.Size(447, 114);
            this.groupBoxErrors.TabIndex = 9;
            this.groupBoxErrors.TabStop = false;
            this.groupBoxErrors.Text = "Errors";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.labelErrors);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(441, 95);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // labelErrors
            // 
            this.labelErrors.AutoSize = true;
            this.labelErrors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelErrors.Location = new System.Drawing.Point(3, 0);
            this.labelErrors.Name = "labelErrors";
            this.labelErrors.Size = new System.Drawing.Size(0, 13);
            this.labelErrors.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(693, 211);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLoad.Location = new System.Drawing.Point(693, 182);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 12;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // lblPlatform
            // 
            this.lblPlatform.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPlatform.Location = new System.Drawing.Point(465, 216);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(85, 13);
            this.lblPlatform.TabIndex = 13;
            this.lblPlatform.Text = "P&latform:";
            this.lblPlatform.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // platformComboBox
            // 
            this.platformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platformComboBox.FormattingEnabled = true;
            this.platformComboBox.Location = new System.Drawing.Point(556, 213);
            this.platformComboBox.Name = "platformComboBox";
            this.platformComboBox.Size = new System.Drawing.Size(131, 21);
            this.platformComboBox.TabIndex = 14;
            // 
            // lblActivityClass
            // 
            this.lblActivityClass.AutoSize = true;
            this.lblActivityClass.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblActivityClass.Location = new System.Drawing.Point(9, 37);
            this.lblActivityClass.Name = "lblActivityClass";
            this.lblActivityClass.Size = new System.Drawing.Size(72, 13);
            this.lblActivityClass.TabIndex = 0;
            this.lblActivityClass.Text = "Activity Class:";
            // 
            // btnEdit
            // 
            this.btnEdit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEdit.Location = new System.Drawing.Point(693, 153);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.Text = "Edit...";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // txtActivityClass
            // 
            this.txtActivityClass.Location = new System.Drawing.Point(116, 37);
            this.txtActivityClass.Name = "txtActivityClass";
            this.txtActivityClass.Size = new System.Drawing.Size(573, 20);
            this.txtActivityClass.TabIndex = 1;
            this.txtActivityClass.TextChanged += new System.EventHandler(this.TxtActivityClass_TextChanged);
            this.txtActivityClass.Validated += new System.EventHandler(this.TxtActivityClass_Validated);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(13, 115);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(755, 2);
            this.panel3.TabIndex = 8;
            // 
            // txtActivityAssembly
            // 
            this.txtActivityAssembly.Location = new System.Drawing.Point(116, 63);
            this.txtActivityAssembly.Name = "txtActivityAssembly";
            this.txtActivityAssembly.Size = new System.Drawing.Size(573, 20);
            this.txtActivityAssembly.TabIndex = 3;
            this.txtActivityAssembly.TextChanged += new System.EventHandler(this.TxtActivityAssembly_TextChanged);
            this.txtActivityAssembly.Validated += new System.EventHandler(this.TxtActivityAssembly_Validated);
            // 
            // btnBrowseRuleset
            // 
            this.btnBrowseRuleset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowseRuleset.Location = new System.Drawing.Point(695, 87);
            this.btnBrowseRuleset.Name = "btnBrowseRuleset";
            this.btnBrowseRuleset.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseRuleset.TabIndex = 7;
            this.btnBrowseRuleset.Text = "Browse...";
            this.btnBrowseRuleset.UseVisualStyleBackColor = true;
            this.btnBrowseRuleset.Click += new System.EventHandler(this.BtnBrowseRuleset_Click);
            // 
            // lblActivityAssembly
            // 
            this.lblActivityAssembly.AutoSize = true;
            this.lblActivityAssembly.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblActivityAssembly.Location = new System.Drawing.Point(9, 63);
            this.lblActivityAssembly.Name = "lblActivityAssembly";
            this.lblActivityAssembly.Size = new System.Drawing.Size(91, 13);
            this.lblActivityAssembly.TabIndex = 2;
            this.lblActivityAssembly.Text = "Activity Assembly:";
            // 
            // lblRuleset
            // 
            this.lblRuleset.AutoSize = true;
            this.lblRuleset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblRuleset.Location = new System.Drawing.Point(9, 89);
            this.lblRuleset.Name = "lblRuleset";
            this.lblRuleset.Size = new System.Drawing.Size(46, 13);
            this.lblRuleset.TabIndex = 5;
            this.lblRuleset.Text = "Ruleset:";
            // 
            // btnBrowseActivityAssembly
            // 
            this.btnBrowseActivityAssembly.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowseActivityAssembly.Location = new System.Drawing.Point(695, 61);
            this.btnBrowseActivityAssembly.Name = "btnBrowseActivityAssembly";
            this.btnBrowseActivityAssembly.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseActivityAssembly.TabIndex = 4;
            this.btnBrowseActivityAssembly.Text = "Browse...";
            this.btnBrowseActivityAssembly.UseVisualStyleBackColor = true;
            this.btnBrowseActivityAssembly.Click += new System.EventHandler(this.BtnBrowseActivityAssembly_Click);
            // 
            // txtRuleset
            // 
            this.txtRuleset.Location = new System.Drawing.Point(116, 89);
            this.txtRuleset.Name = "txtRuleset";
            this.txtRuleset.Size = new System.Drawing.Size(573, 20);
            this.txtRuleset.TabIndex = 6;
            this.txtRuleset.TextChanged += new System.EventHandler(this.TxtRuleset_TextChanged);
            this.txtRuleset.Validated += new System.EventHandler(this.TxtRuleset_Validated);
            // 
            // btnNew
            // 
            this.btnNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNew.Location = new System.Drawing.Point(693, 124);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 10;
            this.btnNew.Text = "New...";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.BtnNew_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 576);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Rule Sets";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.rulesGroupBox.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBoxEditRuleSets.ResumeLayout(false);
            this.groupBoxEditRuleSets.PerformLayout();
            this.groupBoxErrors.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBrowseRuleset;
        private System.Windows.Forms.Label lblRuleset;
        private System.Windows.Forms.TextBox txtRuleset;
        private System.Windows.Forms.Button btnBrowseActivityAssembly;
        private System.Windows.Forms.Label lblActivityAssembly;
        private System.Windows.Forms.Label lblActivityClass;
        private System.Windows.Forms.TextBox txtActivityAssembly;
        private System.Windows.Forms.TextBox txtActivityClass;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBoxEditRuleSets;
        private System.Windows.Forms.GroupBox rulesGroupBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView rulesListView;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader priorityColumnHeader;
        private System.Windows.Forms.ColumnHeader reevaluationCountColumnHeader;
        private System.Windows.Forms.ColumnHeader activeColumnHeader;
        private System.Windows.Forms.ColumnHeader rulePreviewColumnHeader;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.ComboBox platformComboBox;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBoxErrors;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label labelErrors;
        private System.Windows.Forms.Button btnNew;
    }
}



