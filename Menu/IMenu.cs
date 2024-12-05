using BolyukGame.Shared;
using BolyukGame.UI;
using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;


namespace BolyukGame.Menu
{
    public abstract class IMenu
    {

        UIContainer grid = new UIContainer();
        KeyHandling keyHandling = new KeyHandling();
        KeyEvent lastKeyUpdate = new KeyEvent();

        public void InternalUpdate(GameTime gameTime)
        {
            UIDispatcher.ProccessBeforeUpdate();
            BeforeUpdate(gameTime);

            keyHandling.Update();
            var keyEvent = new Shared.KeyEvent()
            {
                DownKeys = keyHandling.GetPressedKeys(),
                UpKeys = keyHandling.GetReleasedKeys(),
            };

            bool isKeyHandlingNeeded = keyEvent.UpKeys.Any() || keyEvent.DownKeys.Any();

            if (isKeyHandlingNeeded && !lastKeyUpdate.Equals(keyEvent))
            {
                grid.onKeyEvent(keyEvent);
                lastKeyUpdate = keyEvent;
            }
                
            grid.Update(gameTime);

            Update(gameTime);
        }

        public void InternalDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            BeforeDraw(gameTime, spriteBatch);
            grid.Draw(gameTime, spriteBatch);
            Draw(gameTime, spriteBatch);
        }

        public virtual void BeforeUpdate(GameTime gameTime) { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void BeforeDraw(GameTime gameTime, SpriteBatch spriteBatch) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        public virtual void OnResize(int width, int height)
        {
            grid.OnParentResized(width, height);
        }

        public void RegUI(UIElement element)
        {
            grid.AddElement(element);
        }

        public void Focus(UIElement element)
        {
            grid.ChildFocus(element);
        }
    }
}
