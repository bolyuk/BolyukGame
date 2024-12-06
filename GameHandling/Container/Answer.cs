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
        ServerFound = 0,
        //lobby 
        PlayerInfo = 1,
        ColorPick = 2,
        //Game
        GameStart = 3,
        EntityPosContainer = 4,
    }
}
