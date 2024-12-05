using BolyukGame.GameHandling.Container;
using BolyukGame.Menu;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.GameHandling.Controller.Listeners
{
    public class LobbyServerListener : IServerGameListener
    {
        public LobbyMenu Menu { get; set; }

        public void OnPlayerLeave(PlayerContainer palyer)
        {
            UIDispatcher.BeforeUpdate(() =>  GameState.CurrentLobby.PlayersList.Remove(palyer));
            Menu.NotifyUpdatePlayerList();
        }

        public void OnPlayerReqistered(PlayerContainer palyer)
        {
            UIDispatcher.BeforeUpdate(() => GameState.CurrentLobby.PlayersList.Add(palyer));
            Menu.NotifyUpdatePlayerList();
        }

        public Answer QueryWork(Request update)
        {
            var controller = GameState.Controller;

            //if (update.Type == RequestType.Join)
            //{
            //    controller.Players.Add(ByteUtils.Deserialize<string>(update.Body));

            //    Menu.NotifyUpdatePlayerList();

            //    GameState.CurrentLobby.PlayersCount++;
            //    return new Answer()
            //    {
            //        LobbyId = GameState.CurrentLobby.Id,
            //        Type = AnswerType.PlayerInfo,
            //        Body = ByteUtils.Serialize(controller.Players)
            //    };
            //}
            return null;
        }

        public void acceptQuery(Answer update)
        {
            throw new NotImplementedException();
        }
    }
}
