using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlert
{
    internal class Asset
    {
        private string AsssetName = string.Empty;
        private double PriceBuy = -1;
        private double PriceSell = -1;
        public Asset(string AssetName)
        {
            this.AsssetName = AssetName;
        }
    }
}
