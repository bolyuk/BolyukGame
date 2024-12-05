using BolyukGame.GameHandling.Container;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;
using Fleck;
using System;
using System.Collections.Generic;

namespace BolyukGame.GameHandling
{
    public class ServerController : IGameController
    {
        private WebSocketServer server;
        private List<IWebSocketConnection> clients = new List<IWebSocketConnection>();
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
                    if (clients.Contains(socket))
                        clients.Remove(socket);
                };

                socket.OnBinary += (msg) =>
                {
                    var parsed = ByteUtils.Deserialize<Request>(msg);

                    if (parsed == null)
                    {
                        socket.Close();
                        return;
                    }

                    if (!clients.Contains(socket))
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

                        clients.Add(socket);
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
            clients.ForEach(c => c.Close());
            clients.Clear();
            server.Dispose();
            server = null;
           
        }

        public void Broadcast(Answer request)
        {
            var parsed = ByteUtils.Serialize(request);
            clients.ForEach(client => client.Send(parsed));
        }

        public override void SetListener(IGameListener listener)
        {
            this.listener = listener as IServerGameListener;
        }
    }
}
