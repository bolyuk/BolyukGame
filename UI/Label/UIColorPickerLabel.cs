using BolyukGame.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BolyukGame.UI.Label
{
    public class UIColorPickerLabel : UILabel
    {
        protected bool isEditMode = false;
        protected double elapsedTime = 0;
        protected int selected = 0;

        protected Color shadowColor;

        private Texture2D SelectedTexture = new Texture2D(GameState.GraphicsDevice, 1, 1);
        private Texture2D ShadowTexture = new Texture2D(GameState.GraphicsDevice, 1, 1);
        public int DistanceToColor { get; set; } = 10;

        public override Color ShadowColor 
        { 
            get => shadowColor; 
            set
            {
                shadowColor = value;
                ShadowTexture.SetData(new[] {shadowColor});
            }
        }

        public event Action OnEditModeLeaved;
        public List<Color> Colors { get; set; } = new List<Color>();

        public Color Selected { get => Colors[selected]; }
        public override bool onKeyEvent(KeyEvent args)
        {
            if (args.IsOnlyUp(Keys.Enter))
            {
                isEditMode = !isEditMode;
                if (!isEditMode)
                    OnEditModeLeaved?.Invoke();
                return true;
            }

            if (!isEditMode)
                return Parent?.TryMoveFromChild(this, args) ?? false;

            else if (args.IsOnlyUp(Keys.Left) && selected > 0)
            {
                selected--;
            }
            else if (args.IsOnlyUp(Keys.Right) && selected < Colors.Count - 1)
            {
                selected++;
            }

            return true;
        }

        public UIColorPickerLabel()
        {
            ShadowColor = Color.Gray;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SelectedTexture.SetData(new[] { Colors[selected] });
        }

        public void ForceEditMode()
        {
            isEditMode=true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            DrawRectangle(spriteBatch);
        }

        public override void CalculateSize()
        {
            base.CalculateSize();
            Width += Height+ DistanceToColor;
        }
        private void DrawRectangle(SpriteBatch spriteBatch)
        {
            int x = 0;
            int y = 0;

            if (IsFocused)
            {
                x = 5;
                y = 5;
                if (DrawShadowOnSelected)
                    spriteBatch.Draw(ShadowTexture, new Rectangle(StartDrawX + Width - DistanceToColor- Height, StartDrawY, Height-5, Height-5), Color.White);
            }

            spriteBatch.Draw(SelectedTexture, new Rectangle(StartDrawX + Width - DistanceToColor - x- Height, StartDrawY - y, Height-5, Height-5), Color.White);

        }
    }
}