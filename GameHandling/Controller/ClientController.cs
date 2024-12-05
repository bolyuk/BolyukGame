using System;
using System.Net.WebSockets;
using System.Threading;
using BolyukGame.GameHandling.Container;
using BolyukGame.Shared;

namespace BolyukGame.GameHandling
{
    public class ClientController : IGameController
    {
        public override async void TryStartSessionAsync(string ip)
        {
            using (var webSocket = new ClientWebSocket())
            {
                await webSocket.ConnectAsync(new Uri($"ws://{ip}:{C.server_port}/ws/"), CancellationToken.None);
                Logger.l("Connected to server.");

                handler = new WebSocketHandler(webSocket);

                while (webSocket.State == WebSocketState.Open)
                {
                    var serverMessage = await handler.ReceiveMessageAsync();
                    if (serverMessage == null)
                        return;

                    this.AcceptQuery(ByteUtils.Deserialize<Answer>(serverMessage));
                }
            }
        }

        public override void SendQuery(Request update)
        {
           handler.SendMessageAsync(ByteUtils.Serialize(update));
        }
    }
}
