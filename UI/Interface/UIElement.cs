using BolyukGame.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BolyukGame.UI.Interface
{
    public abstract class UIElement
    {
        private int width, height, widthMax, widtMin, heightMax, heightMin;
        private Color background;
        private Texture2D backgroundTexture = new Texture2D(GameState.GraphicsDevice, 1, 1);

        public Color Background
        {
            get => background;
            set
            {
                background = value;
                backgroundTexture.SetData(new[] { background });
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

        public int[] Padding { get; } = new int[4];

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (backgroundTexture != null)
            {
                spriteBatch.Draw(backgroundTexture, new Rectangle(StartDrawX, StartDrawY, Width, Height), Color.White);
            }
        }

        public abstract void OnWindowResize(float window_width, float window_height);

        public abstract void ReCalculate();
    }
}
