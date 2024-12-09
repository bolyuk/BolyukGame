using BolyukGame.Communication.UPD;
using BolyukGame.GameHandling;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;

namespace BolyukGame.Menu
{
    public class CreateLobbyMenu : IMenu
    {
        public CreateLobbyMenu()
        {
            var info = new UILabel()
            {
                Text = "[Lobby  Creating]",
                IsSelectable = false,
                Padding = new int[4] { 0, 10, 10, 0 },
                PositionPolicy = new StickyPositionPolicy() { Horizontal = StickyPosition.Right },
            };

            RegUI(info);

            var back_but = new UILabel() { 
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

            var question = new UIEditLable() { Question = "Lobby  Name:" };

            list.AddElement(question);

            var create_but = new UILabel()
            {
                Text = "Create ->",
                Padding = new int[4] { 0, 0, 10, 10 },
                PositionPolicy = new StickyPositionPolicy() { Horizontal= StickyPosition.Right, Vertical=StickyPosition.Bottom},
            };

            RegUI(create_but);

            create_but.OnClick += (e) =>
            {
                var answer = question.Answer;
                if (answer == null || answer.Length < 3)
                {
                    ShowSimpleToast("Lobby Name is too short!");
                    return;
                }

                if (answer.Length > 10)
                {
                    ShowSimpleToast("Lobby Name is too long!");
                    return;
                }


                var lobby = new LobbyInfoExtended() { 
                    PlayersCount = 1, 
                    Name = answer
                };

                ShareLobby.ExecAsync(lobby);

                var server = new ServerController();
                server.StartSession("localhost");

                GameState.Controller = server;
                GameState.CurrentLobby = lobby;
                GameState.Game.NavigateTo(new LobbyMenu());
            };


            RegUI(list);
            Focus(back_but);
        }
    }
}
