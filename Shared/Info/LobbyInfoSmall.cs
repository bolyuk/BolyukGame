using System;
using System.Collections.Generic;

namespace BolyukGame.Shared.Info
{
    public class LobbyInfoSmall
    {
        public int PlayersCount { get; set; }

        public string Name { get; set; }
        public string Ip { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            if (obj is LobbyInfoSmall other)
            {
                return PlayersCount == other.PlayersCount &&
                       Name == other.Name &&
                       Ip == other.Ip;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayersCount, Name, Ip);
        }
    }
}
