using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CS2___FSys_2._0
{
    [Serializable]
    public class FSysSettings
    {
        public string logPath;
        public string currency;
        public float savingGoal;
        public float expectedIncome;
        public string currencySymbol;

        // future proofing
        public Dictionary<string,string> otherSettings;
    }

    [Serializable]
    public class defaultSettings : FSysSettings
    {
        public defaultSettings()
        {
            this.logPath = ".\\Logs\\defaultLog.fsl";
            this.currency = "AUD";
            this.currencySymbol = "$";
            this.savingGoal = 10000;
            this.expectedIncome = 60000;
        }

    }


    //Settings serialiser
    [Serializable]
    public class SettingSerialiser
    {
        public FSysSettings loadSettings(string settingPath)
        {
            serialiser = new BinaryFormatter();
            reader = File.Open(settingPath, FileMode.Open);
            FSysSettings result = (FSysSettings)serialiser.Deserialize(reader);
            reader.Close();
            saveSettings(result);
            return result;

        }

        public void saveSettings(FSysSettings toBeSaved)
        {
            serialiser = new BinaryFormatter();
            writer = File.OpenWrite(".\\Settings\\FSconfig.fsc");
            serialiser.Serialize(writer, toBeSaved);
            writer.Close();
        }

        public void saveSettingsAs(FSysSettings toBeSaved, string filePath)
        {
            serialiser = new BinaryFormatter();
            writer = File.OpenWrite(filePath);
            serialiser.Serialize(writer, toBeSaved);
            writer.Close();
        }

        BinaryFormatter serialiser;
        Stream writer;
        Stream reader;
    }

    //Log serialiser
    [Serializable]
    public class LogSerialiser
    {
        public FsysLog loadLog(string logFilePath)
        {
            FsysLog result;
            serializer = new BinaryFormatter();
            try
            {
                readStream = File.OpenRead(logFilePath);
                result = (FsysLog)serializer.Deserialize(readStream);
                readStream.Close();
            }
            catch (FileNotFoundException)
            {
                result = new FsysLog("Default Log", "Default User");
            }
            return result;
        }

        public void saveLog(FsysLog toBeSaved, string logFilePath)
        {
            serializer = new BinaryFormatter();
            writeStream = File.OpenWrite(logFilePath);
            serializer.Serialize(writeStream, toBeSaved);
            writeStream.Close();
        }

        BinaryFormatter serializer;
        Stream writeStream;
        Stream readStream;
        
    }
}
