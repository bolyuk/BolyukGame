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
        private float textScale = 1f;
        private Vector2 textSize = Vector2.Zero; // Сохранённый размер текста

        public float TextScale
        {
            get => textScale;
            set
            {
                if (Math.Abs(textScale - value) > float.Epsilon)
                {
                    textScale = value;
                    ReCalculate();
                }
            }
        }

        public string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    ReCalculate();
                }
            }
        }

        public float TextWidth => textSize.X * TextScale;
        public float TextHeight => textSize.Y * TextScale;

        public float Rotation { get; set; }
        public Color TextColor { get; set; } = Color.Black;

        public event Action<UIElement> OnClick;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (string.IsNullOrEmpty(Text))
                return;

            var origin = Vector2.Zero;


            var textPosition = new Vector2(StartDrawX, StartDrawY);

            if (AnimationPolicy != null)
            {
                origin = AnimationPolicy.ModifyOrigin(this);
                textPosition = AnimationPolicy.ModifyPosition(gameTime, textPosition);
            }

            spriteBatch.DrawString(
                GameState.Font,
                text,
                textPosition,
                TextColor,
                Rotation,
                origin,
                TextScale,
                SpriteEffects.None,
                0
            );
        }

        public bool onKeyEvent(KeyEvent args)
        {
            if (IsSelectable && OnClick != null && args.IsOnlyDown(Keys.Enter))
            {
                OnClick?.Invoke(this);
                return true;
            }
            return Parent?.TryMoveFromChild(this, args) ?? false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Background = IsFocused ? Color.Yellow : Color.Transparent;
        }

        public override void ReCalculate()
        {
            if (!string.IsNullOrEmpty(Text) && GameState.Font != null)
            {
                textSize = GameState.Font.MeasureString(Text);

                Width = (int)(textSize.X * TextScale);
                Height = (int)(textSize.Y * TextScale);
            }
        }
    }
}

