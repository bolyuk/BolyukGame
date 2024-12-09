using BolyukGame.GameHandling.Container;
using BolyukGame.Menu;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;

namespace BolyukGame.GameHandling.Controller.Listeners.Lobby
{
    public class LobbyPlayerListener : IPlayerGameListener
    {
        public LobbyMenu Menu { get; set; }

        public virtual void AcceptQuery(Answer update)
        {
            var controller = GameState.CurrentLobby;

            if (update.Type == AnswerType.PlayerInfo)
            {
                controller.PlayersList = ByteUtils.Deserialize<List<PlayerContainer>>(update.Body);
                Menu.NotifyUpdatePlayerList();
            }
            if (update.Type == AnswerType.ColorPick)
            {
                var data = ByteUtils.Deserialize<ColorContainer>(update.Body);

                if (data == null)
                    return;

                var p = controller.PlayersList.Where(p => p.Id == data.PlayerId).FirstOrDefault();
                if (p == null)
                    return;
                

                p.Color = data.Color;
                Menu.NotifyUpdatePlayerList();
            }
            if (update.Type == AnswerType.GameStart)
            {
                var map = ByteUtils.Deserialize<GameMap>(update.Body);

                if(map == null)
                {
                    Menu.ShowSimpleToast("Error Loading Map!");
                    GameState.Game.NavigateTo(new MainMenu());
                    return;
                }

                GameState.CurrentLobby.Map = map;

                GameState.Game.NavigateTo(new GameMenu());
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
            GameState.Controller.SendQuery(new Request()
            {
                LobbyId = GameState.CurrentLobby.Id,
                Body = ByteUtils.Serialize(GameState.Config.PlayerContainer),
                Type = RequestType.Join
            });
        }
    }
}
