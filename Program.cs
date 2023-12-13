using PriceAlert;
using System;

static class Constants
{
    public const byte addAsset = 0;

    public const byte opSell = 0;
    public const byte opBuy = 1;
}

internal class Program
{
    private static void Main(string[] args)
    {
        ProcessingThread processingThread = new ProcessingThread();
        processingThread.Start();

        while (true)
        {
            string user_input = Console.ReadLine();
            ProcessUserInput(user_input);
        }
        void ProcessUserInput(string UserInput)
        {
            string[] UserInputSplitted = UserInput.Split(' ');
            processingThread.PostAddAsset(Constants.addAsset, UserInputSplitted[0], Convert.ToDouble(UserInputSplitted[1]), Constants.opSell);
            processingThread.PostAddAsset(Constants.addAsset, UserInputSplitted[0], Convert.ToDouble(UserInputSplitted[2]), Constants.opBuy);
        }
    }
}