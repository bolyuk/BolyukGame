﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolyukGame.Shared
{
    public class KeyHandling
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        public KeyHandling()
        {
            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = currentKeyboardState;
        }

        public void Update()
        {
            // Сохраняем текущее состояние как предыдущее
            previousKeyboardState = currentKeyboardState;

            // Получаем текущее состояние клавиатуры
            currentKeyboardState = Keyboard.GetState();
        }

        public List<Keys> GetReleasedKeys()
        {
            // Получаем список всех клавиш, которые были нажаты в предыдущем кадре, но отжаты в текущем
            return previousKeyboardState.GetPressedKeys()
                .Where(key => !currentKeyboardState.IsKeyDown(key))
                .ToList();
        }

        public List<Keys> GetPressedKeys()
        {
            return currentKeyboardState.GetPressedKeys().ToList();
        }
    }

    public class KeyEvent
    {
        public List<Keys> DownKeys;

        public List<Keys> UpKeys;

        public bool IsOnlyDown(Keys key)
        {
            return DownKeys.Count == 1 && DownKeys[0] == key;
        }

        public bool IsOnlyUp(Keys key)
        {
            return UpKeys.Count == 1 && UpKeys[0] == key;
        }
    }
}