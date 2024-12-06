using BolyukGame.GameHandling.Container;
using BolyukGame.Menu;
using BolyukGame.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace BolyukGame.GameHandling.Controller.Listeners.Game
{
    public class GamePlayerListener : IPlayerGameListener
    {
        public GameMenu Menu { get; set; }

        public void AcceptQuery(Answer update)
        {
            Menu.AcceptQuery(update);
        }

        public void OnError(ErrorEventArgs args)
        {
            Menu.ShowSimpleToast(args.Message);
        }

        public void OnSessionEnds()
        {
            Menu.ShowSimpleToast("Session ended!");
            GameState.Game.NavigateTo(new MainMenu());
        }

        public void OnSessionStarts()
        {
            throw new NotImplementedException();
        }
    }
}
