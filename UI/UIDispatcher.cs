using System;
using System.Collections.Concurrent;

namespace BolyukGame.UI
{
    public class UIDispatcher
    {
        private static ConcurrentBag<Action> beforeUpdateQueue = new ConcurrentBag<Action>();
        public static void BeforeUpdate(Action action)
        {
            beforeUpdateQueue.Add(action);
        }

        public static void ProccessBeforeUpdate()
        {
            while (beforeUpdateQueue.TryTake(out var action))
            {
                action.Invoke();
            }
        }
    }
}