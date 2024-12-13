using BolyukGame.Communication.Controller;
using BolyukGame.Communication.DataContainer;
using BolyukGame.Menu;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI;
using System;
using System.Linq;

namespace BolyukGame.GameHandling.Listeners.Lobby
{
    public class LobbyServerListener : IServerGameListener
    {
        public LobbyMenu Menu { get; set; }

        private LobbyInfoExtended Lobby => GameState.CurrentLobby;
        public void OnPlayerLeave(PlayerContainer palyer)
        {
            UIDispatcher.BeforeUpdate(() => Lobby.PlayersList.Remove(palyer));
            Menu.NotifyUpdatePlayerList();
        }

        public void OnPlayerReqistered(PlayerContainer palyer)
        {
            UIDispatcher.BeforeUpdate(() => Lobby.PlayersList.Add(palyer));
            Menu.NotifyUpdatePlayerList();
        }

        public Answer QueryWork(Request update)
        {
            if (update.Type == RequestType.ColorSelect)
            {
                var data = update.GetBody<ColorContainer>();
                var p = GameState.CurrentLobby.PlayersList.Where(p => p.Id == data.PlayerId).FirstOrDefault();

                p.Color = data.Color;

                var server = GameState.Controller as ServerController;

                server.Broadcast(new Answer(Lobby.Id)
                {
                    Body = update.Body,
                    Type = AnswerType.ColorPick,
                });

                Menu.NotifyUpdatePlayerList();
            }

            return null;
        }

        public void AcceptQuery(Answer update)
        {

        }
    }
}
