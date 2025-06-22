using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    public interface IToken
    {
        string ID { get; }
        string NameHash { get; }
        BitmapImage IconMini { get; }
    }
}
