using BolyukGame.Communication.UPD;

namespace BolyukGame.Menu
{
    public class LobbyMenu : IMenu
    {
        public LobbyMenu() 
        {
            ShareLobby.ExecAsync(new Shared.Info.LobbyInfo() { Name = "test" });
        }
    }
}
