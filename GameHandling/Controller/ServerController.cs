using System.Net.WebSockets;
using System.Net;
using System.Threading.Tasks;
using BolyukGame.Shared;
using System;
using System.Threading;

namespace BolyukGame.GameHandling
{
    public class ServerController : IGameController
    {
        public override async void TryStartSessionAsync(string ip)
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add($"http://{ip}:{C.server_port}/ws/");
            httpListener.Start();
            Logger.l($"WebSocket server started at ws://{ip}:{C.server_port}/ws/");

            try
            {
                while (httpListener.IsListening)
                {
                    try
                    {
                        HttpListenerContext context;
                        try
                        {
                           context = await httpListener.GetContextAsync();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            return;
                        }

                        // Проверяем, является ли запрос WebSocket
                        if (context.Request.IsWebSocketRequest)
                        {
                            // Принимаем WebSocket
                            var webSocketContext = await context.AcceptWebSocketAsync(null);
                            var webSocket = webSocketContext.WebSocket;

                            Logger.l("Client connected.");

                            // Создаём обработчик WebSocket
                            handler = new WebSocketHandler(webSocket);

                            // Асинхронно обрабатываем соединение
                            _ = HandleWebSocketConnectionAsync(webSocket);
                        }
                        else
                        {
                            // Если запрос не WebSocket, возвращаем 400 Bad Request
                            context.Response.StatusCode = 400;
                            context.Response.Close();
                        }
                    }
                    catch (HttpListenerException ex) when (ex.ErrorCode == 995)
                    {
                        // HttpListener закрыт (например, вызван Close)
                        Logger.l("HttpListener stopped.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Logger.l($"Error processing request: {ex.Message}");
                    }
                }
            }
            finally
            {
                httpListener.Close();
                Logger.l("HttpListener closed.");
            }
        }

        private async Task HandleWebSocketConnectionAsync(WebSocket webSocket)
        {
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    try
                    {
                        // Получаем сообщение от клиента
                        var message = await handler.ReceiveMessageAsync();

                        if (message == null)
                        {
                            Logger.l("Client disconnected.");
                            break;
                        }

                        // Обрабатываем сообщение
                        var request = ByteUtils.Deserialize<Request>(message);
                        OnQueryRecieved(request);
                    }
                    catch (WebSocketException ex)
                    {
                        Logger.l($"WebSocket error: {ex.Message}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Logger.l($"Error processing WebSocket message: {ex.Message}");
                    }
                }
            }
            finally
            {
                if (webSocket?.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
                }

                webSocket?.Dispose();
                Logger.l("WebSocket connection closed.");
            }
        }

        private void OnQueryRecieved(Request update)
        {
            handler.SendMessageAsync(ByteUtils.Serialize(this.QueryWork(update)));
        }
    }
}
   