using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using BolyukGame.UI;
using BolyukGame.Shared;
using Microsoft.Xna.Framework;

namespace BolyukGame.Menu
{
    public class SettingsMenu : IMenu
    {
        public SettingsMenu() {
            var info = new UILabel()
            {
                Text = "[Settings]",
                IsSelectable = false,
                Padding = new int[4] { 0, 10, 10, 0 },
                PositionPolicy = new StickyPositionPolicy() { Horizontal = StickyPosition.Right },
            };

            RegUI(info);

            var back_but = new UILabel()
            {
                Text = "<- Back",
                Padding = new int[4] { 10, 10, 0, 0 }
            };

            back_but.OnClick += (e) =>
            {
                GameState.Game.NavigateTo(new MainMenu());
            };

            RegUI(back_but);

            var list = new UIList()
            {
                StartY = back_but.LogicalHeight + 40,
                Padding = new int[] { 10, 0, 0, 0 },
            };

            var question = new UIEditLable() { Question = "Name:", Answer=GameState.Config.UserName };

            list.AddElement(question);

            var create_but = new UILabel()
            {
                Text = "Save",
                Padding = new int[4] { 0, 0, 10, 10 },
                PositionPolicy = new StickyPositionPolicy() { Horizontal = StickyPosition.Right, Vertical = StickyPosition.Bottom },
            };

            RegUI(create_but);

            create_but.OnClick += (e) =>
            {
                var name = question.Answer;
                if (name == null || name.Length < 3)
                {
                    ShowSimpleToast("Name is too short!");
                    return;
                }

                if (name.Length > 10)
                {
                    ShowSimpleToast("Name is too long!");
                    return;
                }
                GameState.Config.UserName = name;
                ShowSimpleToast("Saved!", Color.Green);
            };


            RegUI(list);
            Focus(back_but);
        }
    }
}
