using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    public class NullWagon : IWagon
    {
        public string IconTexturePath { get; private set; } = "";
        public string ID { get; private set; } = "";
        public string IconHash { get; private set; } = "";
        public string IconColor { get; private set; } = "";
        public int CostAmount1 { get; private set; } = 0;
        public IToken CostToken1 { get; private set; }
        public int CostAmount2 { get; private set; } = 0;
        public IToken CostToken2 { get; private set; }
        public int TrackType { get; private set; } = 99; // 0 - normal track, 2 - electric track
        public int Capacity { get; private set; } = 0;
        public string Name { get; private set; } = "NullWagon";
        public IMaterial Cargo { get; private set; }
        public int WeightFull { get; private set; } = 0;
        public int[] AvailableEpochs { get; private set; } = new int[2] { 98, 99 };
        public double Length { get; private set; } = 0;
        public BitmapImage Icon { get; private set; }
    }
}
