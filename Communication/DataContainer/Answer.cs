using System;

namespace BolyukGame.Communication.DataContainer
{
    public class Answer : IContainer<AnswerType, Answer>
    {
        public Answer(Guid LobbyID) : base(LobbyID)
        {
        }
    }

    public enum AnswerType
    {
        ServerFound = 0,

        //lobby 
        PlayerInfo = 1,
        ColorPick = 2,

        //Game
        MapSend = 3,
        GameStart = 4,
        PlayerPosContainer = 5,
    }
}
