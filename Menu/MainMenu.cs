using BolyukGame.Shared;
using BolyukGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BolyukGame.Menu
{
    public class MainMenu : IMenu
    {
        public MainMenu()
        {
            UIList list = new UIList() { HighlightColor = Color.Yellow};

            list.AddElement(new UILabel() { Text = "Join  Game" });
            list.AddElement(new UILabel() { Text = "Create  Game" });
            list.AddElement(new UILabel() { Text = "Settings" });
            list.AddElement(new UILabel() { Text = "Exit"});

            list.Get<UILabel>(3).OnClick += () => GameState.Game.Exit();

            RegUI(list);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
