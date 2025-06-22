using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    public class Wagon : IWagon
    {
        public string ID { get; private set; }
        public int CostAmount1 { get; private set; }
        public IToken CostToken1 { get; private set; }
        public int CostAmount2 { get; private set; }        
        public IToken CostToken2 { get; private set; }
        public int TrackType { get; private set; } // 0 - normal track, 2 - electric track
        public int Capacity { get; private set; }
        public string Name { get; private set; }
        public IMaterial Cargo { get; private set; }
        public int WeightFull { get; private set; }
        public int[] AvailableEpochs { get; private set; }
        public double Length { get; private set; }
        public BitmapImage Icon { get; private set; }

        public Wagon()
        {

        }
        public Wagon(BitmapImage iconImage, double length, string iD, int costAmount1, IToken costToken1, int costAmount2, IToken costToken2, int trackType, int capacity, string name, IMaterial cargo, int weightFull, int startEpoch, int endEpoch)
        {
            ID = iD;
            CostAmount1 = costAmount1;
            CostToken1 = costToken1;
            CostAmount2 = costAmount2;
            CostToken2 = costToken2;
            TrackType = trackType;
            Capacity = capacity;
            Name = name;
            Cargo = cargo;
            WeightFull = weightFull;
            AvailableEpochs = CountEpochs(startEpoch, endEpoch);
            Length = length;
            Icon = iconImage;
        }

        public int[] CountEpochs(int start, int end)
        {
            int[] range = new int[end - start + 1];
            for (int i = 0; i < range.Length; i++)
            {
                range[i] = start + i;
            }
            return range;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
