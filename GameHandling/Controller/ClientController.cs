using BolyukGame.GameHandling.Container;
using BolyukGame.Shared;
using System;
using WebSocketSharp;

namespace BolyukGame.GameHandling
{
    public class ClientController : IGameController
    {
        WebSocket connection;
        private IPlayerGameListener listener;
        public override void SendQuery(Request update)
        {
            connection.Send(ByteUtils.Serialize(update));
        }

        public override void SetListener(IGameListener listener)
        {
            this.listener = listener as IPlayerGameListener;
        }

        public override void StartSession(string ip)
        {
            connection = new WebSocket($"ws://{ip}:{C.server_port}");

            connection.OnClose += (a, s) => listener.OnSessionEnds();

            connection.OnOpen += (a, s) => listener.OnSessionStarts();

            connection.OnError += (a, s) => listener.OnError(s);

            connection.OnMessage += (a, s) =>
            {
                if (s.IsBinary && s.RawData != null)
                {
                    var parsed = ByteUtils.Deserialize<Answer>(s.RawData);
                    if (parsed == null)
                        return;
                    listener.acceptQuery(parsed);
                }
            };

            connection.Connect();
        }

        public override void StopSession()
        {
            connection.Close();
            listener.OnSessionEnds();
        }
    }
}
