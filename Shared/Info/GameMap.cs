using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.Shared.Info
{
    public class GameMap
    {
        public string Name { get; set; }
        public int[] CollisionLayer { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
    }
}
