using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;


namespace MashinkyCalculator
{
    public class Engine : Wagon
    {


        public string Whistle { get; private set; }
        public double FuelAmount1 { get; private set; }
        public IToken FuelToken1 { get; private set; }
        public double FuelAmount2 { get; private set; }
        public IToken FuelToken2 { get; private set; }
        private int maxSpeed;
        public int MaxSpeed
        {
            get
            {
                if (Settings.Miles)
                    return Convert.ToInt32(maxSpeed * 0.621371192);
                else
                    return maxSpeed;
            }
            private set
            {
                maxSpeed = value;
            }
        }
        public int Power { get; private set; }
        public int RealPower { get; private set; }

        public Engine(string whistle, double fuelAmount1, IToken fuelToken1, double fuelAmount2, IToken fuelToken2, int maxSpeed, int power, BitmapImage iconImage, double length, string iD, int costAmount1, IToken costToken1, int costAmount2, IToken costToken2, int trackType, int capacity, string name, IMaterial cargo, int weightFull, int startEpoch, int endEpoch) : base(iconImage, length, iD, costAmount1, costToken1, costAmount2, costToken2, trackType, capacity, name, cargo, weightFull, startEpoch, endEpoch)
        {
            Whistle = whistle;
            FuelAmount1 = fuelAmount1 * (-1);
            FuelToken1 = fuelToken1;
            FuelAmount2 = fuelAmount2 * (-1);
            FuelToken2 = fuelToken2;
            MaxSpeed = maxSpeed;
            Power = power;
            // RealPower = (int)Math.Round((double)power / MaxSpeed * 42.0); // how much weight can engine pul, old unprecise formula
            RealPower = (int)Math.Round(Power * 0.745699872 / (maxSpeed / 3.6) / (9.81 * 0.0065)); // how much weight can engine pull, formula from Mashinky code
        }


    }
}
