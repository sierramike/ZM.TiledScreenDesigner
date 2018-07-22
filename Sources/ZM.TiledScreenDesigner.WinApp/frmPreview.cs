using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZM.TiledScreenDesigner.WinApp
{
    public partial class frmPreview : Form
    {
        public frmPreview(Image i)
        {
            InitializeComponent();

            pnlImage.BackgroundImage = i;
            pnlImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        }

        private void frmPreview_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlImage_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPreview_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
