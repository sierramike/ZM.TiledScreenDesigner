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
    public partial class frmGetRotation : Form
    {
        public frmGetRotation()
        {
            InitializeComponent();

            lstOptions.Items.Add(Rotation.None);
            lstOptions.Items.Add(Rotation.Clockwise90);
            lstOptions.Items.Add(Rotation.Clockwise180);
            lstOptions.Items.Add(Rotation.Clockwise270);
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

        public static Rotation? GetRotation(Rotation source)
        {
            Rotation? result = null;

            var f = new frmGetRotation();
            f.lstOptions.SelectedItem = source;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                result = (Rotation)f.lstOptions.SelectedItem;

            f.Close();

            return result;
        }
    }
}
