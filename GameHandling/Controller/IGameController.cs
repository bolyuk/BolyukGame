using BolyukGame.GameHandling.Container;

namespace BolyukGame.GameHandling
{
    public abstract class IGameController
    {
        protected WebSocketHandler handler;

        public abstract void tryStartSessionAsync();

        public async void stopSession()
        {
            await handler.CloseConnectionAsync();
        }

        public virtual void sendQuery(Request update)
        {
            acceptQuery(queryWork(update));
        }

        public virtual void acceptQuery(Answer update)
        {

        }

        public virtual Answer queryWork(Request update)
        {
            return null;
        }       

    }
}
