using BolyukGame.UI.Interface;
using BolyukGame.UI.Label;
using Microsoft.Xna.Framework;
using System;

namespace BolyukGame.UI.Animation
{
    public class OscillatingTransformAnimation : IAnimationPolicy
    {
        private float scaleDirection = 1f; // Направление изменения масштаба
        private float elapsedTime = 0f;

        public float MinScale { get; set; } = 0.8f; // Минимальный масштаб
        public float MaxScale { get; set; } = 1.2f; // Максимальный масштаб
        public float ScaleSpeed { get; set; } = 0.005f; // Скорость изменения масштаба

        public void OnBeforeDraw(UIElement element, GameTime gameTime)
        {

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (element is UILabel label)
            {
                label.TextScale += scaleDirection * ScaleSpeed;
                if (label.TextScale >= MaxScale)
                {
                    scaleDirection = -1f;
                    label.TextScale = MaxScale;
                }
                else if (label.TextScale <= MinScale)
                {
                    scaleDirection = 1f;
                    label.TextScale = MinScale;
                }
                label.ForceOnParentResized();
            }
        }

        public Vector2 ModifyOrigin(UIElement element)
        {
            //if (element is UILabel label)
            //    return new Vector2(label.TextWidth, label.TextHeight) / 2;

            return Vector2.Zero;
        }
    }
}

