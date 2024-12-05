using BolyukGame.GameHandling.Container;
using BolyukGame.Menu;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace BolyukGame.GameHandling.Controller.Listeners
{
    public class LobbyPlayerListener : IPlayerGameListener
    {
        public LobbyMenu Menu { get; set; }

        public virtual void acceptQuery(Answer update)
        {
            var controller = GameState.CurrentLobby;

            if (update.Type == AnswerType.PlayerInfo)
            {
               controller.PlayersList = ByteUtils.Deserialize<List<PlayerContainer>>(update.Body);
               Menu.NotifyUpdatePlayerList();
            }
        }

        public void OnError(ErrorEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnSessionEnds()
        {
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
