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
    public class FindLobby
    {
        private static CancellationTokenSource tokenSource;
        private static Task receiveTask;

        public static async Task ExecAsync(Action<LobbyInfo> onFind)
        {
            if (tokenSource != null)
            {
                throw new InvalidOperationException("FindLobby is already running!");
            }

            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.EnableBroadcast = true;
                var broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, C.udp_port);
                var request = ByteUtils.Serialize(new Request() { Type = RequestType.ServerSearch });

                receiveTask = Task.Run(async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        try
                        {
                            var result = await udpClient.ReceiveAsync(token);
                            var answer = ByteUtils.Deserialize<Answer>(result.Buffer);

                            if (answer != null && answer.Type == AnswerType.ServerFound)
                            {
                                Logger.l($"Server found at: {result.RemoteEndPoint.Address}");

                                var lobby = ByteUtils.Deserialize<LobbyInfo>(answer.Body);
                                lobby.Ip = result.RemoteEndPoint.Address.ToString();
                                onFind?.Invoke(lobby);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            Logger.l("Receiving canceled.");
                            break;
                        }
                        catch (SocketException ex)
                        {
                            Logger.l($"Socket error: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Logger.l($"Unexpected error: {ex.Message}");
                        }
                    }
                }, token);

                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        await udpClient.SendAsync(request, request.Length, broadcastEndpoint);
                        Logger.l("Broadcast sent.");

                        await Task.Delay(1000, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger.l("Sending canceled.");
                }
                catch (Exception ex)
                {
                    Logger.l($"Unexpected error during sending: {ex.Message}");
                }
            }
        }

        public static void Stop()
        {
            if (tokenSource == null)
            {
                throw new InvalidOperationException("FindLobby is not running!");
            }

            tokenSource.Cancel();
            tokenSource = null;

            try
            {
                receiveTask?.Wait();
            }
            catch (AggregateException)
            {
                
            }

            Logger.l("FindLobby stopped.");
        }
    }
}
