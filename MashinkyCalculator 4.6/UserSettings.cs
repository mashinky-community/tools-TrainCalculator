using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyCalculator
{
    public class UserSettings
    {
        public string GameFolderPath { get; set; }
        public string Language { get; set; }
        public string ResultPriority { get; set; }
        public int SpeedTolerance { get; set; }
        public bool CapacityPriorMarked { get; set; }
        public bool CostPriorMarked { get; set; }
        public bool FuelPriorMarked { get; set; }
        public bool CombinedPriorMarked { get; set; }
        private DataManager dataManager;
        public bool Miles { get; set; }

        public UserSettings(DataManager dataManager)
        {
            GameFolderPath = Settings.GameFolderPath;
            ResultPriority = Settings.ResultPriority;
            SpeedTolerance = Settings.SpeedTolerance;
            Miles = Settings.Miles;
            this.dataManager = dataManager;
            UpdateCheckBoxes();
        }

        public void UpdateResultPriority()
        {
            if (CapacityPriorMarked)
                ResultPriority = "capacity";
            if (CostPriorMarked) 
                ResultPriority = "cost";
            if (FuelPriorMarked)
                ResultPriority = "fuel";
            if (CombinedPriorMarked)
                ResultPriority = "combined";
        }

        public void SaveSettings()
        {
            Settings.SetGameFolder(GameFolderPath);
           // Settings.SetLanguage(Language);
            Settings.SetResultPriority(ResultPriority);
            Settings.SetSpeedTolerance(SpeedTolerance);
            Settings.SetMiles(Miles);
            dataManager.SaveSettings();
        }

        public void CancelSettings()
        {
            GameFolderPath = Settings.GameFolderPath;
            ResultPriority = Settings.ResultPriority;
            SpeedTolerance = Settings.SpeedTolerance;
            Miles = Settings.Miles;
        }

        public void UpdateCheckBoxes()
        {
            switch (ResultPriority)
            {
                case "capacity":
                    CapacityPriorMarked = true;
                    break;
                case "cost":
                    CostPriorMarked = true;
                    break;
                case "fuel":
                    FuelPriorMarked = true;
                    break;
                case "combined":
                    CombinedPriorMarked = true;
                    break;
                default:
                    break;
            }
        }
    }
}
