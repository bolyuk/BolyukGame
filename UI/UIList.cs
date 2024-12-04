using BolyukGame.Shared;
using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BolyukGame.UI
{
    public class UIList : UIElement, UIKeyHandle
    {
        private List<UIElement> elements = new List<UIElement>();
        private int current = 0;

        private Texture2D highlightTexture = new Texture2D(GameState.GraphicsDevice, 1, 1);
        private Color highhlightColor;

        public Color HighlightColor
        {
            get => highhlightColor;
            set
            {
                highhlightColor = value;
                highlightTexture.SetData(new[] { highhlightColor });
            }
        }

        public bool onKeyEvent(KeyEvent args)
        {
            if (args.IsOnlyUp(Keys.Down))
            {
                current = Math.Min(current + 1, elements.Count - 1);
                return true;
            }
            if (args.IsOnlyUp(Keys.Up))
            {
                current = Math.Max(current - 1, 0);
                return true;
            }

            bool r = false;

            foreach (UIKeyHandle item in elements.Where(e => e is UIKeyHandle))
            {
                r = item.onKeyEvent(args);
                if (r)
                    return true;
            }
            return false;
        }

        public override void OnWindowResize(float width, float height)
        {
            elements.ForEach(e => e.OnWindowResize(width, height));
        }

        public override void Update(GameTime gameTime)
        {
            elements.ForEach(e => e.Update(gameTime));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (i == current)
                {
                    var highlightRect = new Rectangle(elements[i].StartDrawX-1, elements[i].StartDrawY-1, elements[i].Width+1, elements[i].Height+1);
                    spriteBatch.Draw(highlightTexture, highlightRect, Color.White);
                }

                elements[i].Draw(gameTime, spriteBatch);
            }
        }

        public void AddElement(UIElement element)
        {
            element.StartX = this.StartX;
            element.StartY = this.StartY + elements.Sum(e => e.Height);
            elements.Add(element);
            ReCalculate();
        }

        public void RemoveElement(UIElement element)
        {
            elements.Remove(element);
            ReCalculate();
        }

        public override void ReCalculate()
        {
            this.Height = elements.Sum(e => e.Height);
            this.Width = elements.Max(e => e.Width);
            elements.ForEach(e => e.ReCalculate());
        }


        public UIElement Get(int index)
        {
            return elements[index];
        }

        public T Get<T>(int index) where T : UIElement
        {
            return (T)elements[index];
        }

    }
}
