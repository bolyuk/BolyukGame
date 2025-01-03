﻿using BolyukGame.UI.Interface;
using System;

namespace BolyukGame.UI.Policy
{
    public class StickyPositionPolicy : IPositionPolicy
    {
        public StickyPosition Horizontal { get; set; }
        public StickyPosition Vertical { get; set; }

        public void Execute(int width, int height, UIElement element, UIContainer parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            element.StartX = CalculateHorizontal(width, element, parent);
            element.StartY = CalculateVertical(height, element, parent);

            element.StartX = Math.Max(parent.StartX, Math.Min(parent.StartX + width - element.LogicalWidth, element.StartX));
            element.StartY = Math.Max(parent.StartY, Math.Min(parent.StartY + height - element.LogicalHeight, element.StartY));
        }

        private int CalculateHorizontal(int width, UIElement element, UIContainer parent)
        {
            switch (Horizontal)
            {
                case StickyPosition.Center:
                    return parent.StartX + (width - element.LogicalWidth) / 2;
                case StickyPosition.Left:
                    return parent.StartX;
                case StickyPosition.Right:
                    return parent.StartX + width - element.LogicalWidth;
                default:
                    return element.StartX;
            }
        }

        private int CalculateVertical(int height, UIElement element, UIContainer parent)
        {
            switch (Vertical)
            {
                case StickyPosition.Center:
                    return parent.StartY + (height - element.LogicalHeight) / 2;
                case StickyPosition.Top:
                    return parent.StartY;
                case StickyPosition.Bottom:
                    return parent.StartY + height - element.LogicalHeight;
                default:
                    return element.StartY;
            }
        }
    }
    public enum StickyPosition
    {
        None,
        Center,
        Top,
        Bottom,
        Left,
        Right,
    }
}
