using System;

namespace ConsoleChess
{
    class MainClass
    {
        public const string HOST = "";

        public static void Main(string[] args)
        {
            MainClass program = new MainClass();
            program.Start();
        }

        ChessClient.ChessClient client;

        void Start ()
        {
            client = new ChessClient.ChessClient(HOST);
            Console.WriteLine(client.host);
            Console.WriteLine(client.GetCurrentGame());
        }
    }
}
