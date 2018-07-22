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
    public partial class frmGetHorizontalAlign : Form
    {
        public frmGetHorizontalAlign()
        {
            InitializeComponent();

            lstOptions.Items.Add(System.Windows.Forms.VisualStyles.HorizontalAlign.Left);
            lstOptions.Items.Add(System.Windows.Forms.VisualStyles.HorizontalAlign.Center);
            lstOptions.Items.Add(System.Windows.Forms.VisualStyles.HorizontalAlign.Right);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        public static System.Windows.Forms.VisualStyles.HorizontalAlign? GetHorizontalAlign(System.Windows.Forms.VisualStyles.HorizontalAlign source)
        {
            System.Windows.Forms.VisualStyles.HorizontalAlign? result = null;

            var f = new frmGetHorizontalAlign();
            f.lstOptions.SelectedItem = source;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                result = (System.Windows.Forms.VisualStyles.HorizontalAlign)f.lstOptions.SelectedItem;

            f.Close();

            return result;
        }
    }
}
