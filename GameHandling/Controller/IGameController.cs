using BolyukGame.GameHandling.Container;

namespace BolyukGame.GameHandling
{
    public abstract class IGameController
    {
        protected WebSocketHandler handler;
        protected GameListener listener;

        public virtual void TryStartSessionAsync(string ip)
        { }

        public async void StopSession()
        {
            await handler.CloseConnectionAsync();
        }

        public virtual void SendQuery(Request update)
        {
            AcceptQuery(QueryWork(update));
        }

        public virtual void AcceptQuery(Answer update)
        {

        }

        public virtual Answer QueryWork(Request update)
        {
            return null;
        }

        public void SetListener(GameListener listener)
        {
            this.listener = listener;
        }

    }

    public interface GameListener
    {
        public virtual void acceptQuery(Answer update)
        {

        }

        public virtual Answer queryWork(Request update)
        {
            return null;
        }
    }
}
