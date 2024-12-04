using BolyukGame.Shared;
using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;


namespace BolyukGame.Menu
{
    public abstract class IMenu
    {

        List<UIElement> elements = new List<UIElement>();
        KeyHandling keyHandling = new KeyHandling();

        public void InternalUpdate(GameTime gameTime)
        {
            BeforeUpdate(gameTime);
            keyHandling.Update();
            var keyEvent = new Shared.KeyEvent()
            {
                DownKeys = keyHandling.GetPressedKeys(),
                UpKeys = keyHandling.GetReleasedKeys(),
            };

            bool isKeyHandlingNeeded = keyEvent.UpKeys.Any() || keyEvent.DownKeys.Any();

            elements.ForEach(e =>
            {
                if (isKeyHandlingNeeded  && e is UIKeyHandle keyHandle)
                    keyHandle.onKeyEvent(keyEvent);
                e.Update(gameTime);
            });
            Update(gameTime);
        }

        public void InternalDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            BeforeDraw(gameTime, spriteBatch);
            elements.ForEach(e => e.Draw(gameTime, spriteBatch));
            Draw(gameTime, spriteBatch);
        }

        public virtual void BeforeUpdate(GameTime gameTime) { }

        public abstract void Update(GameTime gameTime);

        public virtual void BeforeDraw(GameTime gameTime, SpriteBatch spriteBatch) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        public virtual void OnResize(float width, float height)
        {
            elements.ForEach(e => e.OnWindowResize(width, height));
        }

        public void RegUI(UIElement element)
        {
            elements.Add(element);
        }
    }
}
