using BolyukGame.GameHandling;
using BolyukGame.GameHandling.Container;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI.Animation;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BolyukGame.Menu
{
    public class GameMenu : IMenu
    {
        private UILoadingLabel loading = new UILoadingLabel()
        {
            Text = "Loading",
            IsSelectable = false,
            TextScale = 2f,
            TextColor = Color.Black,
            PositionPolicy = new StickyPolicy()
            {
                Horizontal = Sticky.Center, Vertical = Sticky.Center,
            },
            AnimationPolicy = new OscillatingTransformAnimation()
        };

        public GameMenu() 
        { 
            RegUI(loading);
        }

        public void OnLoaded()
        {
            UnRegUI(loading);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        //both side
        public virtual void AcceptQuery(Answer answer)
        {

        }

        //both side
        public override bool CatchKeyEvent(KeyEvent args)
        {
            return false;
        }

        //server side
        public virtual void OnPlayerLeave(PlayerContainer player)
        {

        }

        //server side
        public virtual Answer QueryWork(Request request)
        {
            return null;
        }
    }
}
