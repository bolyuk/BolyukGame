using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.Shared.Info
{
    public class GameConfig
    {
        public string UserName { get; set; } = "TestUser";

        public Guid UserId { get; set; }  = Guid.NewGuid();

        public virtual PlayerContainer PlayerContainer { get => new PlayerContainer() { Id = UserId, Name = UserName }; }
    }
}
