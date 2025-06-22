using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    // bad SRP compliance
    class XMLReader
    {
        // private HashConvertor hashConvertor;
        private DataManager dataManager;
        public BitmapImage BlankImage { get; private set; } // image for all wagons without icon

        public List<Wagon> Wagons { get; private set; }
        public List<Engine> Engines { get; private set; }
        public List<Token> Tokens { get; private set; }
        public List<Material> Materials { get; private set; }



        public XMLReader(ImageReader imageReader, DataManager dataManager)
        {
            this.dataManager = dataManager;
            // this.hashConvertor = hashConvertor;
            Engines = new List<Engine>();
            Wagons = new List<Wagon>();
            Tokens = new List<Token>();
            Materials = new List<Material>();
        }


        /// <summary>
        ///  reads wagon_types.xml
        /// </summary>
        /// <returns></returns>
        public void ReadWGT(string path)
        {
            string pathXML = path + @"\config\wagon_types.xml";
           // File.AppendAllText("trace.txt", "\nLoading " + pathXML);
            XDocument Data;

            Data = CorrectXmlFile(pathXML);
            if (String.IsNullOrEmpty(Data.ToString()))
                return;
            try
            {
                foreach (XElement w in Data.Element("root").Elements("WagonType"))
                {
                    bool validEntry = true;
                    int vehicleType = 0; // 0 - train, 1 - road vehicle
                    if (w.Attribute("vehicle_type") != null)
                    {
                        vehicleType = int.Parse(w.Attribute("vehicle_type").Value);
                        if (vehicleType == 1)
                            return;
                    }
                    else
                        validEntry = false;
                    string XMLcost = "9999";
                    if (w.Attribute("cost") != null)
                        XMLcost = w.Attribute("cost").Value;
                    string[] costs = XMLcost.Split(';');
                    int cost1 = SeparateCost(costs[0]);
                    IToken costToken1 = dataManager.AssignToken(SeparateType(costs[0]));
                    int cost2 = 0;
                    IToken costToken2 = null;
                    if (costs.Length > 1 && costs[1] != "") // multiple costs, treated against lonely ';'
                    {
                        cost2 = SeparateCost(costs[1]);
                        costToken2 = dataManager.AssignToken(SeparateType(costs[1]));
                    }

                    string iconTexture = "";   // check that XML contains attributes it should to prevent null exceptions
                    if (w.Attribute("icon_texture") != null)
                        iconTexture = w.Attribute("icon_texture").Value;
                    else
                        validEntry = false;
                    string ID = "";
                    if (w.Attribute("id") != null)
                        ID = w.Attribute("id").Value;
                    else
                        validEntry = false;
                    string icon = "";
                    if (w.Attribute("icon") != null)
                        icon = w.Attribute("icon").Value;
                    else
                        validEntry = false;
                    string iconColor = "";
                    if (w.Attribute("icon_color") != null)
                        iconColor = w.Attribute("icon_color").Value;
                    int track = 0;
                    if (w.Attribute("track") != null)
                        track = int.Parse(w.Attribute("track").Value);
                    else
                        validEntry = false;
                    int capacity = 0;
                    if (w.Attribute("capacity") != null)
                        capacity = int.Parse(w.Attribute("capacity").Value);
                    string name = "";
                    if (w.Attribute("name") != null)
                        name = w.Attribute("name").Value;
                    else
                        validEntry = false;
                    IMaterial cargo = new NullMaterial(dataManager.missingImage);
                    if (w.Attribute("cargo") != null)
                        cargo = dataManager.AssignMaterial(w.Attribute("cargo").Value);
                    int weightFull = 0;
                    if (w.Attribute("weight_full") != null)
                        weightFull = int.Parse(w.Attribute("weight_full").Value);
                    else
                        validEntry = false;
                    int startEpoch = 0;
                    int endEpoch = 0;
                    if (w.Attribute("epoch") != null) //epoch in game files is in range format, e.g. 1-3
                    {
                        startEpoch = int.Parse(w.Attribute("epoch").Value.First().ToString());
                        endEpoch = int.Parse(w.Attribute("epoch").Value.Last().ToString());
                    }
                    else
                        validEntry = false;

                    string whistle = "";
                    if (w.Attribute("sound_tunel") != null)
                        whistle = w.Attribute("sound_tunel").Value;
                    int maxSpeed = 0;
                    if (w.Attribute("max_speed") != null)
                        maxSpeed = int.Parse(w.Attribute("max_speed").Value);
                    int power = 0;
                    if (w.Attribute("power") != null)
                        power = int.Parse(w.Attribute("power").Value);

                    double length = 0;
                    if (w.Attribute("length") != null)
                        length = double.Parse(w.Attribute("length").Value, System.Globalization.CultureInfo.InvariantCulture);
                    else
                        validEntry = false;
                    if (w.Attribute("tail_length") != null)
                        length += double.Parse(w.Attribute("tail_length").Value, System.Globalization.CultureInfo.InvariantCulture);
                    if (w.Attribute("head_length") != null)
                        length += double.Parse(w.Attribute("head_length").Value, System.Globalization.CultureInfo.InvariantCulture);

                    BitmapImage iconImage;

                    if (w.Attribute("power") == null && vehicleType == 0 && validEntry && capacity > 0) // vehicle_type="0" = train, no power = wagon
                    {
                        iconImage = dataManager.ReadIcon(path, icon, iconTexture);
                        Wagons.Add(new Wagon(iconImage, length, ID, cost1, costToken1, cost2, costToken2, track, capacity, name, cargo, weightFull, startEpoch, endEpoch)); // improvement - use Builder pattern
                    }
                    else if (w.Attribute("power") != null && vehicleType == 0 && validEntry) //is engine
                    {
                        iconImage = dataManager.ReadIcon(path, icon, iconTexture);
                       // File.AppendAllText("trace.txt", "\nLoading engine" + name);
                        string XMLfuel = w.Attribute("fuel_cost").Value;
                        string[] fuels = XMLfuel.Split(';');
                        int fuelAmount1 = SeparateCost(fuels[0]);
                        IToken fuelToken1 = dataManager.AssignToken(SeparateType(fuels[0]));
                        int fuelAmount2 = 0;
                        IToken fuelType2 = null;
                        if (fuels.Length > 1)
                        {
                            fuelAmount2 = SeparateCost(fuels[1]);
                            fuelType2 = dataManager.AssignToken(SeparateType(fuels[1]));
                        }
                        Engines.Add(new Engine(whistle, fuelAmount1, fuelToken1, fuelAmount2, fuelType2, maxSpeed, power, iconImage, length, ID, cost1, costToken1, cost2, costToken2, track, capacity, name, cargo, weightFull, startEpoch, endEpoch)); // improvement - use Builder pattern
                    }
                    
                }
               // File.AppendAllText("trace.txt", "\nFile loaded");
                return;
            }
            catch 
            {
                //File.AppendAllText(Settings.Path + @"\InvalidXMLContent.xml", Data.ToString() + "\n" + e.Message + "\n\n");

                throw;
            }
        }

        public void ReadTokens(string path)
        {
            string pathXML = path + @"\config\cargo_types.xml";
           // File.AppendAllText("trace.txt", "\nLoading " + pathXML);
            XDocument Data;

            Data = CorrectXmlFile(pathXML);

            foreach (XElement e in Data.Element("root").Elements("TokenType"))
            {
                bool validEntry = true;
                string ID = "";
                if (e.Attribute("id") != null)
                    ID = e.Attribute("id").Value;
                else
                    validEntry = false;

                string nameHash = "";
                if (e.Attribute("name") != null)
                    nameHash = e.Attribute("name").Value;
                else
                    validEntry = false;

                string iconPath = "";
                if (e.Attribute("icon_texture") != null)
                    iconPath = e.Attribute("icon_texture").Value;
                else
                    validEntry = false;

                string iconHash = "";
                if (e.Attribute("icon") != null)
                    iconHash = e.Attribute("icon").Value;
                else
                    validEntry = false;

                string iconMiniHash = "";
                if (e.Attribute("icon_mini") != null)
                    iconMiniHash = e.Attribute("icon_mini").Value;
                else
                    validEntry = false;

                if (validEntry)
                {
                    BitmapImage iconMini = dataManager.ReadIcon(path, iconMiniHash, iconPath);
                    Token token = new Token(ID, nameHash, iconMini);
                    Tokens.Add(token);
                }
            }
           // File.AppendAllText("trace.txt", "\nFile loaded");
        }

        public void ReadMaterials(string path)
        {
            string pathXML = path + @"\config\cargo_types.xml";
           // File.AppendAllText("trace.txt", "\nLoading " + pathXML);
            XDocument Data;

            Data = CorrectXmlFile(pathXML);

            foreach (XElement e in Data.Element("root").Elements("CargoType"))
            {
                bool validEntry = true;
                string ID = "";
                if (e.Attribute("id") != null)
                    ID = e.Attribute("id").Value;
                else
                    validEntry = false;

                string nameHash = "";
                if (e.Attribute("name") != null)
                    nameHash = e.Attribute("name").Value;
                else
                    validEntry = false;

                string iconPath = "";
                if (e.Attribute("icon_texture") != null)
                    iconPath = e.Attribute("icon_texture").Value;
                else
                    validEntry = false;

                string iconHash = "";
                if (e.Attribute("icon") != null)
                    iconHash = e.Attribute("icon").Value;
                else
                    validEntry = false;

                string iconMiniHash = "";
                if (e.Attribute("icon_mini") != null)
                    iconMiniHash = e.Attribute("icon_mini").Value;
                else
                    validEntry = false;

                if (validEntry)
                {
                    BitmapImage iconMini = dataManager.ReadIcon(path, iconMiniHash, iconPath);
                    Material material = new Material(ID, nameHash, iconMini);
                    Materials.Add(material);
                }
            }
            //File.AppendAllText("trace.txt", "\nFile loaded");
        }




        public XDocument CorrectXmlFile(string path)
        {
            //  string testPath = Path.Combine(Environment.CurrentDirectory, path);
            try
            {
                StreamReader originalXml = new StreamReader(path);
                string validXml = RemoveComments(originalXml);
                validXml = RemoveDeclaration(validXml);
                validXml = RemoveInvalidEntities(validXml);
                validXml = AddRoot(validXml);
                //File.WriteAllText(Settings.Path + @"\Wagon_Types.xml", validXml);

                XDocument correctedFile = XDocument.Parse(validXml);
                return correctedFile;
            }
            catch (Exception)
            {
                StreamReader originalXml = new StreamReader(path);
                string invalidXml = RemoveComments(originalXml);
                invalidXml = RemoveDeclaration(invalidXml);
                invalidXml = RemoveInvalidEntities(invalidXml);
                invalidXml = AddRoot(invalidXml);
                throw;
            }

            // try
            {
                /*  XmlReaderSettings settings = new XmlReaderSettings();
                  settings.ConformanceLevel = ConformanceLevel.Fragment;
                  settings.IgnoreComments = true;
                  XmlReader xr = XmlReader.Create(Settings.Path + @"\Wagon_Types.xml", settings);

                  using (xr)
                  {
                      correctedFile = XDocument.Load(xr); 
                  }*/
            }
            // catch (Exception)
            {
                // MessageBox.Show("Invalid XML format");
                // throw new InvalidDataException("invalid XML");
            }



            /* XmlReaderSettings settings = new XmlReaderSettings();
             settings.ConformanceLevel = ConformanceLevel.Fragment;
             settings.IgnoreComments = true;
             XmlReader xr = XmlReader.Create(Settings.Path + "wg.xml", settings);

             using (xr)
             {
                 XDocument xDoc = XDocument.Load(xr);
                 return xDoc;
             }

            // using (StreamReader sr = new StreamReader(Path.Combine(Settings.GameFolderPath, path), true)) */

            /*{

                if (File.Exists(Path.Combine(Settings.GameFolderPath, path)))
                {

                    var doc = File.ReadAllText(Path.Combine(Settings.GameFolderPath, path)); // XDocument.Parse(File.ReadAllText(Path.Combine(Settings.GameFolderPath))); //XDocument.Load(reader);
                    doc = doc.Remove(0, 40);

                    var rootedDoc = "<root>" + doc + "</root>";
                    XDocument correctDoc = XDocument.Parse(rootedDoc);
                    return correctDoc;
                }
                else
                {
                    var doc = File.ReadAllText(path);
                    var rootedDoc = "<root>" + doc + "</root>";
                    XDocument correctDoc = XDocument.Parse(rootedDoc);
                    return correctDoc;
                }
            }*/
        }
        /// <summary>
        /// reads icons
        /// </summary>
        /// <param name="path">path from root folder</param>
        /// <param name="ID">icon ID in tcoords</param>
        /// <returns></returns>
        public int[] ReadIconCoords(string path, string ID)
        {


            int scale = 1;
            if (path.Contains("workshop") == false)
                scale = 2;
            try
            {
                XDocument data = CorrectXmlFile(Path.Combine(Settings.GameFolderPath, path));
                IEnumerable<XElement> matchElements = from el in data.Element("root").Elements("Coord")
                                                      where el.Attribute("id").Value == ID
                                                      select el;
                XElement element = matchElements.First();
                int[] coords = new int[4];
                // if (ID == "829B3189")
                coords[0] = int.Parse(element.Attribute("x").Value) * scale;
                coords[1] = int.Parse(element.Attribute("y").Value) * scale;
                coords[2] = int.Parse(element.Attribute("w").Value) * scale;
                coords[3] = int.Parse(element.Attribute("h").Value) * scale;
                return coords;
            }
            catch (Exception)
            {
                int[] coords = new int[4] { 0, 0, 0, 0 };
                return coords;
            }


        }

        public string ReadModName(string path)
        {
            try
            {
                XDocument meta;
                meta = CorrectXmlFile(path);
                XElement Name = meta.Element("root").Element("name");
                string name = Name.Attribute("value").Value;
                return name;
            }
            catch (Exception)
            {
                return "Unknown";
            }

        }

        public long ReadModID(string path)
        {
            try
            {
                XDocument meta;
                meta = CorrectXmlFile(path);
                XElement ID = meta.Element("root").Element("id");
                long id = long.Parse(ID.Attribute("value").Value);
                return id;
            }
            catch (Exception)
            {
                return 00000000;
            }
        }

        public List<long> ReadDisabledMods(string path)
        {
            List<long> disabledMods = new List<long>();
            XDocument setup;
            try
            {
                setup = CorrectXmlFile(path);
                foreach (XElement m in setup.Element("root").Element("params").Element("mods_disabled").Elements("mod"))
                {
                    if (m.Attribute("id") != null)
                    disabledMods.Add(long.Parse(m.Attribute("id").Value));
                }
            }
            catch
            {
                disabledMods.Add(00000000);
            }
            return disabledMods;
            
        }

        public int SeparateCost(string s)
        {

            string[] temp = s.Split('[');  // type is in brackets, used to split
            int cost = int.Parse(temp[0]);
            return cost;
        }
        public string SeparateType(string s)
        {
            string[] temp = s.Split('[');  // type is in brackets, used to split
            string type = "";
            if (temp.Length > 1) // type is specified
                type = temp[1].Replace("]", string.Empty); //remove ending bracket
            else
                type = "F0000000"; // default type when not specified in game files = money hash

            return type;
        }

        public string RemoveComments(StreamReader stream)
        {
            string removed = Regex.Replace(stream.ReadToEnd(), "<!--.*?-->", "", RegexOptions.Singleline);
            return removed;
        }

        public string RemoveDeclaration(string s)
        {
            string removed = Regex.Replace(s, "<\\?.*?\\?>", "", RegexOptions.Singleline);
            return removed;
        }

        public string RemoveInvalidEntities(string s)
        {
            string removed = Regex.Replace(s, "&", "");
            removed = Regex.Replace(removed, "\u0004", "");
            removed = Regex.Replace(removed, "/n>", "");
            removed = Regex.Replace(removed, "/ >", "/>");
            return removed;
        }

        public string AddRoot(string s)
        {
            return "<root>" + s + "</root>";
        }

      /*  public XDocument RemoveDuplicates(string s)
        {

            s = "!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n" + s;
            XDocument removed = LoadHTMLasXML(s);
            return removed;
        }

        public XDocument LoadHTMLasXML(string url)
        {
            var web = new HtmlWeb();
            var ms = new MemoryStream();
            var xtw = new XmlTextWriter(ms, null);

            web.Load(ms);

            ms.Position = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml((new StreamReader(ms)).ReadToEnd());

            using (var nodeReader = new XmlNodeReader(xmlDoc))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }*/
    }
}

