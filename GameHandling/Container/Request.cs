using System;
using System.Windows.Input;

namespace BolyukGame.GameHandling
{
    public class Request
    {
       //public Key[] keys { get; set; }
        public byte[] body { get; set; }
        public RequestType type { get; set; }

        public Guid lobby_id { get; set; }
    }

    public enum RequestType
    {
        Leave = 0,
        Join = 1,
        Input = 2,
        Config = 3,
        ServerSearch = 4,
    }
}
