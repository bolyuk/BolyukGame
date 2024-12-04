using BolyukGame.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BolyukGame.UI.Interface
{
    public interface UIKeyHandle
    {
        public virtual bool onKeyEvent(KeyEvent args) {  return false; }
    }
}
