using BolyukGame.UI.Interface;
using System;

namespace BolyukGame.UI.Policy
{
    public class StickySizePolicy : ISizePolicy
    {
        public StickySize Top { get; set; } = StickySize.None;

        public StickySize Bottom { get; set; } = StickySize.None;

        public StickySize Left { get; set; } = StickySize.None;

        public StickySize Right { get; set; } = StickySize.None;

        public void Execute(int width, int height, UIElement element, UIContainer parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));


            int StartX = parent.StartX;
            int StartY = parent.StartY;

            int Height = parent.Height;
            int Width = parent.Width;

            if (Top == StickySize.Occupy)
                element.StartX = StartX;
            if(Bottom == StickySize.Occupy)
                element.Width = Width - element.StartX;
 
        }
    }

    public enum StickySize
    {
        None,
        Occupy,
        Wrap
    }
}
