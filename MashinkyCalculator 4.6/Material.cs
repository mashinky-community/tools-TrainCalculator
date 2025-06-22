using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    public class Material : IMaterial
    {
        public string ID { get; private set; }

        public string NameHash { get; private set; }
        public BitmapImage IconMini { get; private set; }

        public Material(string id, string nameHash, BitmapImage iconMini)
        {
            ID = id;
            NameHash = nameHash;
            IconMini = iconMini;
        }
    }

    
}
