using BolyukGame.GameHandling;
using BolyukGame.Shared.Info;
using Microsoft.Xna.Framework.Graphics;

namespace BolyukGame.Shared
{
    public class GameState
    {
        public static IGameController Controller { get; set; }

        public static LobbyInfo CurrentLobby { get; set; }

        //MonoGame
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static SpriteFont Font { get; set; }

        public static GameRunner Game { get; set; }
    }
}
