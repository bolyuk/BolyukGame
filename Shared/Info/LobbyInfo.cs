using System;

namespace BolyukGame.Shared.Info
{
    public class LobbyInfo
    {
        public int players { get; set; }

        public string name { get; set; }
        public string ip { get; set; }
        public Guid id { get; set; } = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            if (obj is LobbyInfo other)
            {
                return players == other.players &&
                       name == other.name &&
                       ip == other.ip;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(players, name, ip);
        }
    }
}
