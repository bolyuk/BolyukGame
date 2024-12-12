using System;
using System.Windows.Input;

namespace BolyukGame.GameHandling
{
    public class Request
    {
        public byte[] Body { get; set; }
        public RequestType Type { get; set; }

        public Guid LobbyId { get; set; }
    }

    public enum RequestType
    {
        ServerSearch = 0,

        //lobby
        Join = 1,
        ColorSelect = 2,

        //Game
        Ready = 3,
        MapGet = 4,
        PosChanged = 5,
    }
}
