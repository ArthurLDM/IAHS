using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Warchief
{
    public partial class Mode : Form
    {
        public Mode()
        {
            InitializeComponent();

            //var fileName = Path.Combine(Environment.GetFolderPath(
            //Environment.SpecialFolder.ApplicationData), "\\HearthstoneDeckTracker\\Plugins\\bot.png");

            //label1.Text = Environment.GetFolderPath(
            //    Environment.SpecialFolder.ApplicationData).ToString
            //    ();
            //string fileName = "C: \\Users\\Thur\\Desktop\\Projet Info\\Warchief\\Projet - Info\\Warchief\\Img\\bot.png";
            //BotPB.Image =
        }
    
        public bool BotRB_Checked()
        {
            return (BotRB.Checked);
        }
    }
}
