using BolyukGame.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BolyukGame.UI.Interface
{
    public class UIContainer : UIElement, UIKeyHandle
    {
        private bool isFocused = false;

        protected List<UIElement> elements = new List<UIElement>();

        protected List<UIElement> removeRequests = new List<UIElement>();

        public virtual UIElement FocusedElement { get; internal set; }
        public override bool IsFocused
        {
            get => isFocused;
            internal set
            {
                isFocused = value;

                if (value)
                    OnFocusGot();
                else
                    OnFocusLost();
            }
        }

        public virtual void ChildFocus(UIElement element)
        {
            if (!elements.Contains(element))
                return;

            ChildFocusLost();

            FocusedElement = element;
            element.IsFocused = true;
        }

        public virtual void ChildFocusLost()
        {
            if (FocusedElement != null)
                FocusedElement.IsFocused = false;
            FocusedElement = null;
        }

        public virtual bool TryResolveByFocused(KeyEvent args)
        {
            if (FocusedElement != null && FocusedElement is UIKeyHandle keyHandle && keyHandle.onKeyEvent(args))
                return true;

            return false;
        }

        public virtual bool TryMoveFromChild(UIElement from, KeyEvent args)
        {
            if (!args.IsOnlyUp(Keys.Up) && !args.IsOnlyUp(Keys.Down) &&
                !args.IsOnlyUp(Keys.Left) && !args.IsOnlyUp(Keys.Right))
                return false;

            var nextElement = GetNextElement(from, args);

            if (nextElement != null)
            {
                ChildFocus(nextElement);
                return true;
            }

            if (Parent is UIContainer parentContainer && parentContainer.TryMoveFromChild(this, args))
            {
                ChildFocusLost();
                return true;
            }

            return false;
        }

        public virtual bool TryMove(KeyEvent args)
        {
            if (!args.IsOnlyUp(Keys.Up) && !args.IsOnlyUp(Keys.Down) &&
                !args.IsOnlyUp(Keys.Left) && !args.IsOnlyUp(Keys.Right))
                return false;

            var nextElement = GetNextElement(FocusedElement, args);

            if (nextElement != null)
            {
                ChildFocus(nextElement);
                return true;
            }

            if (Parent != null && Parent is UIContainer parentContainer)
            {
                return parentContainer.TryMoveFromChild(this, args);
            }

            return false;
        }

        private UIElement GetNextElement(UIElement current, KeyEvent args)
        {

            // Получаем координаты текущего элемента
            var currentX = current?.StartX ?? 0;
            var currentY = current?.StartY ?? 0;

            // Фильтруем элементы, чтобы выбрать только подходящие по направлению
            var candidates = elements
                .Where(e => e != current && e.IsSelectable) // Исключаем текущий элемент
                .Where(e => IsInDirection(currentX, currentY, e, args)) // Проверяем направление
                .ToList();

            // Если кандидатов нет, возвращаем null
            if (!candidates.Any()) return null;

            // Ищем ближайший элемент по евклидову расстоянию
            return candidates
                .OrderBy(e => GetDistance(currentX, currentY, e.StartX, e.StartY))
                .FirstOrDefault();
        }

        private bool IsInDirection(int currentX, int currentY, UIElement candidate, KeyEvent args)
        {
            if (args.IsOnlyUp(Keys.Up))
                return candidate.StartY < currentY; // Вверх
            if (args.IsOnlyUp(Keys.Down))
                return candidate.StartY > currentY; // Вниз
            if (args.IsOnlyUp(Keys.Left))
                return candidate.StartX < currentX; // Влево
            if (args.IsOnlyUp(Keys.Right))
                return candidate.StartX > currentX; // Вправо
            return false;
        }

        private double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public virtual bool onKeyEvent(KeyEvent args)
        {
            if (TryResolveByFocused(args)) return true;
            if (TryMove(args)) return true;

            //foreach (UIKeyHandle item in elements.Where(e => e is UIKeyHandle))
            //{
            //    bool r = item.onKeyEvent(args);
            //    if (r)
            //        return true;
            //}
            return false;
        }

        public override void OnParentResized(int width, int height)
        {
            if (Parent == null)
            {
                this.Width = width;
                this.Height = height;
            }
            else
            {
                base.OnParentResized(width, height);
            }
            elements.ForEach(e => e.OnParentResized(width, height));
        }

        public override void Update(GameTime gameTime)
        {
            elements.ForEach(e => e.Update(gameTime));
            removeRequests.ForEach(e => RemoveElement(e));
            removeRequests.Clear();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            elements.ForEach(e => e.Draw(gameTime, spriteBatch));
        }

        public virtual void AddElement(UIElement element)
        {
            elements.Add(element);
            element.Parent = this;
        }

        public virtual void RemoveElement(UIElement element)
        {
            if (element == FocusedElement)
                ChildFocusLost();
            elements.Remove(element);
            element.Parent = null;
        }

        public virtual void RequestRemove(UIElement element)
        {
            removeRequests.Add(element);
        }

        public override void CalculateSize()
        {
            elements.ForEach(e => e.CalculateSize());
        }

        public virtual UIElement Get(int index)
        {
            return elements[index];
        }

        public virtual T Get<T>(int index) where T : UIElement
        {
            return (T)elements[index];
        }

        public virtual UIElement Get(Guid id)
        {
            return elements.Where(e => e.id == id).FirstOrDefault();
        }

        public virtual T Get<T>(Guid id) where T : UIElement
        {
            return (T)elements.Where(e => e.id == id).FirstOrDefault();
        }
        protected override void OnFocusLost()
        {
            ChildFocusLost();
        }

        protected override void OnFocusGot()
        {
            TryMove(new KeyEvent() { UpKeys = new List<Keys>() { Keys.Down } });
        }
    }
}
