using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace MashinkyCalculator
{
    public class DataManager
    {
        private XMLReader XMLReader;
        private DataWriter dataWriter;
        // private HashConvertor hashConvertor;
        private ImageReader imageReader;
        private string[] VehicleModPaths;
        private string[] TokenModPaths;
        private List<string> LoadedMods = new List<string>();
        private List<string> FailedMods = new List<string>();
        public BitmapImage missingImage;
        private NullToken nullToken;
        private NullMaterial nullMaterial;
        private List<long> disabledMods;
        public List<Wagon> LoadedWagons
        {
            get
            {
                return XMLReader.Wagons;
            }
            private set
            {
            }
        }
        public List<Engine> LoadedEngines
        {
            get
            {
                return XMLReader.Engines;
            }
            private set
            {
            }
        }
        public List<Token> LoadedTokens
        {
            get
            {
                return XMLReader.Tokens;
            }
            private set
            {
            }
        }
        public List<Material> LoadedMaterials
        {
            get
            {
                return XMLReader.Materials;
            }
            private set
            {
            }
        }

        public string gameFolderPath;
        public string steamLibraryFolder;


        public DataManager()
        {
            missingImage = CreateBlankImage();
            nullToken = new NullToken(missingImage);
            nullMaterial = new NullMaterial(missingImage);
            //   hashConvertor = new HashConvertor();
            dataWriter = new DataWriter();
            gameFolderPath = GetGameFolder();
            steamLibraryFolder = GetLibraryFolder();
            GetSettings();
            Settings.SetGameFolder(gameFolderPath);
            try
            {
                imageReader = new ImageReader(missingImage);
                XMLReader = new XMLReader(imageReader, this);
            }
            catch 
            {
                gameFolderPath = Environment.CurrentDirectory;
                Settings.SetGameFolder(gameFolderPath);
                steamLibraryFolder = GetLibraryFolder();
                imageReader = new ImageReader(missingImage);
                XMLReader = new XMLReader(imageReader, this);
            }
            
        }

        public void GetSettings()
        {
            XDocument settings;
            if (File.Exists(Path.Combine(Settings.Path, "settings.xml")))
            {
                settings = XDocument.Load(Path.Combine(Settings.Path, "settings.xml"));
                //File.AppendAllText("trace.txt", "\nOpened settings in " + Settings.Path);
            }
            else
            {
                settings = dataWriter.CreateSettingsFile(gameFolderPath);
                //File.AppendAllText("trace.txt", "\nCreated settings in " + Settings.Path);
            }

            Settings.SetGameFolder(settings.Element("Settings").Element("GameFolder").Value);
            Settings.SetLanguage(settings.Element("Settings").Element("Language").Value);
            Settings.SetResultPriority(settings.Element("Settings").Element("ResultPriority").Value);
            Settings.SetSpeedTolerance(Convert.ToInt32(settings.Element("Settings").Element("SpeedTolerance").Value));
            Settings.SetMusic(Convert.ToBoolean(settings.Element("Settings").Element("Music").Value));
        }

        public void SaveSettings()
        {
            dataWriter.WriteSettings();
        }
        /// <summary>
        /// read both vanila and all mods wagon_types
        /// </summary>
        public void GetGameData()
        {
            try
            {
                LoadVanilaData();
                //File.AppendAllText("trace.txt", "\nVanila data loaded\n");
            }
            catch (Exception)
            {
                //File.AppendAllText("trace.txt", "\nVanila data load failed\n");
                MessageBox.Show("Failed to read vanila files, please place this app into Mashinky game folder.");
                throw;
            }

            GetModsData();
           // File.AppendAllText("trace.txt", "\n Mods loaded");
            ShowModsLoadResult();
        }

        public void LoadVanilaData()
        {
            string path = gameFolderPath + @"\media";
            XMLReader.ReadTokens(path);
            XMLReader.ReadMaterials(path);
            XMLReader.ReadWGT(path);  // read vanila WGT
            disabledMods = XMLReader.ReadDisabledMods(gameFolderPath + "\\setup.xml");

        }

        public void GetModsData()
        {
            try
            {
                VehicleModPaths = Directory.GetFiles(steamLibraryFolder + @"\workshop\content\598960\", @"wagon_types.xml", SearchOption.AllDirectories);
                TokenModPaths = Directory.GetFiles(steamLibraryFolder + @"\workshop\content\598960\", @"cargo_types.xml", SearchOption.AllDirectories);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to find mods folder. If you have any mods, please contact app creator");
                return;
            }
            
            foreach (string path in VehicleModPaths)
            {
                string modFolder = Directory.GetParent(Directory.GetParent(path).ToString()).ToString(); //mod root directory
                long modID = XMLReader.ReadModID(modFolder + @"\meta.xml");
                if (!disabledMods.Contains(modID))
                    {
                    try
                    {
                        XMLReader.ReadWGT(modFolder);
                        LoadedMods.Add(GetModName(modFolder));
                    }
                    catch (Exception)
                    {
                        FailedMods.Add(GetModName(modFolder));
                      //  File.AppendAllText("trace.txt", "\nIvalid XML file in mod " + GetModName(modFolder));
                    }
                }
            }

            foreach (string path in TokenModPaths)
            {
                string modFolder = Directory.GetParent(Directory.GetParent(path).ToString()).ToString();
                try
                {
                    XMLReader.ReadTokens(modFolder);
                    XMLReader.ReadMaterials(modFolder);
                    LoadedMods.Add(GetModName(modFolder));
                }
                catch (Exception)
                {
                 //   File.AppendAllText("trace.txt", "\nIvalid XML file in mod " + GetModName(modFolder));
                    FailedMods.Add(GetModName(modFolder));
                }
            }
        }

        public string GetModName(string path)
        {
            string name = XMLReader.ReadModName(path + @"\meta.xml");
            return name;
        }

        public void ShowModsLoadResult()
        {
            MessageBox.Show("Successfuly loaded mods:\n" + String.Join(", ", LoadedMods) + "\n\n Failed to load mods:\n" + String.Join(", ", FailedMods));
          //  MessageBox.Show("Disabled mods" + String.Join(", ", disabledMods));
        }

        public string GetGameFolder()
        {

            try
            {
                RegistryKey localKey;
                if (Environment.Is64BitOperatingSystem)
                    localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                else
                    localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                // using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam"))
                using (RegistryKey registryKey = localKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 598960"))
                {

                    // string path = (string)registryKey.GetValue("InstallPath");
                    string path = (string)registryKey.GetValue("InstallLocation");
                    //File.AppendAllText("trace.txt", "\nGame folder path:" + path);
                    return path;
                }
            }
            catch (Exception)
            {
                //  return AppDomain.CurrentDomain.BaseDirectory;
                return Environment.CurrentDirectory;
                //MessageBox.Show("Failed to find game folder");
                //return "";
            }

        }

        public string GetLibraryFolder()
        {
            string library = Directory.GetParent(Directory.GetParent(gameFolderPath).ToString()).ToString();
            //File.AppendAllText("trace.txt", "\nLibrary folder is " + library);
            return library;
        }

        /*   public string GetSteamFolder()
           {
               try
               {
                   RegistryKey localKey;
                   if (Environment.Is64BitOperatingSystem)
                       localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                   else
                       localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                   using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam"))
                   {

                       string path = (string)registryKey.GetValue("InstallPath");
                       return path;
                   }
               }
               catch (Exception)
               {
                   MessageBox.Show("Failed to find Steam folder");
                   return "";
               }
           } */

        public BitmapImage ReadIcon(string currentFolder, string ID, string iconTexturePath)
        {
            /* if (ID == "CD00E351")
             {
                 string debugStopForMods = "";
             }*/
            BitmapImage iconImage = null;
            if (String.IsNullOrEmpty(ID) || String.IsNullOrEmpty(iconTexturePath) || iconTexturePath.Contains("unique"))
            {
                iconImage = missingImage;
            }
            else
            {
                string pathTcoords = currentFolder + "\\config\\tcoords.xml";
                int[] iconCoords = XMLReader.ReadIconCoords(pathTcoords, ID);
                if (iconCoords[2] == 0 || iconCoords[3] == 0)
                    iconImage = missingImage;
                else
                    iconImage = imageReader.ReadIcon(currentFolder + "/" + iconTexturePath, iconCoords);
            }
            return iconImage;
        }

        public IMaterial AssignMaterial(string cargoHash)
        {
            List<Material> matchingMats = (from m in LoadedMaterials
                                           where m.ID == cargoHash
                                           select m).ToList();
            if (matchingMats.Count == 0)
                return nullMaterial;
            else
                return matchingMats.First();
        }

        public IToken AssignToken(string hash)
        {
            List<Token> matchingTokens = (from t in LoadedTokens
                                          where t.ID == hash
                                          select t).ToList();
            if (matchingTokens.Count == 0)
                return nullToken;
            else
                return matchingTokens.First();
        }

        public BitmapImage CreateBlankImage()
        {
            BitmapImage bmpImage = new BitmapImage();
            Bitmap image = new Bitmap(10, 10);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    image.SetPixel(x, y, System.Drawing.Color.Crimson);
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                bmpImage.BeginInit();
                bmpImage.StreamSource = ms;
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.EndInit();
            }

            return bmpImage;
        }


    }
}
