using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyCalculator
{

    public class CalculatedTrain : Train
    {
        /// <summary>
        /// all properties are user settings used for creating this train
        /// </summary>
        public int EvaluationFuel { get; private set; }
        public int EvaluationCost { get; private set; }
        public int FuelRating { get; set; }
        public int CostRating { get; set; }
        public int Capacity1Rating { get; set; }
        public int Capacity2Rating { get; set; }
        public int Rating
        {
            get
            {
                return FuelRating + CostRating + Capacity1Rating + Capacity2Rating;
            }
            set
            {

            }
        }
        public bool Wagon2Selected
        {
            get
            {
                return Wagon2 is Wagon;

            }
            set
            {

            }
        }

        public bool EngineTwoCosts
        {
            get
            {
                return Engines.CostToken2 is Token;
            }
            set
            {

            }
        }

        public string Debug
        {
            get
            {
               // return "";
                return String.Format($" Dbg Fuel rating = {FuelRating} Evaluation cost = {EvaluationCost}  Cost rating = {CostRating}  Capacity = {TotalCapacity} Overall rating = {Rating}");
            }
        }



        public CalculatedTrain(int evaluationCost, int evaluationFuel, Engine engine, int engineCount, IWagon wagon1, int wagon1Count, IWagon wagon2, int wagon2Count, int capacityCargo1, IMaterial typeCargo1, int capacityCargo2, IMaterial typeCargo2) : base(engine, engineCount, wagon1, wagon1Count, wagon2, wagon2Count, capacityCargo1, typeCargo1, capacityCargo2, typeCargo2)
        {
            EvaluationFuel = evaluationFuel;
            EvaluationCost = evaluationCost;
        }

        private string GetEnginesCost()
        {
            string costEngine = "";
            if (Engines.CostAmount2 == 0)
            {
                costEngine = $"{Engines.CostAmount1 * EngineCount * -1}x {Engines.CostToken1}";
            }
            else
            {
                costEngine = $"{Engines.CostAmount1 * EngineCount * -1}x {Engines.CostToken1} + {Engines.CostAmount2 * EngineCount * -1}x {Engines.CostToken2}";
            }
            return costEngine;
        }
            
        




        public override string ToString()
        {
            


            string cargo = "";
            if (CapacityCargo2 == 0)
                cargo = $"{CapacityCargo1}x {TypeCargo1}";
            else
            {
                cargo = $"{CapacityCargo1}x {TypeCargo1} + {CapacityCargo2}x {TypeCargo2}";
            }

            return "";
        }
    }
}

