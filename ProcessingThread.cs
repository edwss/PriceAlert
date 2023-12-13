using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlert
{
    internal class ProcessingThread
    {
        private Dictionary<string, List<Asset>> Assets = [];
        private static ConcurrentQueue<Message>? InternalMessages;
        public ProcessingThread(){
            InternalMessages = new ConcurrentQueue<Message>();
        }
        public void Start()
        {
            try
            {
                Thread thread = new Thread(new ThreadStart(Execute));
                thread.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public void Execute()
        {
            while (true)
            {
                try
                {
                    if (InternalMessages != null)
                    {
                        if (InternalMessages.TryDequeue(out var message))
                        {
                            switch (message.MessageID)
                            {
                                case Constants.addAsset:
                                    Console.WriteLine(String.Format("Asset={0} : PriceBuy={1:N} : PriceSell={2:N}", message.AssetName, message.PriceBuy, message.PriceSell));
                                    List<Asset>? AssetList = null;
                                    if (!Assets.TryGetValue(message.AssetName, out AssetList))
                                    {
                                        AssetList = new List<Asset>();
                                        AssetList.Add(new Asset(message.PriceBuy, message.PriceSell));
                                        Assets.Add(message.AssetName, AssetList);
                                    }
                                    else
                                    {
                                        AssetList.Add(new Asset(message.PriceBuy, message.PriceSell));
                                    }
                                    APIThread.PostMessage(Constants.opSubscribe, message.AssetName);
                                    break;
                                case Constants.PriceCheck:
                                    CheckPrice(message.AssetName, message.TriggerPrice);
                                    break;
                            }
                        }
                    }
                    Thread.Sleep(100);  
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }
            }
        }

        public static void PostMessage(byte MessageID, string AssetName, double PriceBuy, double PriceSell)
        {
            InternalMessages?.Enqueue(new Message(MessageID, AssetName, PriceBuy, PriceSell));
        }

        public static void PostMessage(byte MessageID, string AssetName, double Price)
        {
            InternalMessages?.Enqueue(new Message(MessageID, AssetName, Price));
        }

        public void CheckPrice(string Asset, double Price)
        {
            List<Asset>? AssetList;
            List<Asset>? AssetListToRemove = new List<Asset>();
            if (Assets.TryGetValue(Asset, out AssetList)) {
                foreach (var asset in AssetList)
                {
                    byte opResult = asset.CheckTrigger(Price);
                    switch (opResult)
                    {
                        case Constants.opBuy: EmailThread.PostMessage(Constants.opSendAlert, Asset, Price, Constants.opBuy, asset.GetPriceBuy(), asset.GetPriceSell()); break;
                        case Constants.opSell: EmailThread.PostMessage(Constants.opSendAlert, Asset, Price, Constants.opSell, asset.GetPriceBuy(), asset.GetPriceSell()); break;
                    }
                    if (opResult != 255)
                    {
                        AssetListToRemove.Add(asset);
                    }
                }
            }

            foreach (var asset in AssetListToRemove)
            {
                if (AssetList != null)
                {
                    AssetList.Remove(asset);
                }                
            }
            if ((AssetList != null) && (AssetList.Count == 0))
            {
                APIThread.PostMessage(Constants.opUnsubscribe, Asset);
            }
        }
    }
}
