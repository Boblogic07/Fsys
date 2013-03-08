using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS2___FSys_2._0
{
    public partial class fNewPassword : Form
    {
        public fNewPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newPassword = this.textBox1.Text;
        }

        private void fNewPassword_KeyUp(object sender, KeyEventArgs e)
      {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
            }
        }

        public string newPassword;
    }
}
