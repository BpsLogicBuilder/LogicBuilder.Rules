using System;

namespace LogicBuilder.Workflow.Activities.Rules.Design
{
    partial class RuleSetDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleSetDialog));
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rulesListView = new System.Windows.Forms.ListView();
            this.priorityColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.reevaluationCountColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.activeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rulePreviewColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rulesGroupBox = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chainingLabel = new System.Windows.Forms.Label();
            this.chainingBehaviourComboBox = new System.Windows.Forms.ComboBox();
            this.rulesToolStrip = new System.Windows.Forms.ToolStrip();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.newRuleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.ruleGroupBox = new System.Windows.Forms.GroupBox();
            this.reevaluationComboBox = new System.Windows.Forms.ComboBox();
            this.elseTextBox = new LogicBuilder.Workflow.Activities.Rules.Design.IntellisenseTextBox();
            this.elseLabel = new System.Windows.Forms.Label();
            this.thenTextBox = new LogicBuilder.Workflow.Activities.Rules.Design.IntellisenseTextBox();
            this.thenLabel = new System.Windows.Forms.Label();
            this.conditionTextBox = new LogicBuilder.Workflow.Activities.Rules.Design.IntellisenseTextBox();
            this.conditionLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.activeCheckBox = new System.Windows.Forms.CheckBox();
            this.reevaluationLabel = new System.Windows.Forms.Label();
            this.priorityTextBox = new System.Windows.Forms.TextBox();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.headerTextLabel = new System.Windows.Forms.Label();
            this.pictureBoxHeader = new System.Windows.Forms.PictureBox();
            this.okCancelTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.conditionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.thenErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.elseErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.rulesGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.rulesToolStrip.SuspendLayout();
            this.ruleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).BeginInit();
            this.okCancelTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.conditionErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thenErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.elseErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Name = "nameColumnHeader";
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 102;
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
            this.rulesListView.Location = new System.Drawing.Point(0, 27);
            this.rulesListView.MultiSelect = false;
            this.rulesListView.Name = "rulesListView";
            this.rulesListView.Size = new System.Drawing.Size(652, 139);
            this.rulesListView.TabIndex = 3;
            this.rulesListView.UseCompatibleStateImageBehavior = false;
            this.rulesListView.View = System.Windows.Forms.View.Details;
            this.rulesListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.RulesListView_ColumnClick);
            this.rulesListView.SelectedIndexChanged += new System.EventHandler(this.RulesListView_SelectedIndexChanged);
            // 
            // priorityColumnHeader
            // 
            this.priorityColumnHeader.Text = "Priority";
            // 
            // reevaluationCountColumnHeader
            // 
            this.reevaluationCountColumnHeader.Text = "Reevaluation";
            this.reevaluationCountColumnHeader.Width = 91;
            // 
            // activeColumnHeader
            // 
            this.activeColumnHeader.Text = "Active";
            this.activeColumnHeader.Width = 59;
            // 
            // rulePreviewColumnHeader
            // 
            this.rulePreviewColumnHeader.Text = "Rule Preview";
            this.rulePreviewColumnHeader.Width = 211;
            // 
            // rulesGroupBox
            // 
            this.rulesGroupBox.Controls.Add(this.panel1);
            this.rulesGroupBox.Location = new System.Drawing.Point(14, 49);
            this.rulesGroupBox.Name = "rulesGroupBox";
            this.rulesGroupBox.Size = new System.Drawing.Size(668, 188);
            this.rulesGroupBox.TabIndex = 1;
            this.rulesGroupBox.TabStop = false;
            this.rulesGroupBox.Text = "Rule Set";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chainingLabel);
            this.panel1.Controls.Add(this.chainingBehaviourComboBox);
            this.panel1.Controls.Add(this.rulesToolStrip);
            this.panel1.Controls.Add(this.rulesListView);
            this.panel1.Location = new System.Drawing.Point(10, 17);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 166);
            this.panel1.TabIndex = 10;
            // 
            // chainingLabel
            // 
            this.chainingLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chainingLabel.Location = new System.Drawing.Point(349, 3);
            this.chainingLabel.Name = "chainingLabel";
            this.chainingLabel.Size = new System.Drawing.Size(105, 13);
            this.chainingLabel.TabIndex = 1;
            this.chainingLabel.Text = "C&haining:";
            this.chainingLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chainingBehaviourComboBox
            // 
            this.chainingBehaviourComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chainingBehaviourComboBox.FormattingEnabled = true;
            this.chainingBehaviourComboBox.Location = new System.Drawing.Point(457, 0);
            this.chainingBehaviourComboBox.Name = "chainingBehaviourComboBox";
            this.chainingBehaviourComboBox.Size = new System.Drawing.Size(192, 21);
            this.chainingBehaviourComboBox.TabIndex = 2;
            this.chainingBehaviourComboBox.SelectedIndexChanged += new System.EventHandler(this.ChainingBehaviourComboBox_SelectedIndexChanged);
            // 
            // rulesToolStrip
            // 
            this.rulesToolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.rulesToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.rulesToolStrip.ImageList = this.imageList;
            this.rulesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newRuleToolStripButton,
            this.toolStripSeparator1,
            this.deleteToolStripButton});
            this.rulesToolStrip.Location = new System.Drawing.Point(0, 0);
            this.rulesToolStrip.Name = "rulesToolStrip";
            this.rulesToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.rulesToolStrip.Size = new System.Drawing.Size(652, 25);
            this.rulesToolStrip.TabIndex = 0;
            this.rulesToolStrip.TabStop = true;
            this.rulesToolStrip.Text = "toolStrip1";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "NewRule.bmp");
            this.imageList.Images.SetKeyName(1, "RenameRule.bmp");
            this.imageList.Images.SetKeyName(2, "Delete.bmp");
            // 
            // newRuleToolStripButton
            // 
            this.newRuleToolStripButton.ImageIndex = 0;
            this.newRuleToolStripButton.Name = "newRuleToolStripButton";
            this.newRuleToolStripButton.Size = new System.Drawing.Size(75, 22);
            this.newRuleToolStripButton.Text = "&Add Rule";
            this.newRuleToolStripButton.Click += new System.EventHandler(this.NewRuleToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // deleteToolStripButton
            // 
            this.deleteToolStripButton.ImageIndex = 2;
            this.deleteToolStripButton.Name = "deleteToolStripButton";
            this.deleteToolStripButton.Size = new System.Drawing.Size(60, 22);
            this.deleteToolStripButton.Text = "&Delete";
            this.deleteToolStripButton.Click += new System.EventHandler(this.DeleteToolStripButton_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOK.AutoSize = true;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonOK.Location = new System.Drawing.Point(0, 0);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            // 
            // ruleGroupBox
            // 
            this.ruleGroupBox.Controls.Add(this.reevaluationComboBox);
            this.ruleGroupBox.Controls.Add(this.elseTextBox);
            this.ruleGroupBox.Controls.Add(this.elseLabel);
            this.ruleGroupBox.Controls.Add(this.thenTextBox);
            this.ruleGroupBox.Controls.Add(this.thenLabel);
            this.ruleGroupBox.Controls.Add(this.conditionTextBox);
            this.ruleGroupBox.Controls.Add(this.conditionLabel);
            this.ruleGroupBox.Controls.Add(this.nameTextBox);
            this.ruleGroupBox.Controls.Add(this.nameLabel);
            this.ruleGroupBox.Controls.Add(this.activeCheckBox);
            this.ruleGroupBox.Controls.Add(this.reevaluationLabel);
            this.ruleGroupBox.Controls.Add(this.priorityTextBox);
            this.ruleGroupBox.Controls.Add(this.priorityLabel);
            this.ruleGroupBox.Location = new System.Drawing.Point(13, 241);
            this.ruleGroupBox.Name = "ruleGroupBox";
            this.ruleGroupBox.Size = new System.Drawing.Size(669, 276);
            this.ruleGroupBox.TabIndex = 2;
            this.ruleGroupBox.TabStop = false;
            this.ruleGroupBox.Text = "Rule Definition";
            // 
            // reevaluationComboBox
            // 
            this.reevaluationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reevaluationComboBox.FormattingEnabled = true;
            this.reevaluationComboBox.Location = new System.Drawing.Point(294, 34);
            this.reevaluationComboBox.Name = "reevaluationComboBox";
            this.reevaluationComboBox.Size = new System.Drawing.Size(134, 21);
            this.reevaluationComboBox.TabIndex = 5;
            this.reevaluationComboBox.SelectedIndexChanged += new System.EventHandler(this.ReevaluationComboBox_SelectedIndexChanged);
            // 
            // elseTextBox
            // 
            this.elseTextBox.AcceptsReturn = true;
            this.elseTextBox.Location = new System.Drawing.Point(10, 218);
            this.elseTextBox.Multiline = true;
            this.elseTextBox.Name = "elseTextBox";
            this.elseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.elseTextBox.Size = new System.Drawing.Size(638, 47);
            this.elseTextBox.TabIndex = 12;
            this.elseTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ElseTextBox_Validating);
            // 
            // elseLabel
            // 
            this.elseLabel.AutoSize = true;
            this.elseLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.elseLabel.Location = new System.Drawing.Point(10, 202);
            this.elseLabel.Name = "elseLabel";
            this.elseLabel.Size = new System.Drawing.Size(68, 13);
            this.elseLabel.TabIndex = 11;
            this.elseLabel.Text = "&Else Actions:";
            // 
            // thenTextBox
            // 
            this.thenTextBox.AcceptsReturn = true;
            this.thenTextBox.Location = new System.Drawing.Point(10, 150);
            this.thenTextBox.Multiline = true;
            this.thenTextBox.Name = "thenTextBox";
            this.thenTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.thenTextBox.Size = new System.Drawing.Size(638, 45);
            this.thenTextBox.TabIndex = 10;
            this.thenTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ThenTextBox_Validating);
            // 
            // thenLabel
            // 
            this.thenLabel.AutoSize = true;
            this.thenLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.thenLabel.Location = new System.Drawing.Point(10, 134);
            this.thenLabel.Name = "thenLabel";
            this.thenLabel.Size = new System.Drawing.Size(245, 13);
            this.thenLabel.TabIndex = 9;
            this.thenLabel.Text = "&Then Actions (Examples: this.Prop2 = \"Yes\", Halt):";
            // 
            // conditionTextBox
            // 
            this.conditionTextBox.AcceptsReturn = true;
            this.conditionTextBox.Location = new System.Drawing.Point(10, 83);
            this.conditionTextBox.Multiline = true;
            this.conditionTextBox.Name = "conditionTextBox";
            this.conditionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.conditionTextBox.Size = new System.Drawing.Size(638, 45);
            this.conditionTextBox.TabIndex = 8;
            this.conditionTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ConditionTextBox_Validating);
            // 
            // conditionLabel
            // 
            this.conditionLabel.AutoSize = true;
            this.conditionLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.conditionLabel.Location = new System.Drawing.Point(10, 67);
            this.conditionLabel.Name = "conditionLabel";
            this.conditionLabel.Size = new System.Drawing.Size(251, 13);
            this.conditionLabel.TabIndex = 7;
            this.conditionLabel.Text = "&Condition (Example: this.Prop1>5 &&&& this.Prop1<10):";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(10, 34);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(155, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.NameTextBox_Validating);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.nameLabel.Location = new System.Drawing.Point(10, 17);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "&Name:";
            // 
            // activeCheckBox
            // 
            this.activeCheckBox.AutoSize = true;
            this.activeCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.activeCheckBox.Location = new System.Drawing.Point(444, 37);
            this.activeCheckBox.Name = "activeCheckBox";
            this.activeCheckBox.Size = new System.Drawing.Size(56, 17);
            this.activeCheckBox.TabIndex = 6;
            this.activeCheckBox.Text = "Acti&ve";
            this.activeCheckBox.CheckedChanged += new System.EventHandler(this.ActiveCheckBox_CheckedChanged);
            // 
            // reevaluationLabel
            // 
            this.reevaluationLabel.AutoSize = true;
            this.reevaluationLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.reevaluationLabel.Location = new System.Drawing.Point(294, 18);
            this.reevaluationLabel.Name = "reevaluationLabel";
            this.reevaluationLabel.Size = new System.Drawing.Size(73, 13);
            this.reevaluationLabel.TabIndex = 4;
            this.reevaluationLabel.Text = "&Reevaluation:";
            // 
            // priorityTextBox
            // 
            this.priorityTextBox.Location = new System.Drawing.Point(180, 34);
            this.priorityTextBox.MaxLength = 11;
            this.priorityTextBox.Name = "priorityTextBox";
            this.priorityTextBox.Size = new System.Drawing.Size(96, 20);
            this.priorityTextBox.TabIndex = 3;
            this.priorityTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.PriorityTextBox_Validating);
            // 
            // priorityLabel
            // 
            this.priorityLabel.AutoSize = true;
            this.priorityLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.priorityLabel.Location = new System.Drawing.Point(179, 17);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(41, 13);
            this.priorityLabel.TabIndex = 2;
            this.priorityLabel.Text = "&Priority:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point(83, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // headerTextLabel
            // 
            this.headerTextLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.headerTextLabel.Location = new System.Drawing.Point(57, 10);
            this.headerTextLabel.Name = "headerTextLabel";
            this.headerTextLabel.Size = new System.Drawing.Size(518, 36);
            this.headerTextLabel.TabIndex = 0;
            this.headerTextLabel.Text = "Configure the rule set. Add and Remove rules from the list. For each rule, set co" +
    "ndition and actions. ";
            // 
            // pictureBoxHeader
            // 
            this.pictureBoxHeader.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxHeader.Image")));
            this.pictureBoxHeader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBoxHeader.Location = new System.Drawing.Point(15, 10);
            this.pictureBoxHeader.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.pictureBoxHeader.Name = "pictureBoxHeader";
            this.pictureBoxHeader.Size = new System.Drawing.Size(35, 32);
            this.pictureBoxHeader.TabIndex = 21;
            this.pictureBoxHeader.TabStop = false;
            // 
            // okCancelTableLayoutPanel
            // 
            this.okCancelTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okCancelTableLayoutPanel.AutoSize = true;
            this.okCancelTableLayoutPanel.CausesValidation = false;
            this.okCancelTableLayoutPanel.ColumnCount = 2;
            this.okCancelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.okCancelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.okCancelTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.okCancelTableLayoutPanel.Controls.Add(this.buttonOK, 0, 0);
            this.okCancelTableLayoutPanel.Controls.Add(this.buttonCancel, 1, 0);
            this.okCancelTableLayoutPanel.Location = new System.Drawing.Point(520, 523);
            this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
            this.okCancelTableLayoutPanel.RowCount = 1;
            this.okCancelTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.okCancelTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.okCancelTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.okCancelTableLayoutPanel.Size = new System.Drawing.Size(159, 23);
            this.okCancelTableLayoutPanel.TabIndex = 22;
            // 
            // conditionErrorProvider
            // 
            this.conditionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.conditionErrorProvider.ContainerControl = this;
            // 
            // thenErrorProvider
            // 
            this.thenErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.thenErrorProvider.ContainerControl = this;
            // 
            // elseErrorProvider
            // 
            this.elseErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.elseErrorProvider.ContainerControl = this;
            // 
            // RuleSetDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(694, 555);
            this.Controls.Add(this.ruleGroupBox);
            this.Controls.Add(this.headerTextLabel);
            this.Controls.Add(this.pictureBoxHeader);
            this.Controls.Add(this.okCancelTableLayoutPanel);
            this.Controls.Add(this.rulesGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RuleSetDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rule Set Editor";
            this.rulesGroupBox.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.rulesToolStrip.ResumeLayout(false);
            this.rulesToolStrip.PerformLayout();
            this.ruleGroupBox.ResumeLayout(false);
            this.ruleGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).EndInit();
            this.okCancelTableLayoutPanel.ResumeLayout(false);
            this.okCancelTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.conditionErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thenErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.elseErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void buttonCancel_Click(object sender, EventArgs e)
        {
            this.conditionTextBox.Validating -= this.ConditionTextBox_Validating;
            this.thenTextBox.Validating -= this.ThenTextBox_Validating;
            this.elseTextBox.Validating -= this.ElseTextBox_Validating;
        }

        #endregion

        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ListView rulesListView;
        private System.Windows.Forms.ColumnHeader priorityColumnHeader;
        private System.Windows.Forms.ColumnHeader reevaluationCountColumnHeader;
        private System.Windows.Forms.ColumnHeader activeColumnHeader;
        private System.Windows.Forms.ColumnHeader rulePreviewColumnHeader;
        private System.Windows.Forms.GroupBox rulesGroupBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip rulesToolStrip;
        private System.Windows.Forms.ToolStripButton newRuleToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton deleteToolStripButton;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox ruleGroupBox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label headerTextLabel;
        private System.Windows.Forms.PictureBox pictureBoxHeader;
        private System.Windows.Forms.TableLayoutPanel okCancelTableLayoutPanel;
        private System.Windows.Forms.CheckBox activeCheckBox;
        private System.Windows.Forms.Label reevaluationLabel;
        private System.Windows.Forms.TextBox priorityTextBox;
        private System.Windows.Forms.Label priorityLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.ErrorProvider conditionErrorProvider;
        private System.Windows.Forms.ErrorProvider thenErrorProvider;
        private System.Windows.Forms.ErrorProvider elseErrorProvider;
        private IntellisenseTextBox elseTextBox;
        private System.Windows.Forms.Label elseLabel;
        private IntellisenseTextBox thenTextBox;
        private System.Windows.Forms.Label thenLabel;
        private IntellisenseTextBox conditionTextBox;
        private System.Windows.Forms.Label conditionLabel;
        private System.Windows.Forms.ComboBox reevaluationComboBox;
        private System.Windows.Forms.ComboBox chainingBehaviourComboBox;
        private System.Windows.Forms.Label chainingLabel;
    }
}
