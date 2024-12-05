using System;

namespace BolyukGame.Shared.Info
{
    public class LobbyInfo
    {
        public int Players { get; set; }

        public string Name { get; set; }
        public string Ip { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            if (obj is LobbyInfo other)
            {
                return Players == other.Players &&
                       Name == other.Name &&
                       Ip == other.Ip;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Players, Name, Ip);
        }
    }
}
