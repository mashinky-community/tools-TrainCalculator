using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

namespace MashinkyCalculator
{
    public class UserDataContext : INotifyPropertyChanged
    {
        public DataManager dataManager;
        private DataFilter filter;
        public int MaxTrainLength { get; set; }

        public int SelectedEpoch { get; set; }
        public List<IToken> AvailableFuels { get; private set; }
        public IToken FuelFilter { get; set; }

        public List<IMaterial> AvailableCargos { get; private set; }
        public IMaterial Cargo1TypeFilter { get; set; }
        public IMaterial Cargo2TypeFilter { get; set; }

        public List<IToken> AvailableCostTokens1 { get; private set; }
        public List<IToken> AvailableCostTokens2 { get; private set; }
        public IToken Cargo1CostFilter { get; set; }

        public IToken Cargo2CostFilter { get; set; }

        public int RatioCargo1 { get; set; }
        public int RatioCargo2 { get; set; }
        public int PriorityCargo1
        {
            get
            {
                return Convert.ToInt32((RatioCargo1 + 0.0) / (RatioCargo1 + RatioCargo2 + 0.0) * 100);
            }
        }

        public int PriorityCargo2
        {
            get
            {
                return Convert.ToInt32((RatioCargo2 + 0.0) / (RatioCargo1 + RatioCargo2 + 0.0) * 100);
            }
        }
        public int Speed { get; set; }

        private Calculator calculator;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<CalculatedTrain> calculatedTrains { get; private set; }

        private MediaPlayer trackPLayer;

        public UserDataContext(DataManager dataManager)
        {
            this.dataManager = dataManager;
            dataManager.GetGameData();
            filter = new DataFilter(dataManager.LoadedWagons, dataManager.LoadedEngines);
            calculator = new Calculator(this, filter);
            UpdateAllFilters();
            MaxTrainLength = 6;
            SelectedEpoch = 7;
            RatioCargo1 = 1;
            RatioCargo2 = 1;
            trackPLayer = new MediaPlayer();
            OpenTrack();
            TriggerTrack();

        }

        protected void RaiseChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public void UpdateAllFilters()
        {
            AvailableFuels = filter.AvailableFuels(SelectedEpoch);
            RaiseChange("AvailableFuels");
            AvailableCargos = filter.AvailableCargos(SelectedEpoch);
            RaiseChange("AvailableCargos");
        }

        public void UpdateCargo1()
        {
            AvailableCostTokens1 = filter.AvailableCostTokens(SelectedEpoch, Cargo1TypeFilter);
            RaiseChange("AvailableCostTokens1");
        }

        public void UpdateCargo2()
        {
            AvailableCostTokens2 = filter.AvailableCostTokens(SelectedEpoch, Cargo2TypeFilter);
            RaiseChange("AvailableCostTokens2");
        }

        public void CalculateTrain()
        {
            calculatedTrains = calculator.CalculateTrains();
            SortTrains();
            RaiseChange("calculatedTrains");
        }

        public void SortTrains()
        {
            if (calculatedTrains != null)
            {
                switch (Settings.ResultPriority)
                {
                    case "capacity":
                        calculatedTrains = (from t in calculatedTrains
                                            orderby t.TotalCapacity descending
                                            select t).ToList();
                        break;
                    case "cost":
                        calculatedTrains = (from t in calculatedTrains
                                            orderby t.CostRating
                                            select t).ToList();
                        break;
                    case "fuel":
                        calculatedTrains = (from t in calculatedTrains
                                            orderby t.FuelRating
                                            select t).ToList();
                        break;
                    case "combined":
                        calculatedTrains = (from t in calculatedTrains
                                            orderby t.Rating
                                            select t).ToList();
                        break;
                    default:
                        break;
                }
            }

        }

        private void OpenTrack()
        {
            string trackPath = (dataManager.gameFolderPath + "\\media\\music\\Mashinky OST - 04 Charcoal Au Four - Full_Loopable.ogg");
            if (File.Exists(trackPath))
            {
                var uri = new System.Uri(trackPath);
                trackPLayer.Open(uri);
            }
        }

        public void TriggerTrack()
        {
            if (trackPLayer != null)
            {
                if (Settings.Music)
                {
                    trackPLayer.Play();
                }
                else
                    trackPLayer.Stop();
            }
        }

        private void Media_Ended(object sender, EventArgs args)
        {
            trackPLayer.Position = TimeSpan.Zero;
            trackPLayer.Play();
        }


    }
}
