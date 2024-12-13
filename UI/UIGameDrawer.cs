using BolyukGame.Shared.Info;
using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BolyukGame.UI
{
    public class UIGameDrawer : UIElement
    {

        public float BlockSize { get; internal set; }

        public LobbyInfoExtended Lobby { get; internal set; }

        private GameMap Map => Lobby?.Map;

        private List<PlayerContainer> Players => Lobby.PlayersList;

        public override void OnParentResized(int window_width, int window_height)
        {
            BlockSize = Math.Min(Parent.Width / Map.Width, Parent.Height / Map.Height);

            Width = (int)(BlockSize * Map.Width);
            Height = (int)(BlockSize * Map.Height);

            base.OnParentResized(window_width, window_height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            long pointer = 0;
            float value = BlockSize /GameRunner.Wall.Bounds.Width;
            var scale = new Vector2(value, value);

            for (int y = 0; y < Map.Height; y++)
                for (int x = 0; x < Map.Width; x++)
                {
                    int data = Map.CollisionLayer[pointer++];



                    if (data == 0)
                        continue;

                    DrawTexture(spriteBatch,
                        GameRunner.Wall,
                        new Vector2(StartX + (x * BlockSize), StartY + (y * BlockSize)-GameRunner.Wall.Height),
                        scale);
                }

            Players.ForEach(p =>
            DrawTexture(spriteBatch,
            GameRunner.Player,
            new Vector2((p.Position.X * BlockSize) + StartX, (p.Position.Y *BlockSize) + StartY),
            scale,
            p.Color));
        }

        private void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 scale, Color? color = null)
        {
            spriteBatch.Draw(texture,
                        pos,
                        null,
                        color ?? Color.White,
                        0f,
                        Vector2.Zero,
                        scale,
                        SpriteEffects.None,
                        0);
        }
    }
}