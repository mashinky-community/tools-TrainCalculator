using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyCalculator
{
    public class Train
    {
        public Engine Engines { get; private set; }
        public int EngineCount { get; private set; }
        public IWagon Wagon1 { get; private set; }
        public int Wagon1Count { get; private set; }
        public IWagon Wagon2 { get; private set; }
        public int Wagon2Count { get; private set; }
        public int CapacityCargo1 { get; private set; }
        public IMaterial TypeCargo1 { get; private set; }
        public int CapacityCargo2 { get; private set; }
        public IMaterial TypeCargo2 { get; private set; }
        public int TotalCapacity { get; private set; }
        public double Length { get; private set; }
        public string EnginesCost1P { get; private set; }
        public string EnginesCost2P { get; private set; }
        public string EnginesFuel1P { get; private set; }
        public string EnginesFuel2P { get; private set; }
        public string Wagons1Cost1P { get; private set; }
        public string Wagons1Cost2P { get; private set; }
        public string Wagons2Cost1P { get; private set; }
        public string Wagons2Cost2P { get; private set; }
        public string TotalCost { get; private set; }
        public string EnginesP
        {
            get { return $"{EngineCount}x {Engines}"; }
        }
        public string Wagons1P
        {
            get { return $"{Wagon1Count}x {Wagon1}"; }
        }
        public string Wagons2P
        {
            get { return $"{Wagon2Count}x {Wagon2}"; }
        }

        public string Speed
        {
            get
            {
                if (Settings.Miles == true)
                    return Engines.MaxSpeed + " mph";
                else
                    return Engines.MaxSpeed + " kph";
            }
        }

        public Train(Engine engine, int engineCount, IWagon wagon1, int wagon1Count, IWagon wagon2, int wagon2Count, int capacityCargo1, IMaterial typeCargo1, int capacityCargo2, IMaterial typeCargo2)
        {
            Engines = engine;
            EngineCount = engineCount;
            Wagon1 = wagon1;
            Wagon1Count = wagon1Count;
            Wagon2 = wagon2;
            Wagon2Count = wagon2Count;
            CapacityCargo1 = capacityCargo1;
            TypeCargo1 = typeCargo1;
            CapacityCargo2 = capacityCargo2;
            TypeCargo2 = typeCargo2;
            TotalCapacity = capacityCargo1 + capacityCargo2;
            CalcLength();
            CalcEnginesCost();
            CalcEnginesFuel();
            CalcWagons1Cost();
            CalcWagons2Cost();
            CalcTotalCost();
        }

        public void CalcLength()
        {
            Length = Math.Round(EngineCount * Engines.Length + Wagon1Count * Wagon1.Length + Wagon2Count * Wagon2.Length, 2);
        }

        private void CalcEnginesCost()
        {
            EnginesCost1P = $"{Engines.CostAmount1 * EngineCount}";

            if (Engines.CostAmount2 != 0)
                EnginesCost2P = $"{Engines.CostAmount2 * EngineCount}";
            else
                EnginesCost2P = "";

        }

        private void CalcEnginesFuel()
        {
                EnginesFuel1P = $" {Engines.FuelAmount1 * EngineCount * -1}";

            if (Engines.FuelAmount2 != 0)
                EnginesFuel2P = $"{Engines.FuelAmount2 * EngineCount * -1}";
            else
                EnginesFuel2P = "";
        }

        private void CalcWagons1Cost()
        {
            if (Wagon1.CostAmount1 != 0)
                Wagons1Cost1P = $"{ Wagon1.CostAmount1 * Wagon1Count}";
            else
                Wagons1Cost1P = "";
            if (Wagon1.CostAmount2 != 0)
                Wagons1Cost2P = $"{ Wagon1.CostAmount2 * Wagon1Count}";
            else
                Wagons1Cost2P = "";
        }

        private void CalcWagons2Cost()
        {
            if (Wagon2.CostAmount1 != 0)
                Wagons2Cost1P = $"{ Wagon2.CostAmount1 * Wagon2Count}";
            else
                Wagons2Cost1P = "";
            if (Wagon2.CostAmount2 != 0)
                Wagons2Cost2P = $"{ Wagon2.CostAmount2 * Wagon2Count}";
            else
                Wagons2Cost2P = "";
        }

        private void CalcTotalCost()
        {
            // if engine cost same token as both wagons
            if (Engines.CostToken1 == Wagon1.CostToken1 && Engines.CostToken1 == Wagon2.CostToken1)
            {
                TotalCost = String.Format("{0:#%;;' '}", (Engines.CostAmount1 * EngineCount + Wagon1.CostAmount1 * Wagon1Count + Wagon2.CostAmount1 * Wagon2Count) * -1 + " " + Engines.CostToken1 + (Engines.CostAmount2 * EngineCount * -1).ToString(";;' '") + Engines.CostToken2);
                TotalCost = TotalCost.Replace("<None>", "");
            }
            else if (Engines.CostToken2 == Wagon1.CostToken1 && Engines.CostToken1 == Wagon2.CostToken1)
            {
                TotalCost = String.Format("{0:#%;;' '}", (Engines.CostAmount2 * EngineCount + Wagon1.CostAmount1 * Wagon1Count + Wagon2.CostAmount1 * Wagon2Count) * -1 + " " + Engines.CostToken2 + Engines.CostAmount1 * EngineCount * -1 + Engines.CostToken1);
                TotalCost = TotalCost.Replace("<None>", "");
            }

            // if engine cost same token as any sigle wagons
            if (Engines.CostToken1 == Wagon1.CostToken1)
            {
                TotalCost = String.Format("{0:#%;;' '}", (Engines.CostAmount1 * EngineCount + Wagon1.CostAmount1 * Wagon1Count) * -1 + " " + Engines.CostToken1 + (Engines.CostAmount2 * EngineCount * -1).ToString(";;' '") + " " + Engines.CostToken2 + (Wagon2.CostAmount1 * Wagon2Count * -1).ToString(";;' '") + " " + Wagon2.CostToken1);
                TotalCost = TotalCost.Replace("<None>", "");
            }
            else if (Engines.CostToken2 == Wagon1.CostToken1)
            {
                TotalCost = String.Format("{0:#%;;' '}", (Engines.CostAmount2 * EngineCount + Wagon1.CostAmount1 * Wagon1Count) * -1 + " " + Engines.CostToken2 + (Engines.CostAmount1 * EngineCount * -1).ToString(";;' '") + " " + Engines.CostToken1 + (Wagon2.CostAmount1 * Wagon2Count * -1).ToString(";;' '") + " " + Wagon2.CostToken1);
                TotalCost = TotalCost.Replace("<None>", "");
            }
            else if (Engines.CostToken1 == Wagon2.CostToken1)
            {
                TotalCost = String.Format("{0:#%;;' '}", (Engines.CostAmount1 * EngineCount + Wagon2.CostAmount1 * Wagon2Count) * -1 + " " + Engines.CostToken1 + (Engines.CostAmount2 * EngineCount * -1).ToString(";;' '") + " " + Engines.CostToken2 + " " + Wagon1.CostAmount1 * Wagon1Count * -1 + " " + Wagon1.CostToken1);
                TotalCost = TotalCost.Replace("<None>", "");
            }
            else if (Engines.CostToken2 == Wagon2.CostToken1)
            {
                TotalCost = String.Format("{0:#%;;' '}", (Engines.CostAmount2 * EngineCount + Wagon2.CostAmount1 * Wagon2Count) * -1 + " " + Engines.CostToken2 + Engines.CostAmount1 * EngineCount * -1 + " " + Engines.CostToken1 + " " + Wagon1.CostAmount1 * Wagon1Count * -1 + " " + Wagon1.CostToken1);
                TotalCost = TotalCost.Replace("<None>", "");
            }
            // if wagons cost same token
            if (Wagon1.CostToken1 == Wagon2.CostToken1)
            {
                TotalCost = Engines.CostAmount1 * EngineCount * -1 + " " + Engines.CostToken1 + (Engines.CostAmount2 * EngineCount * -1).ToString(";;' '") + " " + Engines.CostToken2 + (Wagon1.CostAmount1 * Wagon1Count + Wagon2.CostAmount2 * Wagon2Count) * -1 + " " + Wagon1.CostToken1;
                TotalCost = TotalCost.Replace("<None>", "");

            }

            if (String.IsNullOrEmpty(TotalCost))
            {
                TotalCost = Engines.CostAmount1 * EngineCount * -1 + " " + Engines.CostToken1 + (Engines.CostAmount2 * EngineCount * -1).ToString(";;' '") + " " + Engines.CostToken2 + Wagon1.CostAmount1 * Wagon1Count * -1 + " " + Wagon1.CostToken1 + (Wagon2.CostAmount2 * Wagon2Count * -1).ToString(";;' '") + " " + Wagon2.CostToken1;
                TotalCost = TotalCost.Replace("<None>", "");
            }
        }
    }
}
