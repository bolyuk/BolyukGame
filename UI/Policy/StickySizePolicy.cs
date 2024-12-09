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
            throw new NotImplementedException();
        }
    }

    public enum StickySize
    {
        None,
        Occupy,
        Wrap
    }
}
