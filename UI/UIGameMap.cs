using BolyukGame.UI.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BolyukGame.UI
{
    public class UIGameMap : UIContainer
    {
        public bool IsGameStarted { get; set; } = false;

        public int MapWidthBlocks { get; set; } = 32;

        public int MapHeightBlocks { get; set; } = 16;

        private int blockWidth = 0;

        private int blockHeight = 0;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!IsGameStarted)
                return;

            base.Draw(gameTime, spriteBatch);

        }

        public override void Update(GameTime gameTime)
        {
            if (!IsGameStarted)
                return;

            base.Update(gameTime);
        }

        public override void OnParentResized(int width, int height)
        {
            base.OnParentResized(width, height);

            if(width > height)
            {
                Width = width;
                blockWidth = width/MapWidthBlocks;
                blockHeight = blockWidth;
            } 
            else
            {
                Width = width;
                blockWidth = width / MapWidthBlocks;
                blockHeight = blockWidth;
            }
        }
    }
}
