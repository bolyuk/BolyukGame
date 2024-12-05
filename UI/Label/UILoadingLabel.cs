using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.UI.Label
{
    public class UILoadingLabel : UILabel
    {
        private string shownText;

        private int loadingProgress = -0;
        private readonly char[] progressChars = new char[] { '-', '\\', '|', '/' };
        private double elapsedTime = 0;
        public bool IsLoadingShown { get; set; }

        public string ShownText
        {
            get => shownText;
            set
            {
                Text = value;
                shownText = value;
            }
        }

        public long ProgressCoolDown { get; set; } = 300;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsLoadingShown) return;

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= ProgressCoolDown)
            {
                elapsedTime = 0;
                UIDispatcher.BeforeUpdate(() =>
                {
                    Text = $"{ShownText} {progressChars[loadingProgress]}";
                });             

                loadingProgress = (loadingProgress + 1) % progressChars.Length;
            }
        }
    }
}
