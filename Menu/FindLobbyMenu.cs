using BolyukGame.Communication.UPD;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;

namespace BolyukGame.Menu
{
    public class FindLobbyMenu : IMenu
    {
        public FindLobbyMenu()
        {
            UIList list = new UIList()
            {
                HighlightColor = Color.Yellow,
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Center, Vertical = Sticky.Center }
            };

            list.AddElement(new UILoadingLabel() { ShownText = "Searching", IsSelectable = false, IsLoadingShown = true });

            var back_but = new UILabel() { Text = "<- Back" };
            back_but.OnClick += (e) =>
            {
                GameState.Game.NavigateTo(new MainMenu());
                FindLobby.Stop();
            };

            list.AddElement(back_but);

            RegUI(list);

            FindLobby.ExecAsync((l) => LobbyResolve(l, list));
        }

        private void LobbyResolve(LobbyInfo l, UIList list)
        {
            if (list.Get(l.id) == null)
            {
                list.InsertElement(1, new UISelfDesctructLabel() { Text = $"{l.name} ({l.players})", TTL=5000});
            }
            else
            {
                var c = list.Get<UISelfDesctructLabel>(l.id);
                c.Text = $"{l.name} ({l.players})";
                c.TTL = 5000;
            }
        }
    }
}
