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
    public partial class PasswordCheck : Form
    {
        public PasswordCheck(FsysLog currentLog)
        {
            InitializeComponent();
            current = currentLog;
        }

        private void PasswordCheck_Load(object sender, EventArgs e)
        {
            this.label2.Text = current.getName();
            this.label4.Text = current.getUser();
        }

        FsysLog current;
        public bool valid;

        private void button1_Click(object sender, EventArgs e)
        {
            valid = current.isPasswordValid(this.textBox1.Text);
        }

        private void PasswordCheck_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void PasswordCheck_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
