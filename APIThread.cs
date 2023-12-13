using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlert
{
    internal class APIThread
    {
        private static ConcurrentQueue<Message>? InternalMessages;
        private List<String> Assets;
        public APIThread()
        {
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
                if (InternalMessages != null)
                {
                    if (InternalMessages.TryDequeue(out var message))
                    {
                        switch (message.MessageID)
                        {
                            case Constants.opSubscribe:
                                Console.WriteLine(String.Format("AssetName={0}", message.AssetName));
                                break;
                        }
                    }
                }
                            
                Thread.Sleep(100);
            }
        }

        public static void PostMessage(byte MessageID, string AssetName)
        {
            InternalMessages?.Enqueue(new Message(MessageID, AssetName));
        }
    }
}
