using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlert
{
    static class Constants
    {
        // ProcessingThread
        public const byte addAsset = 0;
        public const byte PriceCheck = 1;
        // APIThread
        public const byte opSubscribe = 0;
        public const byte opUnsubscribe = 1;
        // EmailThread
        public const byte opSendAlert = 0;
        // EmailThread Operations
        public const byte opBuy = 0;
        public const byte opSell = 1;

        public static string ConvertOperationToString(int opType)
        {
            switch (opType)
            {
                case Constants.opBuy:
                    return "Buy";
                case Constants.opSell:
                    return "Sell";
            }
            return string.Empty;
        }
    }
}
