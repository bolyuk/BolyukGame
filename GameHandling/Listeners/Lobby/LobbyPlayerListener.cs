using BolyukGame.Communication.Controller;
using BolyukGame.Communication.DataContainer;
using BolyukGame.Menu;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;

namespace BolyukGame.GameHandling.Listeners.Lobby
{
    public class LobbyPlayerListener : IPlayerGameListener
    {
        public LobbyMenu Menu { get; set; }

        private LobbyInfoExtended Lobby => GameState.CurrentLobby;

        public virtual void AcceptQuery(Answer update)
        {
            switch (update.Type)
            {
                case AnswerType.PlayerInfo:
                    Lobby.PlayersList = update.GetBody<List<PlayerContainer>>();
                    Menu.NotifyUpdatePlayerList();

                    break;
                case AnswerType.ColorPick:
                    var data = update.GetBody<ColorContainer>();
                    var player = Lobby.PlayersList.Where(p => p.Id == data.PlayerId).FirstOrDefault();

                    player.Color = data.Color;
                    Menu.NotifyUpdatePlayerList();

                    break;
                case AnswerType.GameStart:

                    var map = update.GetBody<GameMap>();

                    if (map == null)
                    {
                        Menu.ShowSimpleToast("Error Loading Map!");
                        GameState.Game.NavigateTo(new MainMenu());
                        break;
                    }

                    Lobby.Map = map;

                    GameState.Game.NavigateTo(new GameMenu());

                    break;
            }
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
            GameState.Controller.SendQuery(new Request(Lobby.Id)
            {
                Type = RequestType.Join
            }.SetBody(GameState.Config.PlayerContainer));
        }
    }
}
