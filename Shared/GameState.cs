using BolyukGame.Communication.Controller;
using BolyukGame.Shared.Info;
using Microsoft.Xna.Framework.Graphics;

namespace BolyukGame.Shared
{
    public class GameState
    {
        public static IGameController Controller { get; set; }

        public static LobbyInfoExtended CurrentLobby { get; set; }

        public static GameConfig Config { get; set; } = new GameConfig();

        //MonoGame
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static SpriteFont Font { get; set; }

        public static GameRunner Game { get; set; }
    }
}
