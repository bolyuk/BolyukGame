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
        private double elapseTimeKeyshandled = 0;
        private Vector2 textSize = Vector2.Zero;

        public float TextScale
        {
            get => textScale;
            set
            {
                if (Math.Abs(textScale - value) > float.Epsilon)
                {
                    textScale = value;
                    CalculateSize();
                }
            }
        }

        public bool DrawShadowOnSelected { get; set; } = true;

        public string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    CalculateSize();
                }
            }
        }

        public float TextWidth => textSize.X * TextScale;
        public float TextHeight => textSize.Y * TextScale;

        public float Rotation { get; set; }
        public Color TextColor { get; set; } = Color.Black;

        public Color? TextSelectedColor { get; set; } = null;

        public virtual Color ShadowColor {  get; set; } = Color.Gray;

        public event Action<UIElement> OnClick;

        public long KeyHandlingCoolDown { get; set; } = 25;

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

            var textColor = TextColor;

            if(DrawShadowOnSelected && IsFocused)
            {
                spriteBatch.DrawString(
                GameState.Font,
                text,
                textPosition,
                ShadowColor,
                Rotation,
                origin,
                TextScale,
                SpriteEffects.None,
                0
            );
                textPosition = new Vector2(textPosition.X-5, textPosition.Y-5);
            }

            if (IsFocused && TextSelectedColor != null)
                textColor = TextSelectedColor.Value;

            spriteBatch.DrawString(
                GameState.Font,
                text,
                textPosition,
                textColor,
                Rotation,
                origin,
                TextScale,
                SpriteEffects.None,
                0
            );
        }

        public virtual bool onKeyEvent(KeyEvent args)
        {

            if (elapseTimeKeyshandled < KeyHandlingCoolDown || args.DownKeys.Count != 0)
                return true;

            elapseTimeKeyshandled = 0;

            if (IsSelectable && OnClick != null && args.IsOnlyUp(Keys.Enter))
            {
                OnClick?.Invoke(this);
                return true;
            }
            return Parent?.TryMoveFromChild(this, args) ?? false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            elapseTimeKeyshandled += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void CalculateSize()
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

