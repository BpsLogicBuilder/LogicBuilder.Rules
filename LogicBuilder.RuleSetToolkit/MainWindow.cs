using LogicBuilder.Workflow.Activities.Rules;
using LogicBuilder.Workflow.Activities.Rules.Design;
using LogicBuilder.RuleSetToolkit.Exceptions;
using LogicBuilder.RuleSetToolkit.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LogicBuilder.RuleSetToolkit
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        #region Constants
        private const string FULLYQUALIFIEDCLASSNAME = @"^[A-Za-z_]{1}[A-Za-z0-9_\.]*[A-Za-z0-9_]{1}$";
        private const string FULLPATH = @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$";
        private const string reEvaluationPrefix = "Reevaluation";
        #endregion Constants

        #region Variables
        private List<string> validationErrors = new List<string>();
        private Type activityClassType;
        private RuleSet ruleSet;
        private List<Assembly> referenceAssemblies;
        #endregion Variables

        #region Properties
        private string RulesFileFullName
        {
            get { return this.txtRuleset.Text.Trim(); }
        }

        private string AssemblyFullName
        {
            get { return this.txtActivityAssembly.Text.Trim(); }
        }

        private string ActivityClass
        {
            get { return this.txtActivityClass.Text.Trim(); }
        }

        private bool CanSave => (this.activityClassType != null && this.ruleSet != null && this.platformComboBox.SelectedIndex != -1 && this.validationErrors.Count == 0 && Regex.IsMatch(this.RulesFileFullName, FULLPATH));
        #endregion Properties

        #region Methods
        private void Initialize()
        {
            groupBoxEditRuleSets.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtActivityClass.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtActivityAssembly.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtRuleset.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            panel3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPlatform.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            platformComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseActivityAssembly.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseRuleset.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.rulesGroupBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.panel2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.rulesListView.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            this.platformComboBox.Items.AddRange(new ComboBoxItem[]
            {
                new ComboBoxItem(Resources.NetCore, DotNetPlatForm.NetCore),
                new ComboBoxItem(Resources.NetFramework, DotNetPlatForm.NetFramework),
                new ComboBoxItem(Resources.Xamarin, DotNetPlatForm.Xamarin),
                new ComboBoxItem(Resources.NetNative, DotNetPlatForm.NetNative),
            });

            if (Enum.IsDefined(typeof(DotNetPlatForm), Settings.Default.selectedPlatForm))
            {
                DotNetPlatForm selectedPlatForm = (DotNetPlatForm)Enum.Parse(typeof(DotNetPlatForm), Settings.Default.selectedPlatForm);
                this.platformComboBox.SelectedItem = this.platformComboBox.Items
                    .OfType<ComboBoxItem>()
                    .SingleOrDefault(i => i.PlatForm == selectedPlatForm);
            }
            else
            {
                this.platformComboBox.SelectedIndex = 0;
            }

            this.groupBoxErrors.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            labelErrors.ForeColor = SystemColors.InfoText;
            labelErrors.BackColor = SystemColors.Info;
            flowLayoutPanel1.AutoScroll = true;

            txtActivityClass.Text = Settings.Default.activityClass;
            txtActivityAssembly.Text = Settings.Default.activityAssembly;
            txtRuleset.Text = Settings.Default.ruleSet;
            ValidateButtons();
        }

        private void ListRules()
        {
            ClearRules();
            if (this.ruleSet == null) return;
            foreach (Rule rule in this.ruleSet.Rules)
                this.AddNewItem(rule);
        }

        private ListViewItem AddNewItem(Rule rule)
        {
            ListViewItem listViewItem = new ListViewItem(new string[] { rule.Name, String.Empty, String.Empty, String.Empty, String.Empty });
            this.rulesListView.Items.Add(listViewItem);
            listViewItem.Tag = rule;
            UpdateItem(listViewItem, rule);

            return listViewItem;
        }

        private void UpdateItem(ListViewItem listViewItem, Rule rule)
        {
            listViewItem.SubItems[0].Text = rule.Name;
            listViewItem.SubItems[1].Text = rule.Priority.ToString(CultureInfo.CurrentCulture);
            listViewItem.SubItems[2].Text = Resources.ResourceManager.GetString(string.Concat(reEvaluationPrefix, Enum.GetName(typeof(RuleReevaluationBehavior), rule.ReevaluationBehavior)));
            listViewItem.SubItems[3].Text = rule.Active.ToString(CultureInfo.CurrentCulture);
            listViewItem.SubItems[4].Text = GetRulePreview(rule);
        }

        private string GetRulePreview(LogicBuilder.Workflow.Activities.Rules.Rule rule)
        {
            StringBuilder rulePreview = new StringBuilder();

            if (rule != null)
            {
                rulePreview.Append("IF ");
                if (rule.Condition != null)
                    rulePreview.Append(rule.Condition.ToString() + " ");
                rulePreview.Append("THEN ");

                foreach (RuleAction action in rule.ThenActions)
                {
                    rulePreview.Append(action.ToString());
                    rulePreview.Append(' ');
                }

                if (rule.ElseActions.Count > 0)
                {
                    rulePreview.Append("ELSE ");
                    foreach (RuleAction action in rule.ElseActions)
                    {
                        rulePreview.Append(action.ToString());
                        rulePreview.Append(' ');
                    }
                }
            }

            return rulePreview.ToString();
        }

        private void ValidateLoad()
        {
            ClearErrorLabel();
            if (!(Regex.IsMatch(this.ActivityClass, FULLYQUALIFIEDCLASSNAME)))
            {
                this.activityClassType = null;
                btnLoad.Enabled = false;
                return;
            }

            if (!(File.Exists(this.AssemblyFullName)))
            {
                UpdateErrorLabel(string.Format(CultureInfo.CurrentCulture, Resources.cannotLoadAssemblyFormat, this.AssemblyFullName));
                this.activityClassType = null;
                btnLoad.Enabled = false;
                return;
            }

            AssemblyLoader assemblyLoader = new AssemblyLoader(this.AssemblyFullName, Settings.Default.assemblyLoadPaths.Split(new char[] { ';' }), System.Runtime.Loader.AssemblyLoadContext.Default);
            Assembly assembly;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                assembly = assemblyLoader.LoadAssembly();
                if (assembly != null)
                {
                    this.activityClassType = AssemblyLoader.GetType(assembly, this.ActivityClass, true);
                    this.referenceAssemblies = assembly.GetReferencedAssembliesRecursively(assemblyLoader);
                }
            }
            catch (ToolkitException ex)
            {
                this.activityClassType = null;
                UpdateErrorLabel(ex.Message);
                btnLoad.Enabled = false;
                this.Cursor = Cursors.Default;
                return;
            }

            if (assembly == null || this.activityClassType == null)
            {
                this.activityClassType = null;
                UpdateErrorLabel(string.Format(CultureInfo.CurrentCulture, Resources.cannotLoadAssemblyFormat, this.AssemblyFullName));
                btnLoad.Enabled = false;
                this.Cursor = Cursors.Default;
                return;
            }

            this.Cursor = Cursors.Default;
            btnLoad.Enabled = true;
        }

        private void ValidateEdit()
        {
            btnEdit.Enabled = (this.activityClassType != null && File.Exists(this.RulesFileFullName));

        }

        private void ValidateSave()
        {
            btnSave.Enabled = CanSave;
        }

        private void ValidateNew()
        {
            btnNew.Enabled = this.activityClassType != null;
        }

        private void GetAssemblyFullPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = Resources.selectAssemblyDialogTitle,
                Filter = Resources.assembliesFilter
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtActivityAssembly.Text = openFileDialog.FileName;
                txtActivityAssembly.Focus();
                ValidateButtons();
            }
        }

        private void GetRulesetFullPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = Resources.selectRuleSetDialogTitle,
                Filter = Resources.ruleSetsFiler
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtRuleset.Text = openFileDialog.FileName;
                txtRuleset.Focus();
            }
        }

        private void GetNewRulesetFullPath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = Resources.newRuleSetDialogTitle,
                Filter = Resources.ruleSetsFiler
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtRuleset.Text = saveFileDialog.FileName;
                this.ruleSet = new RuleSet
                {
                    Name = Path.GetFileNameWithoutExtension(saveFileDialog.FileName),
                    ChainingBehavior = RuleChainingBehavior.Full
                };

                ValidateRules();
                ListRules();
                if (CanSave)
                {
                    SaveRules();
                    ValidateEdit();
                    ValidateSave();
                }
            }
        }

        private void EditRuleset()
        {
            try
            {
                if (LoadRules())
                {
                    DisplayRules();
                }
            }
            catch (ToolkitException ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
            ValidateSave();
            ValidateNew();
        }

        private bool ValidateRules()
        {
            validationErrors = Rules.ValidateRuleSet(this.activityClassType, ruleSet, this.referenceAssemblies);
            UpdateErrorLabel(validationErrors.Count != 0
                ? string.Join(Environment.NewLine, validationErrors)
                : string.Empty);

            return validationErrors.Count == 0;
        }

        private void ClearErrorLabel()
        {
            labelErrors.Text = string.Empty;
        }

        private void UpdateErrorLabel(string errors)
        {
            labelErrors.Text = errors;
        }

        private bool LoadRules()
        {
            ClearErrorLabel();
            try
            {
                ruleSet = Rules.LoadRuleSet(this.RulesFileFullName);
            }
            catch (ToolkitException ex)
            {
                UpdateErrorLabel(ex.Message);
                return false;
            }

            if (ruleSet == null)
            {
                UpdateErrorLabel(string.Format(CultureInfo.CurrentCulture, Resources.cannotLoadRuleSetFormat, this.RulesFileFullName));
                return false;
            }

            ValidateRules();
            ListRules();
            return true;
        }

        private void ClearRules()
        {
            this.rulesListView.Items.Clear();
        }

        private void DisplayRules()
        {
            RuleSetDialog ruleSetDialog = new RuleSetDialog(this.activityClassType, ruleSet, this.referenceAssemblies)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            ruleSetDialog.ShowDialog();
            if (ruleSetDialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    this.Refresh();
                    this.ruleSet = ruleSetDialog.RuleSet;
                    ValidateRules();
                    ListRules();
                    if (CanSave) SaveRules();

                    ValidateSave();
                    ValidateNew();
                }
                catch (ArgumentException ex)
                {
                    throw new ToolkitException(ex.Message, ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new ToolkitException(ex.Message, ex);
                }
                catch (IOException ex)
                {
                    throw new ToolkitException(ex.Message, ex);
                }
                catch (System.Security.SecurityException ex)
                {
                    throw new ToolkitException(ex.Message, ex);
                }
                catch (ObjectDisposedException ex)
                {
                    throw new ToolkitException(ex.Message, ex);
                }
                catch (NotSupportedException ex)
                {
                    throw new ToolkitException(ex.Message, ex);
                }
            }
        }

        private void ValidateButtons()
        {
            ValidateLoad();
            ValidateEdit();
            ValidateSave();
            ValidateNew();
        }
        #endregion Methods

        #region EventHandlers
        private void TxtActivityClass_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void TxtActivityAssembly_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void TxtRuleset_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void TxtActivityClass_Validated(object sender, EventArgs e)
        {
            ValidateButtons();
        }

        private void TxtActivityAssembly_Validated(object sender, EventArgs e)
        {
            ValidateButtons();
        }

        private void TxtRuleset_Validated(object sender, EventArgs e)
        {
            ValidateEdit();
        }

        private void BtnBrowseActivityAssembly_Click(object sender, EventArgs e)
        {
            GetAssemblyFullPath();
        }

        private void BtnBrowseRuleset_Click(object sender, EventArgs e)
        {
            GetRulesetFullPath();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditRuleset();
            ValidateSave();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            GetNewRulesetFullPath();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.activityClass = txtActivityClass.Text;
            Settings.Default.activityAssembly = txtActivityAssembly.Text;
            Settings.Default.ruleSet = txtRuleset.Text;

            if (this.platformComboBox.SelectedIndex > -1)
                Settings.Default.selectedPlatForm = Enum.GetName(typeof(DotNetPlatForm), ((ComboBoxItem)this.platformComboBox.SelectedItem).PlatForm);

            Settings.Default.Save();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadRules();
            ValidateSave();
            ValidateNew();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveRules();
            ValidateEdit();
            ValidateLoad();
            ValidateSave();
            ValidateNew();
        }

        private void SaveRules()
        {
            StreamWriter sr = new StreamWriter(this.RulesFileFullName, false, Encoding.Unicode);
            sr.Write(Rules.SerializeRuleSet(this.ruleSet, ((ComboBoxItem)this.platformComboBox.SelectedItem).PlatForm));
            sr.Close();
        }

        private void RulesListView_SizeChanged(object sender, EventArgs e)
        {
            int width = rulesListView.Size.Width;
            this.rulesListView.Columns[0].Width = width / 8;
            this.rulesListView.Columns[1].Width = width / 9;
            this.rulesListView.Columns[2].Width = width / 8;
            this.rulesListView.Columns[3].Width = width / 9;
            this.rulesListView.Columns[4].Width = width / 2;
        }
        #endregion EventHandlers
    }

    internal struct AssemblyStrongNames
    {
        internal const string NETCORE = "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        internal const string NETFRAMEWORK = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        internal const string XAMARIN = "mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        internal const string NETNATIVE = "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

        internal const string CODEDOM_NETCORE = "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";
        internal const string CODEDOM_NETFRAMEWORK = "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        internal const string CODEDOM_XAMARIN = "System.CodeDom, Version=4.0.0.0 Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";
        internal const string CODEDOM_NETNATIVE = "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";

        internal const string NETCORE_MATCH = @"System.Private.CoreLib, Version=\d.\d.\d.\d, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        internal const string NETFRAMEWORK_MATCH = @"mscorlib, Version=\d.\d.\d.\d, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        internal const string XAMARIN_MATCH = @"mscorlib, Version=\d.\d.\d.\d, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        internal const string NETNATIVE_MATCH = @"System.Private.CoreLib, Version=\d.\d.\d.\d, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

        internal const string CODEDOM_NETCORE_MATCH = @"System.CodeDom, Version=\d.\d.\d.\d, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";
        internal const string CODEDOM_NETFRAMEWORK_MATCH = @"System, Version=\d.\d.\d.\d, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        internal const string CODEDOM_XAMARIN_MATCH = @"System.CodeDom, Version=\d.\d.\d.\d, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";
        internal const string CODEDOM_NETNATIVE_MATCH = @"System.CodeDom, Version=\d.\d.\d.\d, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";
    }
}
