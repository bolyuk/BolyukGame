using Microsoft.Xna.Framework;
using System;

namespace BolyukGame.Shared.Info
{
    public class PlayerContainer
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public Color? Color { get; set; }

        public bool IsAdmin { get; set; }

        public Vector2 Position { get; set; }
    }
}
