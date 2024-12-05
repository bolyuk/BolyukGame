using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.Shared.Info
{
    public class LobbyInfoExtended : LobbyInfoSmall
    {
        public List<PlayerContainer> PlayersList { get; set; } = new List<PlayerContainer>();
    }
}
