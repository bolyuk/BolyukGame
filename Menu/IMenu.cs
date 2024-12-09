using BolyukGame.Shared;
using BolyukGame.UI;
using BolyukGame.UI.Interface;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;


namespace BolyukGame.Menu
{
    public class IMenu
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
                GameTime = gameTime,
            };

            bool isKeyHandlingNeeded = keyEvent.UpKeys.Any() || keyEvent.DownKeys.Any();

            if (isKeyHandlingNeeded && !lastKeyUpdate.Equals(keyEvent))
            {
               if(!CatchKeyEvent(keyEvent))
                    grid.onKeyEvent(keyEvent);
                lastKeyUpdate = keyEvent;
            }
                
            grid.Update(gameTime);

            Update(gameTime);
        }

        public virtual bool CatchKeyEvent(KeyEvent args)
        {
            return false;
        }

        public void InternalDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            BeforeDraw(gameTime, spriteBatch);
            grid.Draw(gameTime, spriteBatch);
            Draw(gameTime, spriteBatch);
        }

        public virtual void FocusFadingChanged(bool isFaded)
        {
            grid.IsFocusFaded = isFaded;
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

        public void UnRegUI(UIElement element)
        {
            grid.RemoveElement(element);
        }

        public void Focus(UIElement element)
        {
            grid.ChildFocus(element);
        }

        public void ShowSimpleToast(string text, Color? background_color = null, long ttl = 1000)
        {
            UIDispatcher.BeforeUpdate(() =>
            {
                var toast = new UISelfDesctructLabel()
                {
                    Text = text,
                    TTL = ttl,
                    PositionPolicy = new StickyPositionPolicy() { Horizontal = StickyPosition.Center, Vertical = StickyPosition.Bottom },
                    Background = background_color ?? Color.Red,
                    Padding = new int[] {5,5,5,5},
                };
                GameState.Game.InfoLayer.RegUI(toast);
                toast.ForceOnParentResized();
            });
        }
    }
}
