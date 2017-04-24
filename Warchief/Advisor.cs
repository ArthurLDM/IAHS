using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warchief
{
    public partial class Advisor : Form
    {
        public Advisor()
        {
            InitializeComponent();
        }

        internal void SetLabel(string S)
        {
            label1.Text = S;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{UP}");
        }
    }
}
