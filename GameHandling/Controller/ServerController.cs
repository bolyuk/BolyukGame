using BolyukGame.GameHandling.Container;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BolyukGame.GameHandling
{
    public class ServerController : IGameController
    {
        private WebSocketServer server;
        private Dictionary<IWebSocketConnection, PlayerContainer> clients = new Dictionary<IWebSocketConnection, PlayerContainer>();
        private IServerGameListener listener;

        public override void SendQuery(Request update)
        {
            listener.acceptQuery(listener.QueryWork(update));
        }

        public override void StartSession(string ip)
        {

            server = new WebSocketServer($"ws://0.0.0.0:{C.server_port}");

            server.Start(socket =>
            {

                socket.OnClose += () =>
                {
                    if (clients.TryGetValue(socket, out var client))
                    {
                        listener.OnPlayerLeave(client);
                        clients.Remove(socket);
                    }
                        
                };

                socket.OnBinary += (msg) =>
                {
                    var parsed = ByteUtils.Deserialize<Request>(msg);

                    if (parsed == null)
                    {
                        socket.Close();
                        return;
                    }

                    if (!clients.ContainsKey(socket))
                    {
                        if(parsed.Type != RequestType.Join)
                        {
                            socket.Close();
                            return;
                        }

                        var data = ByteUtils.Deserialize<PlayerContainer>(parsed.Body);

                        if (data == null)
                        {
                            socket.Close();
                            return;
                        }

                        clients.Add(socket, data);
                        listener.OnPlayerReqistered(data);
                    } 
                    else
                    {
                        listener.QueryWork(parsed);
                    }                    
                };
            });
        }

        public override void StopSession()
        {
            foreach (var client in clients)
            {
                client.Key.Close();
            }
            clients.Clear();
            server.Dispose();
            server = null;
           
        }

        public void Broadcast(Answer request)
        {
            var parsed = ByteUtils.Serialize(request);
            foreach (var client in clients)
            {
                client.Key.Send(parsed);
            }
        }

        public override void SetListener(IGameListener listener)
        {
            this.listener = listener as IServerGameListener;
        }
    }

    public class ServerPlayerContainer
    {
        public PlayerContainer Player;
        public IWebSocketConnection socket;
    }
}
