using BolyukGame.Shared;
using BolyukGame.UI;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;

namespace BolyukGame.Menu
{
    public class MainMenu : IMenu
    {
        public MainMenu()
        {
            UIList list = new UIList()
            {
                HighlightColor = Color.Yellow,
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Center, Vertical = Sticky.Center }
            };
            var first_but = new UILabel() { Text = "Join  Game" };
            list.AddElement(first_but);
            list.AddElement(new UILabel() { Text = "Create  Game" });
            list.AddElement(new UILabel() { Text = "Settings" });

            list.Get<UILabel>(0).OnClick += (e) => GameState.Game.NavigateTo(new FindLobbyMenu());
            list.Get<UILabel>(1).OnClick += (e) => GameState.Game.NavigateTo(new CreateLobbyMenu());

            var exit_but = new UILabel()
            {
                Text = "Exit",
                Padding = new int[4] { 0, 0, 0, 10 },
                PositionPolicy = new StickyPolicy() {Vertical=Sticky.Bottom, Horizontal=Sticky.Center }
            };
            exit_but.OnClick += (e) => GameState.Game.Exit();

            RegUI(exit_but);
            RegUI(list);

            list.ChildFocus(first_but);
        }
    }
}
