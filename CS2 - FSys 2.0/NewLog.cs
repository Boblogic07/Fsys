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
    public partial class NewLog : Form
    {
        public NewLog()
        {
            InitializeComponent();
        }

        private void NewLog_Load(object sender, EventArgs e)
        {

        }

        public string logName;
        public string userName;
        public string password;

        private void button1_Click(object sender, EventArgs e)
        {
            this.logName = this.tbLogName.Text;
            this.userName = this.tbUser.Text;
            this.password = this.tbPassword.Text;
        }

        private void NewLog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
