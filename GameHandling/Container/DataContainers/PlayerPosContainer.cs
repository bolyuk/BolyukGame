using Microsoft.Xna.Framework;
using System;

namespace BolyukGame.GameHandling.Container.DataContainers
{
    public class PlayerPosContainer
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public Guid PlayerId { get; set; }
    }
}
