using System;
using System.Collections.Specialized;

namespace ChessClient
{
    public struct GameInfo
    {
        public int GameID;
        public string FEN;
        public string Status;

        public GameInfo (NameValueCollection list)
        {
            GameID = int.Parse(list["GameID"]);
            FEN = list["FEN"];
            Status = list["Status"];
        }


        override public string ToString()
        {
            return
                "GameID = " + GameID +
                "\nFEN = " + FEN +
                "\nStatus = " + Status;
        }
    }
}
