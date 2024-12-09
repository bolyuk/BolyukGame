using BolyukGame.Communication.UPD;
using BolyukGame.GameHandling;
using BolyukGame.GameHandling.Container;
using BolyukGame.GameHandling.Controller.Listeners.Lobby;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.Shared.Info.Maps;
using BolyukGame.UI;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BolyukGame.Menu
{
    public class LobbyMenu : IMenu
    {
        private UIList player_list = new UIList()
        {
            PositionPolicy = new StickyPositionPolicy() { Horizontal = StickyPosition.Center, Vertical = StickyPosition.Center },
        };

        private UIColorPickerLabel color_label = new UIColorPickerLabel()
        {
            Text = "Color: ",
            Colors = new List<Color>() { Color.Red, Color.Blue, Color.Black, Color.White, Color.Yellow, Color.Green },
            Padding = new int[] { 10, 10, 0, 0 },
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

            color_label.StartY = back_button.Height + 10;

            color_label.OnEditModeLeaved += () =>
            {
                var p = GameState.CurrentLobby.PlayersList.Where(
                    p => p.Color == color_label.Selected &&
                    p.Id != GameState.Config.UserId
                    ).FirstOrDefault();

                if (p != null)
                {
                    ShowSimpleToast("This Color is Busy!");
                    color_label.ForceEditMode();
                    return;
                }

                GameState.Controller.SendQuery(new Request()
                {
                    LobbyId = GameState.CurrentLobby.Id,
                    Type = RequestType.ColorSelect,
                    Body = ByteUtils.Serialize(new ColorContainer()
                    {
                        Color = color_label.Selected,
                        PlayerId = GameState.Config.UserId,
                    }),
                });
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
                PositionPolicy = new StickyPositionPolicy() { Horizontal = StickyPosition.Right },
            };

            RegUI(info);
            RegUI(color_label);

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
                    PositionPolicy = new StickyPositionPolicy() { Horizontal = StickyPosition.Right, Vertical = StickyPosition.Bottom },
                };

                start_but.OnClick += (e) =>
                {
                    var p = GameState.CurrentLobby.PlayersList.Where(p => p.Color == null).FirstOrDefault();

                    if(p != null)
                    {
                        ShowSimpleToast("All users have to select a Color!");
                        return;
                    }

                    ShareLobby.Stop();
                    var server = GameState.Controller as ServerController;

                    //for test use only
                    GameState.CurrentLobby.Map = new DefaultGameMap();

                    server.Broadcast(new Answer()
                    {
                        LobbyId = GameState.CurrentLobby.Id,
                        Type = AnswerType.GameStart,
                        Body = ByteUtils.Serialize(GameState.CurrentLobby.Map)
                    });
                    GameState.Game.NavigateTo(new GameMenu());
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
                    IsSelectable = false,
                });

                list.ForEach(p =>
                {
                    var label = new UILabel() { Text = p.Name, id = p.Id, TextColor = p.Color.GetValueOrDefault() };
                    if (p.Id == GameState.Config.UserId)
                    {
                        label.Text += "  (ME)";
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
