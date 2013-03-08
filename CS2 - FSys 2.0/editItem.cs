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
    public partial class editItem : Form
    {
        public editItem()
        {
            InitializeComponent();
        }

        public FsysLogItem tobeEdited;
        public ComboBox.ObjectCollection allowedTypes;
        public FSysSettings tempSettings;
        public int HighestReceiptNumber;
        private bool cbReceiptOriginalState;
        private string tb4OriginalState;

        public DateTime newItemDate;
        public float newItemAmount;
        public string newItemDescription;
        public string newItemType;
        public int newItemReceiptID;
        public bool newItemIsIncome;
        public string newItmeComment;

        private void editItem_Load(object sender, EventArgs e)
        {
            label2.Text = tobeEdited.lvGetItemNumber();
            dateTimePicker1.Value = tobeEdited.itemDate;
            textBox1.Text = tobeEdited.lvGetDescription();
            label5.Text = "Amount " + tempSettings.currency;
            textBox2.Text = tobeEdited.lvGetAmount();
            foreach (Object objItemType in allowedTypes)
            {
                comboBox1.Items.Add(objItemType);
            }
            comboBox1.Text = tobeEdited.lvGetType();
            rbIncome.Checked = tobeEdited.isIncome;
            if (rbIncome.Checked == false)
            {
                rbExpense.Checked = true;
            }

            newItemReceiptID = tobeEdited.receiptNumber;
            if (tobeEdited.receiptNumber == -1)
            {
                cbReceiptOriginalState = false;
                cbReceipt.Checked = false;
                textBox4.Text = tobeEdited.lvGetReceiptID();
                tb4OriginalState = tobeEdited.lvGetReceiptID();
                textBox4.Enabled = false;
            }
            else
            {
                cbReceiptOriginalState = true;
                cbReceipt.Checked = true;
                textBox4.Text = tobeEdited.lvGetReceiptID();
                textBox4.Enabled = true;
            }

            textBox3.Text = tobeEdited.comments;

        }

        private void cbReceipt_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = cbReceipt.Checked;
            if (cbReceipt.Checked == true)
            {
                if (cbReceiptOriginalState == true)
                {

                    textBox4.Text = tobeEdited.receiptNumber.ToString("RN000000");

                }
                else
                {
                    int todisp = HighestReceiptNumber + 1;
                    textBox4.Text = todisp.ToString("RN000000");
                }
            }
            else
            {
                {
                    textBox4.Text = "No Receipt";
                    newItemReceiptID = -1;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newItemDate = dateTimePicker1.Value;
            newItemDescription = textBox1.Text;
            newItemAmount = float.Parse(textBox2.Text);
            newItemIsIncome = rbIncome.Checked;
            newItemType = comboBox1.Text;
            newItmeComment = textBox3.Text;
            if (cbReceipt.Checked)
            {
                newItemReceiptID = int.Parse(textBox4.Text.Remove(0, 2));
            }
            else
            {
                newItemReceiptID = -1;
            }

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                float.Parse(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Please Insert Valid Amount.");
                textBox2.Focus();
            }
        }
        
    }
}
