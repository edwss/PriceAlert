using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlert
{
    internal class Asset
    {
        private double PriceBuy = -1;
        private double PriceSell = -1;
        public Asset(double PriceBuy, double PriceSell)
        {
            this.PriceBuy = PriceBuy;
            this.PriceSell = PriceSell;
        }
    }
}
