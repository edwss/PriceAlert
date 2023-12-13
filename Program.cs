using PriceAlert;
using System;

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

        if (args.Length != 0)
        {
            ProcessUserInput(args);
        }

        while (true)
        {
            try
            {
                string? user_input = Console.ReadLine();
                if (user_input != null)
                {
                    string[] UserInputSplitted = user_input.Split(' ');
                    ProcessUserInput(UserInputSplitted);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        void ProcessUserInput(string[] args)
        {
            if (args.Length > 3)
            {
                ProcessingThread.PostMessage(Constants.addAsset, args[1], Convert.ToDouble(args[2]), Convert.ToDouble(args[3]));
            }
            else
            {
                ProcessingThread.PostMessage(Constants.addAsset, args[0], Convert.ToDouble(args[1]), Convert.ToDouble(args[2]));
            }
        }
    }
}