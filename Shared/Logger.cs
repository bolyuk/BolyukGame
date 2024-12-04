using System;

namespace BolyukGame.Shared
{
    public class Logger
    {
        public static void l(object o)
        {
            Console.WriteLine(o.ToString());
        }
    }
}
