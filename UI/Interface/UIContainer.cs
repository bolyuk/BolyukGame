using BolyukGame.Shared;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.UI.Interface
{
    public class UIContainer : UIElement, UIKeyHandle
    {
        protected List<UIElement> elements = new List<UIElement>();

        public virtual bool onKeyEvent(KeyEvent args)
        {
            foreach (UIKeyHandle item in elements.Where(e => e is UIKeyHandle))
            {
                bool r = item.onKeyEvent(args);
                if (r)
                    return true;
            }
            return false;
        }

        public override void OnParentResized(int width, int height)
        {
            if (Parent == null)
            {
                this.Width = width;
                this.Height = height;
            }
            else
            {
                base.OnParentResized(width, height);
            }
            elements.ForEach(e => e.OnParentResized(width, height));
        }

        public override void Update(GameTime gameTime)
        {
            elements.ForEach(e => e.Update(gameTime));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            elements.ForEach(e => e.Draw(gameTime, spriteBatch));
        }

        public virtual void AddElement(UIElement element)
        {
            elements.Add(element);
            element.Parent = this;
        }

        public virtual void RemoveElement(UIElement element)
        {
            elements.Remove(element);
            element.Parent = null;
        }

        public override void ReCalculate()
        {
            elements.ForEach(e => e.ReCalculate());
        }


        public virtual UIElement Get(int index)
        {
            return elements[index];
        }

        public virtual T Get<T>(int index) where T : UIElement
        {
            return (T)elements[index];
        }

        public virtual UIElement Get(Guid id)
        {
            return elements.Where(e => e.id == id).FirstOrDefault();
        }

        public virtual T Get<T>(Guid id) where T : UIElement
        {
            return (T)elements.Where(e => e.id == id).FirstOrDefault();
        }
    }
}
