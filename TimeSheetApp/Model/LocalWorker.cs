using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
namespace TimeSheetApp.Model
{
    public static class LocalWorker
    {
        
        private static string localPath = @"UserData";
        private static List<Selection> _processSelection;
        public static List<Selection> ProcessSelection { get => _processSelection; set => _processSelection = value; }

        static LocalWorker()
        {
            _processSelection = Load();
        }
        private static void Save()
        {
            try
            {
                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Selection>));
                using (StreamWriter stream = new StreamWriter(localPath + $@"\{Environment.UserName}.xml"))
                {
                    serializer.Serialize(stream, ProcessSelection);
                }
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static Selection GetSelection(int ProcessID)
        {
            if (isContainsID(ProcessID))
                foreach (Selection selection in ProcessSelection)
                {
                    if (selection.ProcessID == ProcessID)
                        return selection;
                }
            return null;
        }
        private static List<Selection> Load()
        {
            if (File.Exists($"{localPath}\\{Environment.UserName}.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Selection>));
                using (XmlReader stream = new XmlTextReader(localPath + $@"\{Environment.UserName}.xml"))
                {
                    if (serializer.CanDeserialize(stream))
                        return (List<Selection>)serializer.Deserialize(stream);
                }
            }
            return new List<Selection>();
        }
        private static bool isContainsID(int processID)
        {
            foreach (Selection selection in ProcessSelection)
            {
                if (selection.ProcessID == processID)
                    return true;
            }
            return false;
        }
        public static void StoreSelection(Selection selection)
        {
            if (!isContainsID(selection.ProcessID))
                ProcessSelection.Add(selection);
            else
                foreach (Selection selectionOld in ProcessSelection)
                {
                    if (selectionOld.ProcessID == selection.ProcessID)
                    {
                        selectionOld.SetValues(selection);
                        selectionOld.SelectedCount++;
                    }
                }
            Save();
        }
        public static int ChoosenCounter(int ProcessID)
        {
            if (isContainsID(ProcessID))
                foreach (Selection selection in ProcessSelection)
                {
                    if (selection.ProcessID == ProcessID)
                        return selection.SelectedCount;
                }
            return 0;
        }
    }
}
