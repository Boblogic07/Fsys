using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2___FSys_2._0
{
    // Class definition for Log Item which will be saved in the Log
    [Serializable]
    public class Receipt
    {
        public Receipt()
        {
        }
    }

    [Serializable]
    public class FsysLogItem
    {
        public FsysLogItem() //creates an invalid item
        {
            this.itemNumber = -1;
        }
        public FsysLogItem(
            int inputItemNumber,
            DateTime inputDate, 
            float inputAmount, 
            string inputDescription,
            string inputType,
            int inputReceiptID)
        {
            this.itemNumber = inputItemNumber;
            this.itemDate = inputDate;
            this.amount = inputAmount;
            this.description = inputDescription;
            this.type = inputType;
            this.receiptNumber = inputReceiptID;
            this.comments = "No Comment";
            this.isIncome = false;
        }
        public FsysLogItem(
            int inputItemNumber,
            DateTime inputDate,
            float inputAmount,
            string inputDescription,
            string inputType,
            int inputReceiptID,
            bool income)
        {
            this.itemNumber = inputItemNumber;
            this.itemDate = inputDate;
            this.amount = inputAmount;
            this.description = inputDescription;
            this.type = inputType;
            this.receiptNumber = inputReceiptID;
            this.comments = "No Comment";
            this.isIncome = income;
        }
        public FsysLogItem(
            int inputItemNumber,
            DateTime inputDate,
            float inputAmount,
            string inputDescription,
            string inputType,
            int inputReceiptID,
            string inputComment,
            bool income)
        {
            this.itemNumber = inputItemNumber;
            this.itemDate = inputDate;
            this.amount = inputAmount;
            this.description = inputDescription;
            this.type = inputType;
            this.receiptNumber = inputReceiptID;
            this.comments = inputComment;
            this.isIncome = income;
        }

        public string lvGetDate()
        {
            return itemDate.ToString("dd/MM/yyyy");
        }

        public string lvGetAmount()
        {
            return amount.ToString("0.00"); 
        }

        public string lvGetDescription()
        {
            return description;
        }

        public string lvGetType()
        {
            return type;
        }

        public string lvGetReceiptID()
        {
            if (receiptNumber == -1)
            {
                return "No Receipt";
            }
            else
            {
                string receiptNubmerStr = receiptNumber.ToString("RN000000");
                return receiptNubmerStr;
            }
        }

        public string lvGetItemNumber()
        {
            return itemNumber.ToString("00000000");
        }

        public int getItemNumber()
        {
            return itemNumber;
        }

        public int getReceiptNumber()
        {
            return receiptNumber;
        }

        public bool getIsincome()
        {
            return isIncome;
        }

        public void setIsincome(bool income)
        {
            isIncome = income;
        }

        public string lvGetIncome()
        {
            if (this.isIncome)
            {
                return amount.ToString("0.00");
            }
            else
            {
                return "";
            }
        }

        public string lvGetExpense()
        {
            if (this.isIncome)
            {
                return "";

            }
            else
            {
                return amount.ToString("0.00");
            }
        }

        public DateTime itemDate;
        public float amount;
        public string description;
        public string type;
        public int receiptNumber;
        public string comments;
        public int itemNumber;
        public bool isIncome;

        //Futureproofing
        public Dictionary<string, string> otherItems;

    }
    // Class definition file for F Sys Log Object
    [Serializable]
    public class FsysLog
    {
        //constructor
        public FsysLog(string name, string user)
        {
            creatDate = DateTime.Now;
            lastModDate = DateTime.Now;
            logName = name;
            logUser = user;
            logPassword = "";
            logContent = new List<FsysLogItem>();
            otherLogProperty = new Dictionary<string, string>();
            itemTypes = new List<string>();

        }
        public FsysLog(string name, string user, string password)
        {
            this.logPassword = password;
            creatDate = DateTime.Now;
            lastModDate = DateTime.Now;
            logName = name;
            logUser = user;
            logContent = new List<FsysLogItem>();
            otherLogProperty = new Dictionary<string, string>();
            itemTypes = new List<string>();
        }

        public bool passwordExist()
        {
            if (logPassword == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool isPasswordValid(string password)
        {
            if (password == logPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void addItem(FsysLogItem toAdd)
        {
            this.logContent.Add(toAdd);
        }

        public void addItem(
            DateTime inputDate,
            float inputAmount,
            string inputDescription,
            string inputType,
            bool Receipt,
            bool income)
        {
            bool typeExist = false;
            foreach (string iType in itemTypes)
            {
                if (iType == inputType)
                {
                    typeExist = true;
                    break;
                }

            }
            if (typeExist == false)
            {
                itemTypes.Add(inputType);
            }

            //gets highest ID number
            int highestID = 0;
            int highestReceiptID = 0;

            for (int i = 0; i < this.logContent.Count; i++)
            {
                FsysLogItem thisItem = logContent[i];
                if (thisItem.getItemNumber() > highestID)
                {
                    highestID = thisItem.getItemNumber();
                }
                if (thisItem.getReceiptNumber() > highestReceiptID)
                {
                    highestReceiptID = thisItem.getReceiptNumber();
                }
            }
            int newItemNumber = highestID + 1;
            int inputReceiptID = -1;
            if (Receipt)
            {
                inputReceiptID = highestReceiptID + 1;
            }

            FsysLogItem toAdd = new FsysLogItem(newItemNumber, inputDate, inputAmount, inputDescription, inputType, inputReceiptID, income);
            this.logContent.Add(toAdd);

        }

        public FsysLogItem getItem(int index)
        {
            return this.logContent[index];
        }

        public string getCreationDate()
        {
            return this.creatDate.ToString("dd/MM/yyyy");
        }

        public string getLastModDate()
        {
            return this.lastModDate.ToString("dd/MM/yyyy");
        }

        public string getName()
        {
            return logName;
        }

        public string getUser()
        {
            return logUser;
        }

        public FsysLogItem getItemByID(int ID)
        {
            FsysLogItem item = new FsysLogItem();

            foreach (FsysLogItem current in logContent)
            {
                if (current.getItemNumber() == ID)
                {
                    item = current;
                    break;
                }

            }
            return item;

            
        }

        public void deleteItem(int inputNumber)
        {
            List<FsysLogItem> toRemove = new List<FsysLogItem>();
            foreach (FsysLogItem current in logContent)
            {
                if (current.getItemNumber() == inputNumber)
                {
                    toRemove.Add(current);
                }
            }

            foreach (FsysLogItem current in toRemove)
            {
                logContent.Remove(current);
            }

            
        }

        public void setPassword(string password)
        {
            this.logPassword = password;
        }

        public void updateItem(int eID, DateTime eDate, float eAmount, string eDescription, string eType, bool eIsIncome, int eReceiptNumber, string eComments)
        {
            // find the Item
            for (int i=0; i<logContent.Count; i++)
            {
                if (logContent[i].itemNumber == eID)
                {
                    logContent[i].itemDate = eDate;
                    logContent[i].amount = eAmount;
                    logContent[i].description = eDescription;
                    logContent[i].type = eType;
                    logContent[i].setIsincome(eIsIncome);
                    logContent[i].receiptNumber = eReceiptNumber;
                    logContent[i].comments = eComments;
                    break;
                }
            }

        }

        public int getHighestReceiptNumber()
        {
            int highestReceiptID = 0;

            for (int i = 0; i < this.logContent.Count; i++)
            {
                FsysLogItem thisItem = logContent[i];
                if (thisItem.getReceiptNumber() > highestReceiptID)
                {
                    highestReceiptID = thisItem.getReceiptNumber();
                }
            }
            return highestReceiptID;
        }
        public void setLastModDate()
        {
            lastModDate = DateTime.Now;
        }

        string logName;
        DateTime creatDate;
        DateTime lastModDate;
        string logUser;
        string logPassword;
        public List<string> itemTypes;

        public List<FsysLogItem> logContent;
        public List<string> itemTypeAllowed;

        // Future proofing
        Dictionary<string, string> otherLogProperty;

        
    }
}
