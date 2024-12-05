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
        Leave = 0,
        Join = 1,
        Input = 2,
        Config = 3,
        ServerSearch = 4,
        PlayerInfo = 5,
    }
}
