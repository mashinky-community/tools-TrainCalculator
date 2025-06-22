/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyCalculator
{
    /// <summary>
    /// contains Token objects used by all aplicationn, does't create new instances, use reference to here
    /// </summary>
    public class HashConvertor
    {
        public MoneyToken Money { get; private set; }
        public PlankToken Planks { get; private set; }
        public CoalToken Coal { get; private set; }
        public DieselToken Diesel { get; private set; }
        public SteelToken Steel { get; private set; }
        public ElectricityToken Electricity { get; private set; }
        public IronToken Iron { get; private set; }

        public IToken token { get; private set; }
        public IMaterial material { get; private set; }

        public HashConvertor()
        {
            Money = new MoneyToken();
            Planks = new PlankToken();
            Coal = new CoalToken();
            Diesel = new DieselToken();
            Steel = new SteelToken();
            Electricity = new ElectricityToken();
            Iron = new IronToken();

        }
        /// <summary>
        /// Converts token hash to Token object
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public Token ConvertToToken(string hash)
        {
            Token token = null;

            switch (hash)
            {
                case "F0000000":
                    token = Money;
                    break;

                case "F07C5273":
                    token = Planks;
                    break;

                case "F1283C03":
                    token = Coal; ;
                    break;
                case "F27DB683":
                    token = Iron;
                    break;
                case "F29BF6C1":
                    token = Diesel;
                    break;
                case "F8E3D3EC":
                    token = Steel;
                    break;
                case "F9D8BF64":
                    token = Electricity;
                    break;

                default:
                    break;
            }
            return token;
        }


        /// <summary>
        /// Converts cargo hash to cargo structures
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public CargoStr ConvertToCargo(string hash)
        {
            CargoStr cargo = new CargoStr("NA", hash, "NA", "NA");
            switch (hash)
            {
                case "0BA458C8":
                    cargo = new CargoStr("passanger", hash, "1BE1510B", "2BE1510B");
                    break;
                case "0F822763":
                    cargo = new CargoStr("mail", hash, "21D57F07", "31D57F07");
                        break;
                case "19CFBDA7":
                    cargo = new CargoStr("oil", hash, "733C5A02", "833C5A02");
                    break;
                case "3199AA74":
                    cargo = new CargoStr("logs", hash, "9B91E911", "AB91E911");
                    break;
                case "448DDF23":
                    cargo = new CargoStr("planks", hash, "EDF8C41C", "FDF8C41C");
                    break;
                case "53F1B093":
                    cargo = new CargoStr("diesel", hash, "8EBAAE5A", "9EBAAE5A");
                    break;
                case "61A13BCE":
                    cargo = new CargoStr("iron ore", hash, "6981426F", "7981426F");
                    break;
                case "6ACBCBA9":
                    cargo = new CargoStr("coal", hash, "8DE905C6", "9DE905C6");
                        break;
                case "762F8F3E":
                    cargo = new CargoStr("cobblestone", hash, "517647C6", "617647C6");
                    break;
                case "7C13D1C9":
                    cargo = new CargoStr("iron", hash, "3D29FC8E", "4D29FC8E");
                    break;
                case "88D1A491":
                    cargo = new CargoStr("steel", hash, "97B64A50", "A7B64A50");
                    break;
                case "8DFA75B5":
                    cargo = new CargoStr("cement", hash, "0CF5769F", "1CF5769F");
                    break;
                case "94032E35":
                    cargo = new CargoStr("sand", hash, "B7715F1A", "C7715F1A");
                    break;
                case "C74198A7":
                    cargo = new CargoStr("food", hash, "BD9A9260", "CD9A9260");
                    break;
                case "B388ED8C":
                    cargo = new CargoStr("goods", hash, "C11D5E48", "D11D5E48");
                    break;
                default:
                    break;
            }
            return cargo;
        }
    }
}
 */