using BolyukGame.Communication.UPD;
using BolyukGame.GameHandling;
using BolyukGame.GameHandling.Container;
using BolyukGame.GameHandling.Controller.Listeners;
using BolyukGame.Shared;
using BolyukGame.UI;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;

namespace BolyukGame.Menu
{
    public class LobbyMenu : IMenu
    {
        private UIList player_list = new UIList()
        {
            PositionPolicy = new StickyPolicy() { Horizontal= Sticky.Center, Vertical= Sticky.Center },
        };
        public LobbyMenu()
        {
            bool isAdmin = GameState.Controller is ServerController;

            var back_button = new UILabel()
            {
                Text = isAdmin ? "<- Close  Lobby" : " <- Leave Lobby",
                Padding = new int[] { 10, 10, 0, 0 },
                TextSelectedColor = Color.DarkRed,
            };

            back_button.OnClick += (e) =>
            {
                GameState.Game.NavigateTo(new MainMenu());
                GameState.Controller.StopSession();
                GameState.Controller = null;
                GameState.CurrentLobby = null;
            };

            var info = new UILabel()
            {
                Text = $"[{GameState.CurrentLobby.Name}]",
                IsSelectable = false,
                Padding = new int[4] { 0, 10, 10, 0 },
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Right },
            };

            RegUI(info);

            if (isAdmin)
            {
                back_button.OnClick += (e) =>
                {
                    ShareLobby.Stop();
                };

                GameState.Controller.SetListener(new LobbyServerListener() { Menu = this });

                var me = GameState.Config.PlayerContainer;
                me.IsAdmin = true;
                GameState.CurrentLobby.PlayersList.Add(me);

                var start_but = new UILabel()
                {
                    Text = "Start ->",
                    Padding = new int[] { 0, 0, 10, 10 },
                    PositionPolicy = new StickyPolicy() { Horizontal= Sticky.Right, Vertical = Sticky.Bottom },
                };

                RegUI(start_but);
                NotifyUpdatePlayerList();
            }
            else
            {
                GameState.Controller.SetListener(new LobbyPlayerListener() { Menu = this });
            }

            RegUI(player_list);
            RegUI(back_button);
            Focus(back_button);
        }

        public void NotifyUpdatePlayerList()
        {
            UIDispatcher.BeforeUpdate(() =>
            {
                var list = GameState.CurrentLobby.PlayersList;

                player_list.Clear();
                player_list.AddElement(new UILabel() 
                { 
                    Text = $"Players: {list.Count}",
                    Padding = new int[] { 0, 0, 10, 0 },
                    IsSelectable = false 
                });

                list.ForEach(p =>
                {
                    var label = new UILabel() { Text = p.Name, id = p.Id };
                    if (p.Id == GameState.Config.UserId)
                    {
                        label.Text += "  (ME)";
                        label.TextColor = Color.Green;
                    }
                    player_list.AddElement(label);

                });
                player_list.ForceOnParentResized();

                if (!(GameState.Controller is ServerController))
                    return;

                var server = GameState.Controller as ServerController;

                server.Broadcast(new Answer()
                {
                    LobbyId = GameState.CurrentLobby.Id,
                    Type = AnswerType.PlayerInfo,
                    Body = ByteUtils.Serialize(GameState.CurrentLobby.PlayersList),
                });
            });
        }
    }
}
