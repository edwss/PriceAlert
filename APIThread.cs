using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Timers;

namespace PriceAlert
{
    internal class APIThread
    {
        private System.Timers.Timer aTimer;
        private static ConcurrentQueue<Message>? InternalMessages;
        private List<String>? Assets;
        private const string URL = "https://brapi.dev/api/quote/";
        public APIThread()
        {
            InternalMessages = new ConcurrentQueue<Message>();
            Assets = new List<String>();
            aTimer = new System.Timers.Timer(60000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
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
                                Console.WriteLine(String.Format("Subscribe : AssetName={0}", message.AssetName));
                                if ((Assets != null) && (!Assets.Contains(message.AssetName))) {
                                    Assets.Add(message.AssetName);
                                }                         
                                break;
                            case Constants.opUnsubscribe:
                                Console.WriteLine(String.Format("Unsubscribe : AssetName={0}", message.AssetName));
                                if ((Assets != null) && (Assets.Contains(message.AssetName)))
                                {
                                    Assets.Remove(message.AssetName);
                                }
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

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                if (Assets != null)
                {
                    foreach (string asset in Assets)
                    {
                        Console.WriteLine(String.Format("Checking Price : Asset={0}", asset));
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(URL);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string response = client.GetStringAsync(String.Format("{0}?token={1}", asset, ConfigurationManager.AppSettings.Get("API-Token"))).Result;
                        JObject json = JObject.Parse(response);
                        if (json != null)
                        {
                            string signal = Convert.ToString(json["results"][0]["regularMarketTime"]);
                            double price = Convert.ToDouble(json["results"][0]["regularMarketPrice"]);
                            Console.WriteLine(String.Format("Signal={0} : Price={1}", signal, price));
                            ProcessingThread.PostMessage(Constants.PriceCheck, asset, price);
                        }
                        client.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
