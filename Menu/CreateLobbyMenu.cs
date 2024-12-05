using BolyukGame.Shared;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;

namespace BolyukGame.Menu
{
    public class CreateLobbyMenu : IMenu
    {
        public CreateLobbyMenu()
        {
            var info = new UILabel()
            {
                Text = "[Lobby  Creating]",
                IsSelectable = false,
                Padding = new int[4] { 0, 10, 10, 0 },
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Right },
            };

            RegUI(info);

            var back_but = new UILabel() { 
                Text = "<- Back",
                Padding = new int[4] { 10, 10, 0, 0 }
            };

            back_but.OnClick += (e) =>
            {
                GameState.Game.NavigateTo(new MainMenu());
            };

            RegUI(back_but);
            Focus(back_but);
        }
    }
}
