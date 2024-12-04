using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BolyukGame
{
    public class WebSocketHandler
    {
        private readonly WebSocket _webSocket;

        public WebSocketHandler(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public async Task SendMessageAsync(byte[] bytes)
        {
            await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<byte[]> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                return buffer;
            }

            return null;
        }

        public async Task CloseConnectionAsync()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
        }
    }
}
