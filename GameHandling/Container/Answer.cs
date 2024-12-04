using System;

namespace BolyukGame.GameHandling.Container
{
    public class Answer
    {
        public byte[] body { get; set; }
        public AnswerType type { get; set; }

        public Guid lobby_id { get; set; }
    }

    public enum AnswerType
    {
        Default=0,
        ServerFound=1,
    }
}
