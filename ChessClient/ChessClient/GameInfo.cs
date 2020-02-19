using System;
using System.Collections.Specialized;

namespace ChessClient
{
    public struct GameInfo
    {
        public string GameID;
        public string FEN;
        public string Status;

        public GameInfo (NameValueCollection list)
        {
            GameID = list["ID"];
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
