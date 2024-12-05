using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework;

namespace BolyukGame.UI.Animation
{
    public interface IAnimationPolicy
    {
        public void OnBeforeDraw(UIElement element, GameTime gameTime);

        public Vector2 ModifyOrigin(UIElement element) { return Vector2.Zero; }

        public Vector2 ModifyPosition(GameTime gameTime, Vector2 position) { return position; }
    }
}
