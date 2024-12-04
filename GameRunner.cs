using BolyukGame.Menu;
using BolyukGame.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BolyukGame
{
    public class GameRunner : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IMenu currentMenu;

        public GameRunner()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameState.GraphicsDevice = GraphicsDevice;
            GameState.Game = this;
            GameState.Font = Content.Load<SpriteFont>("font");
            currentMenu = new MainMenu();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentMenu.InternalUpdate(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            currentMenu.InternalDraw(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void NavigateTo(IMenu menu)
        {
            this.currentMenu = menu;
        }
    }
}
