using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.UI.Label
{
    public class UISelfDesctructLabel : UILabel
    {
        private double elapsedTime = 0;

        public long TTL { get; set; }
        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= TTL)
            {

                Parent.RemoveElement(this);
                Parent.ReCalculate();
            }
        }
    }
}
