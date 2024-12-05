using BolyukGame.Communication.UPD;
using BolyukGame.GameHandling;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI;
using BolyukGame.UI.Interface;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BolyukGame.Menu
{
    public class FindLobbyMenu : IMenu
    {
        private List<LobbyInfo> lobbies = new List<LobbyInfo>();
        public FindLobbyMenu()
        {
            var info = new UILabel()
            {
                Text = "[Lobby  Searching]",
                IsSelectable = false,
                Padding = new int[4] { 0, 10, 10, 0 },
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Right },
            };

            RegUI(info);

            UIList list = new UIList()
            {
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Center, Vertical = Sticky.Center }
            };

            list.AddElement(new UILoadingLabel() { ShownText = "Searching", IsSelectable = false, IsLoadingShown = true });

            var back_but = new UILabel()
            {
                Text = "<- Back",
                Padding = new int[4] { 10, 10, 0, 0 }
            };
            back_but.OnClick += (e) =>
            {
                GameState.Game.NavigateTo(new MainMenu());
                FindLobby.Stop();
            };

            RegUI(list);
            RegUI(back_but);
            Focus(back_but);


            FindLobby.ExecAsync((l) => LobbyResolve(l, list));
        }

        private void LobbyResolve(LobbyInfo l, UIList list)
        {
            if (list.Get(l.Id) == null)
            {
                UIDispatcher.BeforeUpdate(() =>
                {
                    var label = new UISelfDesctructLabel() { id = l.Id, Text = $"{l.Name} ({l.Players})", TTL = 5000 };
                    label.OnClick += (e) => Connect(e);

                    list.InsertElement(1, label);
                });
            }
            else
            {
                UIDispatcher.BeforeUpdate(() =>
                {
                    lobbies.Remove(l);
                    var c = list.Get<UISelfDesctructLabel>(l.Id);
                    c.Text = $"{l.Name} ({l.Players})";
                    c.TTL = 5000;
                });
            }

            lobbies.Add(l);
        }

        private void Connect(UIElement element)
        {
            var l = lobbies.Where(e => e.Id == element.id).FirstOrDefault();
            if (l == null)
                return;
            GameState.CurrentLobby = l;

            //have to be edited
            //l.Ip = "localhost";

            var client = new ClientController();
            try
            {
                client.TryStartSessionAsync(l.Ip);

                FindLobby.Stop();
                GameState.Game.NavigateTo(new LobbyMenu());
            }
            catch(Exception e)
            {
                Logger.l(e);
            }
        }
    }
}
