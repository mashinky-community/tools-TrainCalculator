using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    public class Token : IToken
    {
        public string ID { get; }
        public string NameHash { get; private set; }
        public BitmapImage IconMini { get; private set; }

        public Token(string id, string nameHash, BitmapImage iconMini)
        {
            ID = id;
            NameHash = nameHash;
            IconMini = iconMini;
        }
    }
}
