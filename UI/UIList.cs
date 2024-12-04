using BolyukGame.Shared;
using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BolyukGame.UI
{
    public class UIList : UIContainer
    {
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

        public override bool onKeyEvent(KeyEvent args)
        {
            if (args.IsOnlyUp(Keys.Down))
            {
                if(current == elements.Count - 1)
                    return false;
                current++;
                return true;
            }
            if (args.IsOnlyUp(Keys.Up))
            {
                if (current == 0)
                    return false;
                current--;
                return true;
            }

            if (elements[current] is UIKeyHandle keyHandle)
            {
              return keyHandle.onKeyEvent(args);
            }
            return false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (i == current)
                {
                    var highlightRect = new Rectangle(elements[i].StartX-1, elements[i].StartY-1, elements[i].LogicalWidth+1, elements[i].LogicalHeight+1);
                    spriteBatch.Draw(highlightTexture, highlightRect, Color.White);
                }

                elements[i].Draw(gameTime, spriteBatch);
            }
        }

        public override void AddElement(UIElement element)
        {
            base.AddElement(element);
            ReCalculate();
        }

        public void InsertElement(int pos, UIElement element)
        {
            elements.Insert(pos, element);
            element.Parent = this;
            ReCalculate();
        }

        public override void RemoveElement(UIElement element)
        {
            base.RemoveElement(element);
            ReCalculate();
        }

        public override void ReCalculate()
        {
            base.ReCalculate();

            this.Height = elements.Sum(e => e.Height);
            this.Width = elements.Max(e => e.Width);

            for (int i = 0; i < elements.Count; i++)
            {
                var e = elements[i];
                e.StartX = this.StartX;
                e.StartY = this.StartY;
                for (int j = 0; j < i; j++)
                    e.StartY += elements[j].Height;
            }

            elements.ForEach(e => e.ReCalculate());
        }
    }
}
