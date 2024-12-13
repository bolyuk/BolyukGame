using System;

namespace BolyukGame.GameHandling.DataContainer.DataContainers
{
    public class EntityStateContainer
    {
        public Guid EntityId { get; set; }
        public bool IsUser {  get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int VectorX { get; set; }
        public int VectorY { get; set; }

        public int TextureId { get; set; }

    }
}
