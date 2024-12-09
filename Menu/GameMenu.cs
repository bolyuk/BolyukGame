using BolyukGame.GameHandling;
using BolyukGame.GameHandling.Container;
using BolyukGame.GameHandling.Controller.Listeners.Game;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using BolyukGame.UI.Animation;
using BolyukGame.UI.Label;
using BolyukGame.UI.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BolyukGame.Menu
{
    public class GameMenu : IMenu
    {
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

        private bool IsAdmin = false;
        private bool IsLoaded = false;

        public GameMenu()
        {
            RegUI(loading);
            IsAdmin = GameState.Controller is ServerController;

            if (IsAdmin)
            {
                GameState.Controller.SetListener(new GameServerListener() { Menu = this });
            }
            else
            {
                GameState.Controller.SetListener(new GamePlayerListener() { Menu = this });
            }

            GameState.Controller.SendQuery(new Request()
            {
                Type = RequestType.Ready,
                Body = ByteUtils.Serialize(GameState.Config.UserId),
                LobbyId = GameState.CurrentLobby.Id,
            });

        }

        public void OnLoaded()
        {
            UnRegUI(loading);
            IsLoaded = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if(!IsLoaded)
                return;
            DrawGame(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsLoaded)
                return;
        }


        //both side
        public virtual void AcceptQuery(Answer answer)
        {
            if(answer == null)
                return;

            if (answer.Type == AnswerType.GameStart)
            {
                OnLoaded();
            }
        }

        //both side
        public override bool CatchKeyEvent(KeyEvent args)
        {
            return false;
        }

        //server side
        public virtual void OnPlayerLeave(PlayerContainer player)
        {

        }

        //server side
        public virtual Answer QueryWork(Request request)
        {
            if (request.Type == RequestType.Ready)
            {
                var pId = ByteUtils.Deserialize<Guid>(request.Body);

                var p = GameState.CurrentLobby.PlayersList.Where(p => p.Id == pId).FirstOrDefault();

                if (p == null)
                    return null;

                Players.Add(p);

                if (Players.Count == GameState.CurrentLobby.PlayersList.Count)
                {
                    var server = GameState.Controller as ServerController;

                    var answer = new Answer()
                    {
                        LobbyId = GameState.CurrentLobby.Id,
                        Type = AnswerType.GameStart,
                    };

                    server.Broadcast(answer);
                    return answer;
                }
            }


            return null;
        }

        private void DrawGame(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
