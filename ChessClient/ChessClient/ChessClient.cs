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

        public ChessClient (string host)   //public ChessClient(string host, string user)
        {
            this.host = host;
           // this.user = user;

        }

        // public void GetCurrentGame()
        public GameInfo GetCurrentGame()
        {
             return new GameInfo(ParseGame(CallServer()));
            //Console.WriteLine(CallServer());
            
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
