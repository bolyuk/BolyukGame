using Microsoft.Xna.Framework;

namespace BolyukGame.UI.Label
{
    public class UISelfDesctructLabel : UILabel
    {
        private long ttl;
        private double elapsedTime = 0;
        private bool isFinished = false;

        public long TTL
        {
            get => ttl;
            set
            {
                ttl = value;
                elapsedTime = 0;
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= TTL && !isFinished)
            {
                UIDispatcher.BeforeUpdate(() => Parent.RequestRemove(this));
                isFinished = true;
            }
        }
    }
}
