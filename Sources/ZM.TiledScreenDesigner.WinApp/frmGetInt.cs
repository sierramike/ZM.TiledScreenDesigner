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
    public partial class frmGetInt : Form
    {
        public frmGetInt()
        {
            Min = int.MinValue;
            Max = int.MaxValue;

            InitializeComponent();
        }

        public virtual int Min { get; set; }

        public virtual int Max { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (int.TryParse(textBox1.Text, out i))
            {
                if (i >= Min && i <= Max)
                {
                    DialogResult = DialogResult.OK;
                    this.Hide();
                }
                else
                    MessageBox.Show(string.Format("Value is outside boundaries. Please enter a value between {0} and {1}.", Min, Max), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                MessageBox.Show("Invalid value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        public static int? GetInt(int source, string message, int min, int max)
        {
            int? result = null;

            var f = new frmGetInt();
            f.label1.Text = message;
            f.textBox1.Text = source.ToString();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                result = int.Parse(f.textBox1.Text);

            f.Close();

            return result;
        }
    }
}
