using BolyukGame.GameHandling.Container;
using BolyukGame.Menu;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI;
using System;
using System.Linq;

namespace BolyukGame.GameHandling.Controller.Listeners.Lobby
{
    public class LobbyServerListener : IServerGameListener
    {
        public LobbyMenu Menu { get; set; }

        public void OnPlayerLeave(PlayerContainer palyer)
        {
            UIDispatcher.BeforeUpdate(() => GameState.CurrentLobby.PlayersList.Remove(palyer));
            Menu.NotifyUpdatePlayerList();
        }

        public void OnPlayerReqistered(PlayerContainer palyer)
        {
            UIDispatcher.BeforeUpdate(() => GameState.CurrentLobby.PlayersList.Add(palyer));
            Menu.NotifyUpdatePlayerList();
        }

        public Answer QueryWork(Request update)
        {
            if (update.Type == RequestType.ColorSelect)
            {
                var data = ByteUtils.Deserialize<ColorContainer>(update.Body);
                if(data == null) return null;

                var p = GameState.CurrentLobby.PlayersList.Where(p => p.Id == data.PlayerId).FirstOrDefault();
                if(p == null) return null;

                p.Color = data.Color;
               
                var server = GameState.Controller as ServerController;

                server.Broadcast(new Answer() { 
                    LobbyId = GameState.CurrentLobby.Id, 
                    Body=update.Body,
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
