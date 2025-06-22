using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyCalculator
{
    public struct CargoStr
    {
        public string Name { get; private set; }
        public string IconHash { get; private set; }
        public string IconMiniHash { get; private set; }
        public string Hash { get; private set; }

        public CargoStr(string name, string hash, string iconHash, string iconMiniHash)
        {
            Name = name;
            IconHash = iconHash;
            IconMiniHash = iconMiniHash;
            Hash = hash;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
