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
            if(parent == null)
                throw new ArgumentNullException("parent");

            if(Horizontal == Sticky.Center)
                element.StartX = parent.StartX + (width - element.LogicalWidth) / 2;

            if(Vertical == Sticky.Center)
                element.StartY = parent.StartY + (height - element.LogicalHeight) / 2;
        }
    }

    public enum Sticky
    {
        None,
        Center,
        Up,
        Down,
    }
}
