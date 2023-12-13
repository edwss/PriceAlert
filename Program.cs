using PriceAlert;
using System;

static class Constants
{
    // ProcessingThread
    public const byte addAsset = 0;
    public const byte PriceCheck = 1;
    // APIThread
    public const byte opSubscribe = 0;
    // EmailThread
    public const byte opSendAlert = 0;
    // EmailThread Operations
    public const byte opBuy = 0;
    public const byte opSell = 1;
}

internal class Program
{
    private static void Main(string[] args)
    {
        APIThread apiThread = new APIThread();
        apiThread.Start();  

        ProcessingThread processingThread = new ProcessingThread();
        processingThread.Start();

        EmailThread emailThread = new EmailThread();
        emailThread.Start();

        while (true)
        {
            string? user_input = Console.ReadLine();
            if (user_input != null)
            {
                ProcessUserInput(user_input);
            }
        }
        void ProcessUserInput(string UserInput)
        {
            string[] UserInputSplitted = UserInput.Split(' ');
            ProcessingThread.PostMessage(Constants.addAsset, UserInputSplitted[0], Convert.ToDouble(UserInputSplitted[1]), Convert.ToDouble(UserInputSplitted[2]));
        }
    }
}