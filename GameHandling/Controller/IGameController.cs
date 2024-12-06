using BolyukGame.GameHandling.Container;
using BolyukGame.Shared.Info;
using Fleck;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketSharp;

namespace BolyukGame.GameHandling
{
    public abstract class IGameController
    {
        public abstract void StartSession(string ip);

        public abstract void StopSession();

        public abstract void SendQuery(Request update);

        public abstract void SetListener(IGameListener listener);

    }

    public interface IGameListener
    {
        public void AcceptQuery(Answer update);
    }

    public interface IPlayerGameListener : IGameListener
    {
        public void OnSessionEnds();

        public void OnSessionStarts();

        public void OnError(ErrorEventArgs args);
        
    }

    public interface IServerGameListener : IGameListener
    {
        public Answer QueryWork(Request update);

        public void OnPlayerLeave(PlayerContainer palyer);

        public void OnPlayerReqistered(PlayerContainer palyer);
    }
}
