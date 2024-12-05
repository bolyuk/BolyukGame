using BolyukGame.Menu;
using BolyukGame.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace BolyukGame
{
    public class GameRunner : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public int WindowWidth { get; internal set; }
        public int WindowHeight { get; internal set; }

        private IMenu currentMenu;
        public IMenu InfoLayer { get; internal set; }

        public GameRunner()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameState.GraphicsDevice = GraphicsDevice;
            GameState.Game = this;
            GameState.Font = Content.Load<SpriteFont>("font");
            currentMenu = new MainMenu();
            InfoLayer = new IMenu();

            Window.AllowUserResizing = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            

            currentMenu.InternalUpdate(gameTime);
            InfoLayer.InternalUpdate(gameTime);

            if (GraphicsDevice.Viewport.Width != WindowWidth || GraphicsDevice.Viewport.Height != WindowHeight)
            {              

                WindowWidth = GraphicsDevice.Viewport.Width;
                WindowHeight = GraphicsDevice.Viewport.Height;

                currentMenu.OnResize(WindowWidth, WindowHeight);
                InfoLayer.OnResize(WindowWidth, WindowHeight);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(
                samplerState: SamplerState.PointClamp
                );
            currentMenu.InternalDraw(gameTime, _spriteBatch);
            InfoLayer.InternalDraw(gameTime, _spriteBatch );
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void NavigateTo(IMenu menu)
        {
            this.currentMenu = menu;
            currentMenu.OnResize(WindowWidth, WindowHeight);
            //Thread.Sleep(100);
        }
    }
}
