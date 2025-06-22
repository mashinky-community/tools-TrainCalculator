using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyCalculator
{
    public class Evaluator
    {
        // engine fuel values
        private int fMoney = 1;
        private int fCoal = 3;
        private int fDiesel = 6;
        private int fElectricity = 10;

        // cost values;
        private int cMoney = 1;
        private int cPlank = 2;
        private int cIron = 6;
        private int cCoal = 4;
        private int cDiesel = 6;
        private int cSteel = 8;
        private int cElectricity = 10;

        //token names hashes
        private const string moneyNH = "0BA45900";
        private const string timberNH = "0BA45901";
        private const string coalNH = "0BA45902";
        private const string ironNH = "0BA45903";
        private const string dieselNH = "0BA45904";
        private const string steelNH = "0BA45905";
        private const string energyNH = "0BA45906";
        private const string cementNH = "0BA45907";
        private const string electronicsNH = "0BA45908";



        public int EvaluateFuel(Engine engine)
        {
            double evaluation = 0;
            switch (engine.FuelToken1.NameHash)
            {
                case moneyNH:
                    evaluation += fMoney * engine.FuelAmount1;
                    break;
                case coalNH:
                    evaluation += fCoal * engine.FuelAmount1;
                    break;
                case dieselNH:
                    evaluation += fDiesel * engine.FuelAmount1;
                    break;
                case energyNH:
                    evaluation += fElectricity * engine.FuelAmount1;
                    break;
                default:
                    break;
            }
            if (engine.FuelToken2 != null)
                switch (engine.FuelToken2.NameHash)
                {
                    case moneyNH:
                        evaluation += fMoney * engine.FuelAmount2;
                        break;
                    case coalNH:
                        evaluation += fCoal * engine.FuelAmount2;
                        break;
                    case dieselNH:
                        evaluation += fDiesel * engine.FuelAmount2;
                        break;
                    case energyNH:
                        evaluation += fElectricity * engine.FuelAmount2;
                        break;
                    default:
                        break;
                }
            return (int)evaluation;
        }

        public int EvaluateCost(Wagon wagon)
        {
            int evaluation = 0;
            switch (wagon.CostToken1.NameHash)
            {
                case moneyNH:
                    evaluation += cMoney * wagon.CostAmount1;
                    break;
                case timberNH:
                    evaluation += cPlank * wagon.CostAmount1;
                    break;
                case coalNH:
                    evaluation += cCoal * wagon.CostAmount1;
                    break;
                case ironNH:
                    evaluation += cIron * wagon.CostAmount1;
                    break;
                case dieselNH:
                    evaluation += cDiesel * wagon.CostAmount1;
                    break;
                case steelNH:
                    evaluation += cSteel * wagon.CostAmount1;
                    break;
                case energyNH:
                    evaluation += cElectricity * wagon.CostAmount1;
                    break;
                default:
                    break;
            }

            if (wagon.CostToken2 != null)
                switch (wagon.CostToken2.NameHash)
                {
                    case moneyNH:
                        evaluation += cMoney * wagon.CostAmount2;
                        break;
                    case timberNH:
                        evaluation += cPlank * wagon.CostAmount2;
                        break;
                    case coalNH:
                        evaluation += cCoal * wagon.CostAmount2;
                        break;
                    case ironNH:
                        evaluation += cIron * wagon.CostAmount2;
                        break;
                    case steelNH:
                        evaluation += cSteel * wagon.CostAmount2;
                        break;
                    case energyNH:
                        evaluation += cElectricity * wagon.CostAmount2;
                        break;
                    default:
                        break;
                }
            return evaluation;

        }
        /// <summary>
        /// Rates train by ordering them by evaluation in each category
        /// </summary>
        /// <param name="trains"></param>
        public void GiveRatings(List<CalculatedTrain> trains)
        {
            List<CalculatedTrain> sorted = (from t in trains
                                            orderby t.EvaluationCost descending
                                            select t).ToList();
            int ratingIndex = 1;

            for (int i = 0; i < sorted.Count; i++)
            {
                if (i > 0 && sorted[i].EvaluationCost == sorted[i - 1].EvaluationCost) // give equal rating to trains with equal fuel consumption
                    sorted[i].CostRating = sorted[i - 1].CostRating;
                else
                {
                    sorted[i].CostRating = ratingIndex;
                    ratingIndex++;
                }
            }

            sorted = (from t in trains
                      orderby t.EvaluationFuel
                      select t).ToList();
            ratingIndex = 1;

            for (int i = 0; i < sorted.Count; i++)
            {
                if (i > 0 && sorted[i].EvaluationFuel == sorted[i - 1].EvaluationFuel) // give equal rating to trains with equal fuel consumption
                    sorted[i].FuelRating = sorted[i - 1].FuelRating;
                else
                {
                    sorted[i].FuelRating = ratingIndex;
                    ratingIndex++;
                }
            }

            sorted = (from t in trains
                      orderby t.CapacityCargo1 descending
                      select t).ToList();
            ratingIndex = 1;
            for (int i = 0; i < sorted.Count; i++)
            {
                if (i > 0 && sorted[i].CapacityCargo1 == sorted[i - 1].CapacityCargo1)
                    sorted[i].Capacity1Rating = sorted[i - 1].Capacity1Rating;
                else
                {
                    sorted[i].Capacity1Rating = ratingIndex;
                    ratingIndex++;
                }
            }
            ratingIndex = 1;
            if (sorted[0].Wagon2 is NullWagon) // two cargo trains only
            { }
            else
            {
                sorted = (from t in trains
                          orderby t.CapacityCargo2 descending
                          select t).ToList();

                for (int i = 0; i < sorted.Count; i++)
                {
                    if (i > 0 && sorted[i].CapacityCargo2 == sorted[i - 1].CapacityCargo2)
                        sorted[i].Capacity2Rating = sorted[i - 1].Capacity1Rating;
                    else
                    {
                        sorted[i].Capacity2Rating = ratingIndex;
                        ratingIndex++;
                    }
                }
            }


        }
    }


}
