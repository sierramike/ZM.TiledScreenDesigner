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
    public partial class frmGetContentAlignment : Form
    {
        public frmGetContentAlignment()
        {
            InitializeComponent();

            lstOptions.Items.Add(ContentAlignment.TopLeft);
            lstOptions.Items.Add(ContentAlignment.TopCenter);
            lstOptions.Items.Add(ContentAlignment.TopRight);
            lstOptions.Items.Add(ContentAlignment.MiddleLeft);
            lstOptions.Items.Add(ContentAlignment.MiddleCenter);
            lstOptions.Items.Add(ContentAlignment.MiddleRight);
            lstOptions.Items.Add(ContentAlignment.BottomLeft);
            lstOptions.Items.Add(ContentAlignment.BottomCenter);
            lstOptions.Items.Add(ContentAlignment.BottomRight);
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

        public static ContentAlignment? GetContentAlignment(ContentAlignment source)
        {
            ContentAlignment? result = null;

            var f = new frmGetContentAlignment();
            f.lstOptions.SelectedItem = source;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                result = (ContentAlignment)f.lstOptions.SelectedItem;

            f.Close();

            return result;
        }
    }
}
