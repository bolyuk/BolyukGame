using System.Net.WebSockets;
using System.Net;
using System.Threading.Tasks;
using BolyukGame.Shared;

namespace BolyukGame.GameHandling
{
    public class ServerController : IGameController
    {
        public override async void tryStartSessionAsync()
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:5000/ws/");
            httpListener.Start();
            Logger.l($"WebSocket server started at ws://localhost:{C.server_port}/ws/");

            while (true)
            {
                var context = await httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    var webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;
                    Logger.l("Client connected.");

                    handler = new WebSocketHandler(webSocket);

                    _ = Task.Run(async () =>
                    {
                        while (webSocket.State == WebSocketState.Open)
                        {
                            var message = await handler.ReceiveMessageAsync();

                            if (message == null)
                                return;

                            OnQueryRecieved(ByteUtils.Deserialize<Request>(message));
                        }
                    });
                }
            }
        }

        private void OnQueryRecieved(Request update)
        {
            handler.SendMessageAsync(ByteUtils.Serialize(this.queryWork(update)));
        }
    }
}
   