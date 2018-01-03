using System.Drawing;
using System.Windows.Forms;

namespace LogicBuilder.RuleSetToolkit
{
    internal partial class TextViewer : Form
    {
        internal TextViewer()
        {
            InitializeComponent();
            Initialize();
        }

        internal TextViewer(string viewText)
        {
            InitializeComponent();
            textBox1.Text = viewText;
            Initialize();
        }

        internal TextViewer(string[] lines)
        {
            InitializeComponent();
            textBox1.Lines = lines;
            Initialize();
        }

        #region Variables
        #endregion Variables

        #region methods
        private void Initialize()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(800, 600);
            textBox1.WordWrap = false;
            textBox1.ReadOnly = true;
            textBox1.BackColor = SystemColors.Window;
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Select(0, 0);
        }
        #endregion methods

        #region Properties
        internal string ViewText
        {
            set { textBox1.Text = value; }
        }
        #endregion Properties

        #region events
        #endregion events
    }
}