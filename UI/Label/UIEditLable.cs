

using BolyukGame.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BolyukGame.UI.Label
{
    public class UIEditLable : UILabel
    {
        protected string question;
        protected string answer;
        protected bool isEditMode = false;
        protected bool isEditSymbolShown = false;
        protected double elapsedTime = 0;
        public string Question 
        {
            get => question;
            set
            {
                question = value;
                SetText(false);
            }
        }

        public string Answer
        {
            get => answer;
            set
            {
                answer = value;
                SetText(false);
            }
        }

        public string EditSymbol { get; set; } = "_";

        public long EditSymbolCoolDown { get; set; } = 300;      

        public override bool onKeyEvent(KeyEvent args)
        {
            if (args.IsOnlyUp(Keys.Enter))
            {
                isEditMode = !isEditMode;
                SetText(false);
                return true;               
            }

            if(!isEditMode)
                return Parent?.TryMoveFromChild(this, args) ?? false;
            else
                DecodeInput(args);

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!isEditMode)
                return;

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime > EditSymbolCoolDown)
            {
                elapsedTime = 0;
                isEditSymbolShown = !isEditSymbolShown;               
            }
            SetText(isEditSymbolShown);
        }

        protected void SetText(bool showEditSymbol)
        {
            Text = $"{Question} {Answer}" + (showEditSymbol ? EditSymbol : "");
        }

        protected void DecodeInput(KeyEvent args)
        {
            if (args.IsOnlyUp(Keys.Back) && Answer.Length > 0)
            {
                Answer = Answer.Substring(0, Answer.Length - 1);
                return;
            }

            if (args.UpKeys != null && args.UpKeys.Count > 0)
            {
                foreach (var key in args.UpKeys)
                {
                    if (IsValidCharacter(key, args))
                    {
                        char character = ConvertKeyToChar(key, args);
                        Answer += character;
                    }
                }
            }
        }

        protected bool IsValidCharacter(Keys key, KeyEvent args)
        {
            return (key >= Keys.A && key <= Keys.Z) ||
                   (key >= Keys.D0 && key <= Keys.D9) ||
                   key == Keys.Space;
        }

        protected char ConvertKeyToChar(Keys key, KeyEvent args)
        {

            if (key >= Keys.A && key <= Keys.Z)
            {
                return (char)key;
            }

            if (key >= Keys.D0 && key <= Keys.D9)
            {
                return (char)('0' + (key - Keys.D0));
            }

            if (key == Keys.Space)
            {
                return ' ';
            }

            return '\0';
        }
    }
}
