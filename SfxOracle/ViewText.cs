using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SfxOracle
{
    public partial class ViewText : Form
    {
        public ViewText(string text)
        {
            InitializeComponent();
            this.richTextBox1.Text = text;
        }

        private void ViewText_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.richTextBox1.Text);
        }
    }
}
