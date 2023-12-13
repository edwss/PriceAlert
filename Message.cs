using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlert
{
    internal class Message
    {
        public double Price;
        public string AssetName;
        public byte OperationType;
        public byte MessageID;
       public Message(byte MessageID, string AssetName, double Price, byte OperationType) 
       {
            this.MessageID = MessageID;
            this.AssetName = AssetName;
            this.Price = Price;
            this.OperationType = OperationType;
       }
    }
}
