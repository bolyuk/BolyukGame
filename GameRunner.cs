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
        private bool wasActive = true;
        public int WindowWidth { get; internal set; }
        public int WindowHeight { get; internal set; }

        private IMenu currentMenu;
        public IMenu InfoLayer { get; internal set; }

        public static Texture2D Wall { get; set; }

        public static Texture2D Player { get; set; } 

        public GameRunner()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            GameState.GraphicsDevice = GraphicsDevice;
            GameState.Game = this;
            GameState.Font = Content.Load<SpriteFont>("font");
            Wall = Content.Load<Texture2D>("drawable/brickwall_16px");
            Player = Content.Load<Texture2D>("drawable/arrowbutton_16px");
            currentMenu = new MainMenu();
            InfoLayer = new IMenu();
            Window.AllowUserResizing = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {

            currentMenu.InternalUpdate(gameTime);
            InfoLayer.InternalUpdate(gameTime);

            if (GraphicsDevice.Viewport.Width != WindowWidth || GraphicsDevice.Viewport.Height != WindowHeight)
            {              

                WindowWidth = GraphicsDevice.Viewport.Width;
                WindowHeight = GraphicsDevice.Viewport.Height;

                currentMenu.OnResize(WindowWidth, WindowHeight);
                InfoLayer.OnResize(WindowWidth, WindowHeight);
            }

            if (IsActive != wasActive)
            {
                wasActive=IsActive;
                currentMenu.FocusFadingChanged(!IsActive);
            }

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
        }
    }
}
