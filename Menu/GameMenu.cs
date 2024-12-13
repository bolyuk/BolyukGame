using BolyukGame.GameHandling.DataContainer.DataContainers;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI;
using BolyukGame.UI.Animation;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using BolyukGame.Communication.Controller;
using BolyukGame.Communication.DataContainer;
using BolyukGame.GameHandling.Listeners.Game;

namespace BolyukGame.Menu
{
    public class GameMenu : IMenu
    {

        private int BlockSize = 50;

        private PlayerContainer Player => GameState.CurrentLobby.PlayersList.Where(p => p.Id == GameState.Config.UserId).FirstOrDefault();

        private UILoadingLabel loading = new UILoadingLabel()
        {
            Text = "Loading",
            IsSelectable = false,
            TextScale = 2f,
            TextColor = Color.Black,
            PositionPolicy = new StickyPositionPolicy()
            {
                Horizontal = StickyPosition.Center,
                Vertical = StickyPosition.Center,
            },
            AnimationPolicy = new OscillatingTransformAnimation()
        };

        private List<PlayerContainer> Players = new List<PlayerContainer>();

        private LobbyInfoExtended Lobby => GameState.CurrentLobby;

        private IGameController Controller => GameState.Controller;

        private bool IsAdmin = false;
        private bool IsLoaded = false;

        public GameMenu()
        {
            RegUI(loading);
            IsAdmin = Controller is ServerController;

            if (IsAdmin)
            {
                Controller.SetListener(new GameServerListener() { Menu = this });

                Controller.SendQuery(new Request(Lobby.Id)
                {
                    Type = RequestType.Ready,
                    Body = ByteUtils.Serialize(GameState.Config.UserId),
                });
            }
            else
            {
                Controller.SetListener(new GamePlayerListener() { Menu = this });
            }

            Controller.SendQuery(new Request(Lobby.Id)
            {
                Type = RequestType.MapGet,
                Body = ByteUtils.Serialize(GameState.Config.UserId),
            });
        }

        public void OnLoaded()
        {
            UnRegUI(loading);
            IsLoaded = true;
            var game = new UIGameDrawer()
            {
                OnFocusBackground=Color.Transparent,
                OnFocusFadedBackground=Color.Transparent,
                Lobby = Lobby,
                PositionPolicy = new StickyPositionPolicy() { Horizontal = StickyPosition.Center, Vertical = StickyPosition.Center },
            };
            RegUI(game);
            game.ForceOnParentResized();
        }

        //both side
        public virtual void AcceptQuery(Answer answer)
        {
            if (answer == null)
                return;

            if (answer.Type == AnswerType.GameStart)
            {
                OnLoaded();
            }

            if (answer.Type == AnswerType.PlayerPosContainer)
            {
                var data = ByteUtils.Deserialize<PlayerPosContainer>(answer.Body);

                var p = Lobby.PlayersList.Where(p => p.Id == data.PlayerId).FirstOrDefault();

                if (p == null)
                    return;

                p.Position = new Vector2(data.PositionX, data.PositionY);
            }

            if (answer.Type == AnswerType.MapSend && !IsAdmin)
            {
                var data = ByteUtils.Deserialize<LobbyInfoExtended>(answer.Body);

                if (data == null)
                    return;

                GameState.CurrentLobby = data;
                Controller.SendQuery(new Request(Lobby.Id)
                {
                    Type = RequestType.Ready,
                    Body = ByteUtils.Serialize(GameState.Config.UserId),
                });
            }
        }

        //both side
        public override bool CatchKeyEvent(KeyEvent args)
        {
            Vector2 direction=Vector2.Zero;

            if (args.IsDown(Keys.Up))
                direction += new Vector2(0, -0.5f);


            if (args.IsDown(Keys.Down))
                direction += new Vector2(0, 0.5f);


            if (args.IsDown(Keys.Left))
                direction += new Vector2(-0.5f, 0);


            if (args.IsDown(Keys.Right))
                direction += new Vector2(0.5f, 0);


            if (direction != Vector2.Zero)
                TryMove(direction);
            

            return direction != Vector2.Zero;
        }

        //server side
        public virtual void OnPlayerLeave(PlayerContainer player)
        {

        }

        public virtual void TryMove(Vector2 direction)
        {
            var newPos = Player.Position+direction;

            if(newPos.X < 1)
                newPos.X = 1;

            if(newPos.Y < 1)
                newPos.Y = 1;

            if(newPos.X > Lobby.Map.Width-2)
                newPos.X = Lobby.Map.Width - 2;

            if(newPos.Y > Lobby.Map.Height-3)
                newPos.Y = Lobby.Map.Height-3;

            Player.Position = newPos;
            NotifyPosChanged();
        }

        //server side
        public virtual Answer QueryWork(Request request)
        {
            if (request.Type == RequestType.Ready)
            {
                var pId = ByteUtils.Deserialize<Guid>(request.Body);

                var p = Lobby.PlayersList.Where(p => p.Id == pId).FirstOrDefault();

                if (p == null)
                    return null;

                Players.Add(p);

                if (Players.Count == Lobby.PlayersList.Count)
                {
                    var server = Controller as ServerController;

                    var answer = new Answer(Lobby.Id)
                    {
                        Type = AnswerType.GameStart,
                    };

                    server.Broadcast(answer);
                    OnLoaded();
                    return answer;
                }
            }
            else if (request.Type == RequestType.PosChanged)
            {
                var data = ByteUtils.Deserialize<PlayerPosContainer>(request.Body);

                var p = Lobby.PlayersList.Where(p => p.Id == data.PlayerId).FirstOrDefault();

                if (p == null)
                    return null;

                p.Position = new Vector2(data.PositionX, data.PositionY);

                var answer = new Answer(Lobby.Id)
                {
                    Type = AnswerType.PlayerPosContainer,
                    Body = request.Body,
                };

                var server = Controller as ServerController;
                server.Broadcast(answer);
                return answer;
            }
            else if (request.Type == RequestType.MapGet)
            {
                var answer = new Answer(Lobby.Id)
                {
                    Type = AnswerType.MapSend,
                    Body = ByteUtils.Serialize(Lobby),
                };
                var server = Controller as ServerController;
                server.Broadcast(answer);
                return answer;
            }


            return null;
        }

        private void NotifyPosChanged()
        {
            GameState.Controller.SendQuery(new Request(Lobby.Id)
            {
                Type = RequestType.PosChanged,
                Body = ByteUtils.Serialize(new PlayerPosContainer()
                {
                    PlayerId = GameState.Config.UserId,
                    PositionX = Player.Position.X,
                    PositionY = Player.Position.Y,
                })
            });
        }
    }
}
