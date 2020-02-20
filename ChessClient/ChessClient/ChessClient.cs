using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ChessClient
{
    public class ChessClient
    {
        public string host { get; private set; }
        // public string user { get; private set; }
        int CurrentGameID;
        public ChessClient (string host)   //public ChessClient(string host, string user)
        {
            this.host = host;
           // this.user = user;

        }

        // public void GetCurrentGame()
        public GameInfo GetCurrentGame()
        {

            GameInfo game = new GameInfo(ParseGame(CallServer()));
            CurrentGameID = game.GameID;
            return game;
            //Console.WriteLine(CallServer());
            
        }
        public GameInfo SendMove (string move)
        {
            
            string json = CallServer(CurrentGameID + "/" + move);
            var list = ParseGame(json);
            GameInfo game = new GameInfo(list);
            return game;

        }
        private string CallServer (string param = "")
        {
            WebRequest request = WebRequest.Create(host + "/" + param);

            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
        private NameValueCollection ParseGame(string json)
        {
            NameValueCollection list = new NameValueCollection();

            string pattern = @"""(\w+)"":""?([^,""""}]*)"; 
            foreach (Match m in Regex.Matches(json, pattern)) 
                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;
              
                return list;
            
           
        }
    }
}
