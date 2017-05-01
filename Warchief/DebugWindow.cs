using System;
using System.Windows.Forms;

namespace Warchief
{
    public partial class DebugWindow : Form
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        string ancien; // contient l'état précédent des variables 

        internal void SetLabel(string S)
        {
            if (ancien != S)
            {
                debugTB.Text = S + "\r\n \r\n " + debugTB.Text;
                ancien = S;
            }

        }
    }
}
