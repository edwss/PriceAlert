using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlert
{
    internal class Message
    {
        public double PriceBuy;
        public double PriceSell;
        public string AssetName;
        public byte OperationType;
        public byte MessageID;
        public double TriggerPrice;
       public Message(byte MessageID, string AssetName, double PriceBuy, double PriceSell) 
       {
            this.MessageID = MessageID;
            this.AssetName = AssetName;
            this.PriceBuy = PriceBuy;
            this.PriceSell = PriceSell;
       }
        public Message(byte MessageID, string AssetName)
        {
            this.MessageID = MessageID;
            this.AssetName = AssetName;
        }
        public Message(byte MessageID, string AssetName, double TriggerPrice, byte OperationType)
        {
            this.MessageID = MessageID;
            this.AssetName= AssetName;
            this.TriggerPrice = TriggerPrice;
            this.OperationType = OperationType;
        }

        public Message(byte MessageID, string AssetName, double TriggerPrice, byte OperationType, double PriceBuy, double PriceSell)
        {
            this.MessageID = MessageID;
            this.AssetName = AssetName;
            this.TriggerPrice = TriggerPrice;
            this.OperationType = OperationType;
            this.PriceBuy= PriceBuy;
            this.PriceSell= PriceSell;
        }

        public Message(byte MessageID, string AssetName, double TriggerPrice)
        {
            this.AssetName = AssetName;
            this.MessageID = MessageID;
            this.TriggerPrice = TriggerPrice;
        }
    }
}
