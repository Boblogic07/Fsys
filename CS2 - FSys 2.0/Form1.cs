using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CS2___FSys_2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = ".\\Logs"; 
            this.openFileDialog1.FileName = currentLog.getName();
            this.openFileDialog1.ShowDialog();       
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.currentSetting.logPath = openFileDialog1.FileName;
            saveSettings();
            initialiseLog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Defining Inital Variables
            // Loads most recent setting and default log
            initialiseSettings();
            initialiseLog();
            initialiseUndoStack();
            updateTypeSelect();
            
            //Displays Log
            updateLogDisplay();
            updateListView();

        }


        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.currentSetting.logPath = this.saveFileDialog1.FileName;
            saveSettings();
            saveLog();
        }

        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = ".\\Logs";
            saveFileDialog1.FileName = currentLog.getName();
            saveFileDialog1.ShowDialog();
        }

        private void newLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewLog newWindow = new NewLog();
            newWindow.ShowDialog();
            if (newWindow.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (newWindow.password == "")
                {
                    this.currentLog = new FsysLog(newWindow.logName, newWindow.userName);
                }
                else
                {
                    this.currentLog = new FsysLog(newWindow.logName, newWindow.userName, newWindow.password);
                }
                saveFileDialog1.InitialDirectory = ".\\Logs";
                saveFileDialog1.FileName = currentLog.getName();
                currentSetting.logPath = ".\\Logs\\newLog.fsl";
                saveFileDialog1.ShowDialog();

            }

            updateLogDisplay();
            updateListView();

            


        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            currentSetting.logPath = openFileDialog2.FileName;
            saveSettings();
        }

        private void setDefaultLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.FileName = currentSetting.logPath;
            openFileDialog2.ShowDialog();
        }

        private void saveLogToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveLog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            backupToUndo();
            bool inputValid = true;

            DateTime itemDate = this.dtpQuickAddDate.Value;
            string rawAmount = this.tbQuickAddAmount.Text.Trim();
            string strAmount;
            int currencyLocation = rawAmount.IndexOf(currentSetting.currencySymbol);
            if (currencyLocation != -1)
            {
                strAmount = rawAmount.Remove(currencyLocation, 1);
            }
            else
            {
                strAmount = rawAmount;
            }
            float amount;
            float.TryParse(strAmount, out amount);
            if (amount == 0)
            {
                MessageBox.Show("Please input valid amount.");
                inputValid = false;
            }
            string description = this.tbQuickAddDescription.Text.Trim();
            if (description == "")
            {
                MessageBox.Show("Please input valid description.");
                inputValid = false;
            }
            string type = this.cbQuickAddType.Text.Trim();

            if (inputValid == true)
            {
                currentLog.addItem(itemDate, amount, description, type, cbQuickAddReceipt.Checked, this.radioButton1.Checked);
                saveLog();
                this.tbQuickAddDescription.Text = "";
                this.tbQuickAddAmount.Text = "";
                this.cbQuickAddReceipt.Checked = false;
            }



            updateListView();

        }

        private void resetSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetSettings();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteSelectedItems();
        }


        // USER DEFINED FUNCITONS for basic functionality
        private void updateLogDisplay()
        {
            this.label4.Text = currentLog.getName();
            this.label5.Text = currentLog.getCreationDate();
            this.label6.Text = currentLog.getLastModDate();
            this.label8.Text = currentLog.getUser();
            updateTypeSelect();
        }
        private void saveSettings()
        {
            SettingSerialiser sS = new SettingSerialiser();
            sS.saveSettings(this.currentSetting);
        }
        private void resetSettings()
        {
            currentSetting = new defaultSettings();
            saveSettings();
        }
        private void initialiseSettings()
        {
            SettingSerialiser sS = new SettingSerialiser();
            try
            {
                currentSetting = sS.loadSettings(".\\Settings\\FSconfig.fsc");
            }
            catch (FileNotFoundException)
            {
                resetSettings();
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(".\\Settings");
                resetSettings();
            }

            this.label11.Text = "Amount(" + currentSetting.currency + "):";
        }
        private void initialiseLog()
        {
            LogSerialiser lS = new LogSerialiser();
            this.currentLog = lS.loadLog(currentSetting.logPath);
            logPasswordCheck();
            updateLogDisplay();
            updateListView();
            saveLog();
        }
        private void updateTypeSelect()
        {
            foreach (string item in currentLog.itemTypes)
            {
                
                if (!cbQuickAddType.Items.Contains(item))
                {
                    this.cbQuickAddType.Items.Add(item);
                }
            }

        }
        private void saveLog()
        {
            currentLog.setLastModDate();
            LogSerialiser logS = new LogSerialiser();
            logS.saveLog(currentLog, currentSetting.logPath);
        }
        private void pushLog()
        {
            LogSerialiser logS = new LogSerialiser();
            logS.saveLog(currentLog, ".\\Settings\\undostack.fsu");
            undoToolStripMenuItem.Enabled = true;
            undoValid = true;
        }
        private void undolog()
        {
            LogSerialiser logS = new LogSerialiser();
            logS.saveLog(currentLog, ".\\Settings\\redostack.fsu");
            LogSerialiser lS = new LogSerialiser();
            this.currentLog = lS.loadLog(".\\Settings\\undostack.fsu");
            saveLog();
            updateListView();
            redoToolStripMenuItem.Enabled = true;
            redoValid = true;
            undoToolStripMenuItem.Enabled = false;
            undoValid = false;
        }
        private void redolog()
        {
            pushLog();
            LogSerialiser lS = new LogSerialiser();
            this.currentLog = lS.loadLog(".\\Settings\\redostack.fsu");
            saveLog();
            updateListView();
            undoToolStripMenuItem.Enabled = true;
            undoValid = true;
            redoToolStripMenuItem.Enabled = false;
            redoValid = false;
        }
        private void updateListView()
        {
            listView1.Items.Clear();
            listView1.Columns[2].Width = listView1.Width - 600;

            foreach (FsysLogItem logItem in currentLog.logContent)
            {
                ListViewItem item = new ListViewItem(logItem.lvGetItemNumber());
                string strCurrency = currentSetting.currencySymbol.ToString();
                bool income = logItem.getIsincome();
                if (income)
                {
                    item.ForeColor = Color.Green;
                }
                else
                {
                    item.ForeColor = Color.Red;
                }
                string[] subItem = new string[6];
                subItem[0] = logItem.lvGetDate();
                subItem[1] = logItem.lvGetDescription();
                subItem[2] = logItem.lvGetType();
                subItem[3] = logItem.lvGetExpense();
                subItem[4] = logItem.lvGetIncome();
                subItem[5] = logItem.lvGetReceiptID();

                item.SubItems.AddRange(subItem);
                listView1.Items.Add(item);
            }

            updateTypeSelect();

        }
        private void deleteSelectedItems()
        {
            if (listView1.SelectedItems == null)
            {
                MessageBox.Show("Please select an item first then press delete");
            }
            else
            {
                backupToUndo();
                ListView.SelectedListViewItemCollection selected = listView1.SelectedItems;
                foreach (ListViewItem item in selected)
                {
                    string toParse = item.Text;
                    currentLog.deleteItem(int.Parse(toParse));
                }
            }
            updateListView();
            saveLog();
        }
        private void initialiseUndoStack()
        {
            undoValid = false;
            redoValid = false;
        }
        private void backupToUndo()
        {
            pushLog();
        }
        private void logPasswordCheck()
        {
            if (this.currentLog.passwordExist())
            {
                bool isPasswordValid = false;
                for (int i = 0; i < 3; i++)
                {
                    PasswordCheck challange = new PasswordCheck(currentLog);
                    if (challange.ShowDialog() == DialogResult.OK)
                    {
                        if (challange.valid)
                        {
                            isPasswordValid = true;
                            break;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Passoword, please try again.");
                        }
                    }
                }
                if (!isPasswordValid)
                {
                    MessageBox.Show("Invalid Password, too many attempts has been made.");
                    Application.Exit();
                }

            }

        }
        FsysLog currentLog;
        FSysSettings currentSetting;
        bool redoValid;
        bool undoValid;
        bool ctrlPressed;

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undolog();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redolog();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.Z)
                {
                    if (redoValid)
                    {
                        redolog();
                    }
                    else if (undoValid)
                    {
                        undolog();
                    }
                }
            }
        }

        

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                deleteSelectedItems();
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logPasswordCheck();
            fNewPassword nPass = new fNewPassword();
            if (nPass.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentLog.setPassword(nPass.newPassword);
                saveLog();
                MessageBox.Show("Password Changed!");
            }


        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            listView1.Columns[2].Width = (listView1.Width - 600);
        }

        private void editItemToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Get Types 
            if (listView1.SelectedItems.Count > 1)
            {
                MessageBox.Show("Error! Please select only 1 item to edit.");
            }
            else
            {
                ComboBox.ObjectCollection itemTypes = cbQuickAddType.Items;
                try
                {
                    int TargetItemNumber = int.Parse(listView1.SelectedItems[0].Text);
                    FsysLogItem toBeEditedItem = currentLog.getItemByID(TargetItemNumber);
                    if (toBeEditedItem.itemNumber == -1)
                    {
                        MessageBox.Show("Invalid Item!");
                        // invalid Item Found
                    }
                    else
                    {

                        editItem editor = new editItem();

                        editor.tobeEdited = toBeEditedItem;
                        editor.allowedTypes = itemTypes;
                        editor.tempSettings = currentSetting;
                        editor.HighestReceiptNumber = currentLog.getHighestReceiptNumber();
                        DialogResult eResult = editor.ShowDialog();
                        if (eResult == DialogResult.OK)
                        {
                            pushLog();
                            currentLog.updateItem(
                                toBeEditedItem.itemNumber, 
                                editor.newItemDate,
                                editor.newItemAmount, 
                                editor.newItemDescription, 
                                editor.newItemType, 
                                editor.newItemIsIncome, 
                                editor.newItemReceiptID, 
                                editor.newItmeComment);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error! Please select an item to edit");
                }
                saveLog();
                updateListView();
                updateLogDisplay();
            }
        }

        

    }
}
