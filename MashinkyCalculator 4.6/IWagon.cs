using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    public interface IWagon
    {
        string ID { get;}
        int CostAmount1 { get;}
        IToken CostToken1 { get;}
        int CostAmount2 { get;}
        IToken CostToken2 { get; }
        int TrackType { get; } // 0 - normal track, 2 - electric track
        int Capacity { get;}
        string Name { get; }
        IMaterial Cargo { get;  }
        int WeightFull { get; }
        int[] AvailableEpochs { get;  }
        double Length { get; }
        BitmapImage Icon { get; }
    }
}
