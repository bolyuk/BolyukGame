using BolyukGame.Communication.UPD;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using System.Threading;

namespace BolyukGame.Menu
{
    public class FindLobbyMenu : IMenu
    {
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
                HighlightColor = Color.Yellow,
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Center, Vertical = Sticky.Center }
            };

            list.AddElement(new UILoadingLabel() { ShownText = "Searching", IsSelectable = false, IsLoadingShown = true });

            var back_but = new UILabel() { 
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
                    list.InsertElement(1, new UISelfDesctructLabel() { id = l.Id, Text = $"{l.Name} ({l.Players})", TTL = 5000 });
                });            
            }
            else
            {
                var c = list.Get<UISelfDesctructLabel>(l.Id);
                c.Text = $"{l.Name} ({l.Players})";
                c.TTL = 5000;
            }
        }
    }
}
