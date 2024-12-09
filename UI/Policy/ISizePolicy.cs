using BolyukGame.UI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.UI.Policy
{
    public interface ISizePolicy
    {
        public void Execute(int width, int height, UIElement element, UIContainer parent);
    }
}
