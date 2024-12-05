using BolyukGame.Shared;
using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BolyukGame.UI.Label
{
    public class UILabel : UIElement, UIKeyHandle
    {
        private string text;
        public string Text
        {
            get => text;
            set
            {
                if (!string.IsNullOrEmpty(value) && GameState.Font != null)
                {
                    text = value;

                    var textSize = GameState.Font.MeasureString(value);

                    Width = Width == 0 ? (int)textSize.X : Width;
                    Height = Height == 0 ? (int)textSize.Y : Height;
                }
            }
        }
        public Color TextColor { get; set; } = Color.Black;

        public event Action<UIElement> OnClick;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (string.IsNullOrEmpty(Text))
                return;

            var textPosition = new Vector2(StartDrawX, StartDrawY);
            spriteBatch.DrawString(GameState.Font, text, textPosition, TextColor);


        }

        public bool onKeyEvent(KeyEvent args)
        {
            if (IsSelectable && OnClick != null && args.IsOnlyDown(Keys.Enter))
            {
                OnClick?.Invoke(this);
                return true;
            }
            return Parent.TryMoveFromChild(this, args);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsFocused)
                Background = Color.Yellow;
            else
                Background = Color.Transparent;
        }
    }
}
