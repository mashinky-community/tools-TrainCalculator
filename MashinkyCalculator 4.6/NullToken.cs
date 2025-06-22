using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    public class NullToken : IToken
    {
        public string NameHash { get; } = "<None>";

        public string ID { get; } = "";
        public BitmapImage IconMini { get; private set; }

        public NullToken(BitmapImage iconMini)
        {
            IconMini = iconMini;
        }
    }
}
