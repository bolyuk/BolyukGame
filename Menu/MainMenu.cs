using BolyukGame.Shared;
using BolyukGame.UI;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BolyukGame.Menu
{
    public class MainMenu : IMenu
    {
        public MainMenu()
        {
            UIList list = new UIList() { 
                HighlightColor = Color.Yellow,
                PositionPolicy = new StickyPolicy() { Horizontal = Sticky.Center, Vertical = Sticky.Center}
            };

            list.AddElement(new UILabel() { Text = "Join  Game" });
            list.AddElement(new UILabel() { Text = "Create  Game" });
            list.AddElement(new UILabel() { Text = "Settings" });
            list.AddElement(new UILabel() { Text = "Exit"});

            list.Get<UILabel>(3).OnClick += (e) => GameState.Game.Exit();
            list.Get<UILabel>(0).OnClick += (e) => GameState.Game.NavigateTo(new FindLobbyMenu());
            RegUI(list);
        }
    }
}
