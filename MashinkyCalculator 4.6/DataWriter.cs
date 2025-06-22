using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace MashinkyCalculator
{
    public class DataWriter
    {
        private string SettingsLocation = Settings.Path + @"\settings.xml";
        public DataWriter()
        {

        }

        public XDocument CreateSettingsFile(string gameFolderPath)
        {
            Directory.CreateDirectory(Settings.Path);
            XDocument settings = new XDocument(
                      new XDeclaration("1.0", "UTF-8", null),
                      new XElement("Settings",
                      new XElement("GameFolder", gameFolderPath),
                      new XElement("Language", "english"),
                      new XElement("ResultPriority", "combined"),
                      new XElement("SpeedTolerance", "10"),
                      new XElement("Miles", "false"),
                      new XElement("Music", "True")
                      ));
            settings.Save(SettingsLocation);
          //  File.WriteAllText("trace.txt", "\nCreated settings");
            return settings;
        }

        public void WriteSettings()
        {
            XDocument doc = XDocument.Load(SettingsLocation);
            doc.Element("Settings").Element("Language").Value = Settings.Language;
            doc.Element("Settings").Element("ResultPriority").Value = Settings.ResultPriority;
            doc.Element("Settings").Element("SpeedTolerance").Value = Settings.SpeedTolerance.ToString();
            doc.Element("Settings").Element("Music").Value = Settings.Music.ToString();
        }
    }
}
