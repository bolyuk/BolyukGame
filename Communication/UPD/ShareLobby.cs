using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BolyukGame.GameHandling;
using BolyukGame.GameHandling.Container;
using BolyukGame.Shared;
using BolyukGame.Shared.Info;

namespace BolyukGame.Communication.UPD
{
    public class ShareLobby
    {
        private static CancellationTokenSource token_source = null;

        public static async Task ExecAsync(LobbyInfoSmall lobby)
        {
            if (token_source != null)
            {
                throw new InvalidOperationException("ShareLobby is running!");
            }

            token_source = new CancellationTokenSource();
            CancellationToken token = token_source.Token;

            using (UdpClient udpServer = new UdpClient(C.udp_port))
            {
                Logger.l("Server listening...");

                var answer = new Answer
                {
                    Type = AnswerType.ServerFound,
                    Body = ByteUtils.Serialize(lobby),
                    LobbyId = lobby.Id
                };

                var response = ByteUtils.Serialize(answer);

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
                        var receiveTask = udpServer.ReceiveAsync();

                        var result = await Task.WhenAny(receiveTask, Task.Delay(-1, token));

                        if (result != receiveTask)
                            continue; //skip

                        var request = ByteUtils.Deserialize<Request>(receiveTask.Result.Buffer);

                        if (request == null || request.Type != RequestType.ServerSearch)
                            continue; //skip

                        Logger.l($"Received: {request.ToString()}");

                        await udpServer.SendAsync(response, response.Length, receiveTask.Result.RemoteEndPoint);

                    }
                    catch (OperationCanceledException)
                    {
                        Logger.l("Server is shutting down...");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Logger.l($"Error: {ex.Message}");
                    }
                }
            }

            Logger.l("Server stopped.");
        }

        public static void Stop()
        {
            if (token_source == null)
            {
                throw new InvalidOperationException("ShareLobby is not running!");
            }

            token_source.Cancel();
            token_source = null;
        }
    }
}
