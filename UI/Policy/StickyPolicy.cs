using BolyukGame.UI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BolyukGame.UI.Policy
{
    public class StickyPolicy : IPositionPolicy
    {
        public Sticky Horizontal { get; set; }
        public Sticky Vertical { get; set; }

        public void Execute(int width, int height, UIElement element, UIContainer parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            element.StartX = CalculateHorizontal(width, element, parent);
            element.StartY = CalculateVertical(height, element, parent);

            // Убедимся, что элемент не выходит за границы
            element.StartX = Math.Max(parent.StartX, Math.Min(parent.StartX + width - element.LogicalWidth, element.StartX));
            element.StartY = Math.Max(parent.StartY, Math.Min(parent.StartY + height - element.LogicalHeight, element.StartY));
        }

        private int CalculateHorizontal(int width, UIElement element, UIContainer parent)
        {
            switch (Horizontal)
            {
                case Sticky.Center:
                    return parent.StartX + (width - element.LogicalWidth) / 2;
                case Sticky.Left:
                    return parent.StartX;
                case Sticky.Right:
                    return parent.StartX + width - element.LogicalWidth;
                default:
                    return element.StartX;
            }
        }

        private int CalculateVertical(int height, UIElement element, UIContainer parent)
        {
            switch (Vertical)
            {
                case Sticky.Center:
                    return parent.StartY + (height - element.LogicalHeight) / 2;
                case Sticky.Top:
                    return parent.StartY;
                case Sticky.Bottom:
                    return parent.StartY + height - element.LogicalHeight;
                default:
                    return element.StartY;
            }
        }
    }
    public enum Sticky
    {
        None,
        Center,
        Top,
        Bottom,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight 
    }
}
