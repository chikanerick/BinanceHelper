using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance
{
   public class Cryptocurrency
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public double Price { get; set; }
        public double PercentChange24h { get; set; }

    }
}
