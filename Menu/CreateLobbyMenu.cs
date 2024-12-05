using BolyukGame.Shared;
using BolyukGame.UI.Label;

namespace BolyukGame.Menu
{
    public class CreateLobbyMenu : IMenu
    {
        public CreateLobbyMenu()
        {
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
