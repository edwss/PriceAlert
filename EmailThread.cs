using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace PriceAlert
{
    internal class EmailThread
    {
        private static ConcurrentQueue<Message>? InternalMessages;
        public EmailThread()
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
                        try
                        {
                            switch (message.MessageID)
                            {
                                case Constants.opSendAlert:
                                    string? report = ConfigurationManager.AppSettings.Get("Report");
                                    string? from = ConfigurationManager.AppSettings.Get("From");
                                    string body = string.Empty;
                                    if ((report != null) && (from != null))
                                    {
                                        body = String.Format("{0} atigiu o valor {1} - {2}", message.AssetName, message.TriggerPrice, Constants.ConvertOperationToString(message.OperationType));
                                        var smtpClient = new SmtpClient(ConfigurationManager.AppSettings.Get("STMP-Address"))
                                        {
                                            Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("STMP-Port")),
                                            Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("STMP-Username"), ConfigurationManager.AppSettings.Get("STMP-Password")),
                                            EnableSsl = true,
                                        };

                                        smtpClient.Send(from, report, "PriceAlert", body);
                                    }

                                    Console.WriteLine(string.Format("Alert : AssetName={0} : DesiredPrice={1} : Price={2} : Type={3} : Email={4}",
                                                      message.AssetName, message.PriceBuy, message.TriggerPrice, Constants.ConvertOperationToString(message.OperationType), report)
                                    );
                                    break;
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.ToString());
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }

        public static void PostMessage(byte MessageID, string AssetName, double TriggerPrice, byte OperationType, double PriceBuy, double PriceSell)
        {
            InternalMessages?.Enqueue(new Message(MessageID, AssetName, TriggerPrice, OperationType, PriceBuy, PriceSell));
        }
    }
}
