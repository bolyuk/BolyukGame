using BolyukGame.Shared;
using BolyukGame.UI;
using BolyukGame.UI.Animation;
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
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Center, Vertical = Sticky.Center }
            };

            var splash = new UILabel()
            {
                Text = "A   Bolyuk   GAME!",
                IsSelectable = false,
                TextScale = 2f,
                TextColor = Color.DarkRed,
                Padding = new int[4] { 0, 60, 0, 0 },
                PositionPolicy = new StickyPolicy()
                {
                    Horizontal = Sticky.Center
                },
                AnimationPolicy = new OscillatingTransformAnimation()
            };

            RegUI(splash);

            var join_but = new UILabel() { Text = "Join  Game" };
            join_but.OnClick += (e) => GameState.Game.NavigateTo(new FindLobbyMenu());

            list.AddElement(join_but);
            list.AddElement(new UILabel() { Text = "Create  Game" });
            list.AddElement(new UILabel() { Text = "Settings" });

            list.Get<UILabel>(1).OnClick += (e) => GameState.Game.NavigateTo(new CreateLobbyMenu());
            list.Get<UILabel>(2).OnClick += (e) => GameState.Game.NavigateTo(new SettingsMenu());
            var exit_but = new UILabel()
            {
                Text = "Exit",
                TextSelectedColor = Color.DarkRed,
                Padding = new int[4] { 0, 0, 0, 10 },
                PositionPolicy = new StickyPolicy() {Vertical=Sticky.Bottom, Horizontal=Sticky.Center }
            };
            exit_but.OnClick += (e) => GameState.Game.Exit();

            RegUI(exit_but);
            RegUI(list);
            Focus(list);

            list.ChildFocus(join_but);
        }
    }
}
