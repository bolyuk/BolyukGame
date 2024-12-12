using BolyukGame.Shared;
using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework.Input;
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
            KeyEvent keyEvent = new Shared.KeyEvent();

            int StartX = parent.StartX;
            int StartY = parent.StartY;

            int Height = parent.Height;
            int Width = parent.Width;

            if (Top == StickySize.Occupy)
                element.StartY = StartY;
            if (Bottom == StickySize.Occupy)
                element.Height = Height - element.StartY;
            if (Left == StickySize.Occupy)
                element.StartX = StartX;
            if (Right == StickySize.Occupy)
                element.Width = Width - element.StartX;

            if (Top == StickySize.OccupySoft)
            {
                keyEvent.UpKeys.Clear();
                keyEvent.UpKeys.Add(Keys.Up);
                UIElement reference = parent.GetNextElement(element, keyEvent, true);
                if (reference != null)
                    element.StartY = reference.EndY;
                else
                    element.StartY = StartY;

            }

            if (Left == StickySize.OccupySoft)
            {
                keyEvent.UpKeys.Clear();
                keyEvent.UpKeys.Add(Keys.Left);
                UIElement reference = parent.GetNextElement(element, keyEvent, true);
                if (reference != null)
                    element.StartX = reference.EndX;
                else
                    element.StartX = StartX;
            }

            if (Right == StickySize.OccupySoft)
            {
                keyEvent.UpKeys.Clear();
                keyEvent.UpKeys.Add(Keys.Right);
                UIElement reference = parent.GetNextElement(element, keyEvent,true);
                if (reference != null)
                    element.Width = reference.Width - element.StartX;
                else
                    element.Width = Width - element.StartX;
            }

            if (Bottom == StickySize.OccupySoft)
            {
                keyEvent.UpKeys.Clear();
                keyEvent.UpKeys.Add(Keys.Down);
                UIElement reference = parent.GetNextElement(element, keyEvent,true);
                if (reference != null)
                    element.Height = reference.Height - element.StartY;
                else
                    element.Height = Height - element.StartY;
            }
        }
    }

    public enum StickySize
    {
        None,
        Occupy,
        OccupySoft,
    }
}
