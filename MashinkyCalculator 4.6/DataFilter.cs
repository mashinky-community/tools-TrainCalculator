using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyCalculator
{
    public class DataFilter
    {
        private List<Wagon> AllWagons;
        private List<Engine> AllEngines;


        public DataFilter(List<Wagon> allWagons, List<Engine> allEngines)
        {
            AllWagons = allWagons;
            AllEngines = allEngines;

        }
        /// <summary>
        ///     Possible fuel types of all engines available in specified era
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public List<IToken> AvailableFuels(int epoch)
        {
            List<IToken> fuel1 = null;
            List<IToken> fuel2 = null;
            if (epoch == 0)
            {
                fuel1 = (from e in AllEngines
                         select e.FuelToken1).Distinct().ToList();
                fuel2 = (from e in AllEngines
                         select e.FuelToken2).Distinct().ToList();
            }
            else
            {
                fuel1 = (from e in AllEngines
                         where e.AvailableEpochs.Contains(epoch)
                         // orderby e.AvailableEpochs
                         select e.FuelToken1).Distinct().ToList();
                fuel2 = (from e in AllEngines
                         where e.AvailableEpochs.Contains(epoch)
                         // orderby e.AvailableEpochs
                         select e.FuelToken2).Distinct().ToList();
            }
            List<IToken> fuels = fuel1.Concat(fuel2).Distinct().ToList();
            fuels.Insert(0, new EmptyToken());
            fuels.RemoveAll(item => item == null);
            fuels.RemoveAll(item => item is NullToken);
            return fuels;
        }
        /// <summary>
        ///  Possible cargos of all wagons available in specified era
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public List<IMaterial> AvailableCargos(int epoch)
        {
            List<IMaterial> cargos = null;
            if (epoch == 0)
            {
                cargos = (from w in AllWagons
                          select w.Cargo).Distinct().ToList();
            }
            else
            {
                cargos = (from w in AllWagons
                          where w.AvailableEpochs.Contains(epoch)
                          select w.Cargo).Distinct().ToList();
            }
            cargos = cargos.Where(f => f.NameHash != "NA").ToList(); //filters out invalid wagons with no cargo (e.g. Sasha 872)
            /*
            List<string> tracelist = (from w in AllWagons
                                      where w.Cargo.Name == "NA"
                                      select w.Name).ToList();

            foreach (string s in tracelist)
            {
                Trace.WriteLine(s);
            } */
            cargos.Insert(0, new EmptyMaterial());
            cargos.RemoveAll(item => item == null);
            cargos.RemoveAll(item => item is NullMaterial);
            return cargos;
        }
        /// <summary>
        /// Possible cost types of all engines in specified era
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="cargo"></param>
        /// <returns></returns>
        public List<IToken> AvailableCostTokens(int epoch, IMaterial cargo)
        {
            List<IToken> cost1 = null;
            List<IToken> cost2 = null;
            if (epoch == 0)
            {
                cost1 = (from w in AllWagons
                         select w.CostToken1).Distinct().ToList();
                cost2 = (from w in AllWagons
                         select w.CostToken2).Distinct().ToList();
            }
            else
            {
                cost1 = (from w in AllWagons
                         where (w.AvailableEpochs.Contains(epoch) && w.Cargo.Equals(cargo))
                         select w.CostToken1).Distinct().ToList();
                cost2 = (from w in AllWagons
                         where (w.AvailableEpochs.Contains(epoch) && w.Cargo.Equals(cargo))
                         select w.CostToken2).Distinct().ToList();
            }
            List<IToken> cost = (cost1.Concat(cost2)).Distinct().ToList();
            cost.Insert(0, new EmptyToken());
            cost.RemoveAll(item => item == null);
            cost.RemoveAll(item => item is NullToken);
            return cost;
        }

        public List<Engine> AvailableEngines(int epoch, int speed)
        {
            List<Engine> engines;
            if (epoch == 0)
                engines = AllEngines;
            else
                engines = (from e in AllEngines
                           where e.AvailableEpochs.Contains(epoch)
                           select e).ToList();
            if (speed > 0)
                engines = (from e in engines
                           where e.MaxSpeed >= speed - Settings.SpeedTolerance && e.MaxSpeed <= speed + Settings.SpeedTolerance
                           select e).ToList();
            return engines;
        }
        public List<Engine> AvailableEngines(int epoch, int speed, IToken fuelToken)
        {
            List<Engine> engines;
            if (epoch == 0)
                engines = (from e in AllEngines
                           where ((e.FuelToken1 == fuelToken || e.FuelToken2 == fuelToken))
                           select e).ToList();
            else

                engines = (from e in AllEngines
                           where (e.AvailableEpochs.Contains(epoch) && (e.FuelToken1 == fuelToken || e.FuelToken2 == fuelToken))
                           select e).ToList();
            if (speed > 0)
                engines = (from e in engines
                           where e.MaxSpeed >= speed - Settings.SpeedTolerance && e.MaxSpeed <= speed + Settings.SpeedTolerance
                           select e).ToList();

            return engines;
        }

        public List<Wagon> AvailableWagons(int epoch, IMaterial cargo)
        {
            List<Wagon> wagons;
            if (epoch == 0)
                wagons = (from w in AllWagons
                          where w.Cargo.Equals(cargo)
                          select w).ToList();
            else
                wagons = (from w in AllWagons
                          where (w.AvailableEpochs.Contains(epoch) && w.Cargo.Equals(cargo))
                          select w).ToList();
            return wagons;
        }

        public List<Wagon> AvailableWagons(int epoch, IMaterial cargo, IToken costToken)
        {
            List<Wagon> wagons;
            if (epoch == 0)
                wagons = (from w in AllWagons
                          where (w.Cargo.Equals(cargo) && (w.CostToken1 == costToken || w.CostToken2 == costToken))
                          select w).ToList();
            else
                wagons = (from w in AllWagons
                          where (w.AvailableEpochs.Contains(epoch) && w.Cargo.Equals(cargo) && (w.CostToken1 == costToken || w.CostToken2 == costToken))
                          select w).ToList();
            return wagons;
        }


    }
}
