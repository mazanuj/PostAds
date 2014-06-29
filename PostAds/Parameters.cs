using System.Collections.Generic;

namespace Motorcycle
{
    static class Parameters
    {
        internal static string GetManufacture(string site, string parameter)
        {
            var motosale = new Dictionary<string, string>
            {
                {"honda", "30"},
                {"Yamaha", "29"},
                {"Suzuki", "28"},
                {"Kawasaki", "27"},
                {"Husqvarna", "39"}
            };

            return site == "motosale" ? motosale[parameter] : string.Empty;
        }        
    }
}