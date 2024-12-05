using System;

namespace BolyukGame.GameHandling.Container
{
    public class Answer
    {
        public byte[] Body { get; set; }
        public AnswerType Type { get; set; }

        public Guid LobbyId { get; set; }
    }

    public enum AnswerType
    {
        Default=0,
        ServerFound=1,
        LobbyConfig=2,
        Kicked=3,
        PlayerInfo = 4,
    }
}
