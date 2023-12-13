using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlert
{
    internal class ProcessingThread
    {
        private Dictionary<string, Asset> Assets = [];
        private ConcurrentQueue<Message> Messages;
        public ProcessingThread(){
            this.Messages = new ConcurrentQueue<Message>();
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
                    if (Messages.TryDequeue(out var message))
                    {
                        switch (message.MessageID)
                        {
                            case Constants.addAsset: 
                                Console.WriteLine(String.Format("Asset={0} : Price={1:N} : Operation={2:D}", message.AssetName, message.Price, message.OperationType));
                                break;
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

        public void PostAddAsset(byte MessageID, string AssetName, double Price, byte OperationType)
        {
            Messages.Enqueue(new Message(MessageID, AssetName, Price, OperationType));
        }
    }
}
