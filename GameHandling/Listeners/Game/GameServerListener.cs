using BolyukGame.Communication.Controller;
using BolyukGame.Communication.DataContainer;
using BolyukGame.Menu;
using BolyukGame.Shared.Info;

namespace BolyukGame.GameHandling.Listeners.Game
{
    public class GameServerListener : IServerGameListener
    {
        public GameMenu Menu { get; set; }

        public void AcceptQuery(Answer update)
        {
            Menu.AcceptQuery(update);
        }

        public void OnPlayerLeave(PlayerContainer palyer)
        {
            Menu.OnPlayerLeave(palyer);
        }

        public void OnPlayerReqistered(PlayerContainer palyer)
        {
            throw new System.NotImplementedException();
        }

        public Answer QueryWork(Request update)
        {
            return Menu.QueryWork(update);
        }
    }
}
