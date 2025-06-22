using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyCalculator
{
    public class Calculator
    {
        private UserDataContext userData;
        private DataFilter filter;
        private List<Engine> availableEngines;
        private List<Wagon> availableWagons1;
        private List<Wagon> availableWagons2;
        private Evaluator evaluator;

        public Calculator(UserDataContext userData, DataFilter dataFilter)
        {
            this.userData = userData;
            filter = dataFilter;
            GetEngines();
            GetWagons();
            evaluator = new Evaluator();
        }



        public void GetEngines()
        {
            if (userData.FuelFilter is null || userData.FuelFilter is EmptyToken) // check if user selected any fuel filter
                availableEngines = filter.AvailableEngines(userData.SelectedEpoch, userData.Speed);
            else
                availableEngines = filter.AvailableEngines(userData.SelectedEpoch, userData.Speed, userData.FuelFilter);
        }

        public void GetWagons()
        {
            if (userData.Cargo1CostFilter is null || userData.Cargo1CostFilter is EmptyToken)
                availableWagons1 = filter.AvailableWagons(userData.SelectedEpoch, userData.Cargo1TypeFilter);
            else
                availableWagons1 = filter.AvailableWagons(userData.SelectedEpoch, userData.Cargo1TypeFilter, userData.Cargo1CostFilter);

 
                if (userData.Cargo2CostFilter is null || userData.Cargo2CostFilter is EmptyToken)
                    availableWagons2 = filter.AvailableWagons(userData.SelectedEpoch, userData.Cargo2TypeFilter);
                else
                    availableWagons2 = filter.AvailableWagons(userData.SelectedEpoch, userData.Cargo2TypeFilter, userData.Cargo2CostFilter);
            
        }

        public List<CalculatedTrain> CalculateTrains()
        {
            GetEngines();
            GetWagons();
            List<CalculatedTrain> trains = null;
            if (userData.Cargo2TypeFilter is null || userData.Cargo2TypeFilter is EmptyMaterial)
                trains = CSingleCargoTrains();
            else
                trains = CDoubleCargoTrains();

            return trains;
        }

        public List<CalculatedTrain> CSingleCargoTrains()
        {
            List<CalculatedTrain> trains = new List<CalculatedTrain>();
            foreach (Engine engine in availableEngines)
            {

                foreach (Wagon wagon in availableWagons1)
                {
                    int engineCount = 0;
                    double trainLength = 0;
                    int wagonCount = 0;
                    do
                    {
                        engineCount++;
                        wagonCount = ((engine.RealPower * engineCount) - engine.WeightFull * engineCount) / wagon.WeightFull; // add wagons
                        trainLength = TrainLength(engineCount, engine, wagonCount, wagon);

                        while (trainLength > userData.MaxTrainLength) // remove extra wagons when train length limit is crossed
                        {
                            wagonCount--;
                            trainLength = TrainLength(engineCount, engine, wagonCount, wagon);
                        }
                    } while (trainLength + engine.Length + (wagon.Length * 2) < userData.MaxTrainLength); // add engines until engine + specified amount of wagons fits to traing length max limit

                   // if (engine.Name.Contains("Bardotka")) debug
                    {

                    }
                    int costEvaluation = (evaluator.EvaluateCost(engine) * engineCount) + (evaluator.EvaluateCost(wagon) * wagonCount);
                    int fuelEvaluation = evaluator.EvaluateFuel(engine) * engineCount;
                    int capacity = 0;
                    if (engine.Cargo.Equals(wagon.Cargo))
                        capacity = (engine.Capacity * engineCount) + (wagon.Capacity * wagonCount);
                    else
                        capacity = wagon.Capacity * wagonCount;
                    trains.Add(new CalculatedTrain(costEvaluation, fuelEvaluation, engine, engineCount, wagon, wagonCount, new NullWagon(), 0, capacity, wagon.Cargo, 0, new EmptyMaterial()));

                }

            }
            if (trains.Count > 0)
            evaluator.GiveRatings(trains);

            return trains;
        }

        public List<CalculatedTrain> CDoubleCargoTrains()
        {
            List<CalculatedTrain> trains = new List<CalculatedTrain>();
            foreach (Engine engine in availableEngines)
            {
                if ( engine.Name.Contains("GS"))
                    {

                }
                foreach (Wagon wagon1 in availableWagons1)
                {
                    {
                        foreach (Wagon wagon2 in availableWagons2)
                        {
                            double trainLength;
                            double trainWeight = 0;
                            int engineCount = 0;
                            int wagon1Count = 0;
                            int wagon2Count = 0;
                            int cargo1Capacity = 0;
                            int cargo2Capacity = 0;
                            int lastAdd = 0;
                            do
                            {
                                
                                engineCount++;
                                trainLength = TrainLength(engineCount, engine, wagon1Count, wagon1, wagon2Count, wagon2);
                                while (trainLength < userData.MaxTrainLength && trainWeight < engine.RealPower * engineCount) // loop for adding wagons
                                {
                                    if (cargo1Capacity * userData.PriorityCargo2 <= cargo2Capacity * userData.PriorityCargo1) // checking which type of cargo to add based on user specified priority
                                    {
                                        wagon1Count++;
                                        cargo1Capacity = wagon1.Capacity * wagon1Count;
                                        lastAdd = 1;
                                    }
                                    else
                                    {
                                        wagon2Count++;
                                        cargo2Capacity = wagon2.Capacity * wagon2Count;
                                        lastAdd = 2;
                                    }
                                    trainLength = TrainLength(engineCount, engine, wagon1Count, wagon1, wagon2Count, wagon2);
                                    trainWeight = TrainWeight(engineCount, engine, wagon1Count, wagon1, wagon2Count, wagon2);
                                }
                            } while (trainLength < userData.MaxTrainLength && CheckEngineAdd(engine, trainLength, wagon1, wagon2) );
                            // remove last added wagon if limit is crossed
                            if (trainLength > userData.MaxTrainLength || trainWeight > engine.RealPower * engineCount && lastAdd == 1) // previous loop always adds last wagon over limits, remove it
                                wagon1Count--;
                            else if (trainLength > userData.MaxTrainLength || trainWeight > engine.RealPower * engineCount && lastAdd == 2)
                                wagon2Count--;

                            int costEvaluation = (evaluator.EvaluateCost(engine) * engineCount) + (evaluator.EvaluateCost(wagon1) * wagon1Count) + evaluator.EvaluateCost(wagon2) * wagon2Count;
                            int fuelEvaluation = evaluator.EvaluateFuel(engine) * engineCount;
                            int capacity1 = 0;
                            if (engine.Cargo.Equals(wagon1.Cargo))
                                capacity1 = (engine.Capacity * engineCount) + (wagon1.Capacity * wagon1Count);
                            else
                                capacity1 = wagon1.Capacity * wagon1Count;
                            int capacity2 = 0;
                            if (engine.Cargo.Equals(wagon2.Cargo))
                                capacity2 = (engine.Capacity * engineCount) + (wagon2.Capacity * wagon2Count);
                            else
                                capacity2 = wagon2.Capacity * wagon2Count;
                            trains.Add(new CalculatedTrain(costEvaluation, fuelEvaluation, engine, engineCount, wagon1, wagon1Count, wagon2, wagon2Count, capacity1, wagon1.Cargo, capacity2, wagon2.Cargo));
                        }


                    } //while (trainLength + engine.Length + (wagon1.Length * 2) < userData.MaxTrainLength); // add engines until engine + specified amount of wagons fits to traing length max limit
                }

            }
            if (trains.Count > 0)
                evaluator.GiveRatings(trains);
            return trains;
        }

        public double TrainLength(int engineCount, Engine engine, int wagonCount, Wagon wagon)
        {
            double length = (engine.Length * engineCount) + (wagon.Length * wagonCount);
            return length;
        }

        public double TrainLength(int engineCount, Engine engine, int wagon1Count, Wagon wagon1, int wagon2Count, Wagon wagon2)
        {
            double length = (engine.Length * engineCount) + (wagon1.Length * wagon1Count) + (wagon2.Length * wagon2Count);
            return length;
        }

        public double TrainWeight(int engineCount, Engine engine, int wagonCount, Wagon wagon)
        {
            double weight = (engine.WeightFull * engineCount) + (wagon.WeightFull * wagonCount);
            return weight;
        }

        public double TrainWeight(int engineCount, Engine engine, int wagon1Count, Wagon wagon1, int wagon2Count, Wagon wagon2)
        {
            double weight = (engine.WeightFull * engineCount) + (wagon1.WeightFull * wagon1Count) + (wagon2.WeightFull * wagon2Count);
            return weight;
        }

        public bool CheckEngineAdd(Engine engine, double trainLength, Wagon wagon1, Wagon wagon2)
        {
            double remainingLength = userData.MaxTrainLength - trainLength;
            int weight1 = Convert.ToInt32(remainingLength / wagon1.Length * userData.PriorityCargo1 / 100* wagon1.WeightFull);
            int weight2 = Convert.ToInt32( remainingLength / wagon2.Length * userData.PriorityCargo2 / 100 * wagon2.WeightFull);

            return weight1 + weight2 > engine.RealPower * 0.4;
        }
    }
}
