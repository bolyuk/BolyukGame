using BolyukGame.UI.Interface;
using System.Linq;


namespace BolyukGame.UI
{
    public class UIList : UIContainer
    {
        public override void AddElement(UIElement element)
        {
            base.AddElement(element);
            CalculateSize();
        }

        public void InsertElement(int pos, UIElement element)
        {
            elements.Insert(pos, element);
            element.Parent = this;
            CalculateSize();
        }

        public override void RemoveElement(UIElement element)
        {
            base.RemoveElement(element);
            CalculateSize();
        }

        public override void CalculateSize()
        {
            base.CalculateSize();

            this.Height = elements.Sum(e => e.Height);
            this.Width = elements.Max(e => e.Width);

            for (int i = 0; i < elements.Count; i++)
            {
                var e = elements[i];
                e.StartX = this.StartDrawX;
                e.StartY = this.StartDrawY;
                for (int j = 0; j < i; j++)
                    e.StartY += elements[j].Height;
            }

            elements.ForEach(e => e.CalculateSize());
        }
    }
}
