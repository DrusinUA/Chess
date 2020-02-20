using System;

namespace ConsoleChess
{
    class MainClass
    {
        public const string HOST = "http://localhost:50207/api/games/";

        public static void Main(string[] args)
        {
            MainClass program = new MainClass();
            program.Start();
            Console.ReadKey();
        }

        ChessClient.ChessClient client;

        void Start ()
        {
            client = new ChessClient.ChessClient(HOST);
            Console.WriteLine(client.host);
            Console.WriteLine(client.GetCurrentGame());
            while(true)
            {
               
                Console.WriteLine("Your Move: ");
                string move = Console.ReadLine();
                if (move == "q") return;
                Console.Clear();
                Console.WriteLine(client.SendMove(move));
            }
            


        }
    }
}
