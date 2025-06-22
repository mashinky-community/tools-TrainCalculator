using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    public class EmptyToken : IToken
    {
        public string ID { get; } = "";

        public string NameHash { get; } = "";

        public BitmapImage IconMini { get; }

        public EmptyToken()
        {

        }
    }
}
