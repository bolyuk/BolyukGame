using BolyukGame.Shared;
using BolyukGame.UI.Animation;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BolyukGame.UI.Interface
{
    public abstract class UIElement
    {
        private int width, height, widthMax, widtMin, heightMax, heightMin;
        private bool isFocused;
        private Color? background;
        private Color? backgroundOnFocus = Color.Yellow;

        private Texture2D backgroundTexture = new Texture2D(GameState.GraphicsDevice, 1, 1);
        private Texture2D backgroundOnFocusTexture = new Texture2D(GameState.GraphicsDevice, 1, 1);
        public UIContainer Parent { get; set; }

        public bool IsSelectable { get; set; } = true;

        public virtual bool IsFocused
        {
            get => isFocused;
            internal set
            {
                isFocused = value;
                if (value)
                    OnFocusGot();
            }
        }

        public IPositionPolicy PositionPolicy { get; set; }

        public IAnimationPolicy AnimationPolicy { get; set; }

        public Guid id { get; set; } = Guid.NewGuid();

        public Color Background
        {
            get => background.Value;
            set
            {
                background = value;
                backgroundTexture.SetData(new[] { background.Value });
            }
        }

        public Color OnFocusBackground
        {
            get => backgroundOnFocus.Value;
            set
            {
                backgroundOnFocus = value;
                backgroundOnFocusTexture.SetData(new[] { backgroundOnFocus.Value });
            }
        }

        #region Position

        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX => StartX + Padding[0] + Padding[2] + Width;
        public int EndY => StartY + Padding[1] + Padding[3] + Height;

        #endregion Position

        #region DrawPosition

        public int StartDrawX => StartX + Padding[0];
        public int StartDrawY => StartY + Padding[1];

        #endregion DrawPosition

        public int LogicalWidth { get => EndX - StartX; }

        public int LogicalHeight { get => EndY - StartY; }

        #region Width

        public int Width
        {
            get => width;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();

                if (value > WidthMax && WidthMax != 0) value = WidthMax;
                if (value < WidthMin) value = WidthMin;

                width = value;
            }
        }

        public int WidthMax
        {
            get => widthMax;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"{nameof(WidthMax)} can not be less than zero");
                if (value < WidthMin) throw new ArgumentOutOfRangeException($"{nameof(WidthMax)} can not be less than {nameof(WidthMin)}");

                widthMax = value;
                if (value > Width) Width = value;
            }
        }

        public int WidthMin
        {
            get => widtMin;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"{nameof(WidthMin)} can not be less than zero");
                if (value > WidthMax) throw new ArgumentOutOfRangeException($"{nameof(WidthMin)} can not be more than {nameof(WidthMax)}");

                widtMin = value;
                if (value < Width) Width = value;
            }
        }

        #endregion Width

        #region Height

        public int Height
        {
            get => height;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"{nameof(Height)} can not be less than zero");

                if (value > HeightMax && HeightMax != 0) value = HeightMax;
                if (value < HeightMin) value = HeightMin;

                height = value;
            }
        }

        public int HeightMax
        {
            get => heightMax;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"{nameof(HeightMax)} can not be less than zero");
                if (value < HeightMin) throw new ArgumentOutOfRangeException($"{nameof(HeightMax)} can not be less than {nameof(HeightMin)}");

                heightMax = value;
                if (value > Height) Height = value;
            }
        }

        public int HeightMin
        {
            get => heightMin;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException($"{nameof(HeightMin)} can not be less than zero");
                if (value > HeightMax) throw new ArgumentOutOfRangeException($"{nameof(HeightMin)} can not be more than {nameof(HeightMax)}");

                heightMin = value;
                if (value < Height) Height = value;
            }
        }

        #endregion Height


        public int[] Padding { get; set; } = new int[4];

        public UIElement()
        {
            //workaround
            OnFocusBackground = backgroundOnFocus.Value;
        }

        public virtual void Update(GameTime gameTime) 
        {
        
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (AnimationPolicy != null)
                AnimationPolicy.OnBeforeDraw(this, gameTime);

            DrawBackground(gameTime, spriteBatch);
        }

        protected virtual void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(backgroundOnFocus != null && IsFocused)
            {
                spriteBatch.Draw(backgroundOnFocusTexture, new Rectangle(StartDrawX, StartDrawY, Width, Height), Color.White);
                return;
            }

            if (background != null)
            {
                spriteBatch.Draw(backgroundTexture, new Rectangle(StartDrawX, StartDrawY, Width, Height), Color.White);
            }
        }

        public virtual void OnParentResized(int window_width, int window_height)
        {
            if (PositionPolicy != null)
                PositionPolicy.Execute(window_width, window_height, this, Parent);
            CalculateSize();
        }

        public virtual void ForceOnParentResized()
        {
            OnParentResized(GameState.Game.WindowWidth, GameState.Game.WindowHeight);
        }

        public virtual void CalculateSize()
        {

        }

        protected virtual void OnFocusGot()
        {

        }

        protected virtual void OnFocusLost()
        {

        }
    }
}
