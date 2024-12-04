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
        private static CancellationTokenSource token_source;
        public static async Task ExecAsync(Action<LobbyInfo> onFind)
        {
            if (token_source != null)
            {
                throw new InvalidOperationException("FindLobby is running!");
            }

            token_source = new CancellationTokenSource();
            CancellationToken token = token_source.Token;

            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.EnableBroadcast = true;
                var broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, C.udp_port);
                var request = ByteUtils.Serialize(new Request() { type = RequestType.ServerSearch });

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        await udpClient.SendAsync(request, request.Length, broadcastEndpoint);

                        var receiveTask = udpClient.ReceiveAsync();
                        var result = await Task.WhenAny(receiveTask, Task.Delay(3000, token));

                        if (result != receiveTask)
                            continue; //skip

                        var answer = ByteUtils.Deserialize<Answer>(receiveTask.Result.Buffer);

                        if (answer == null || answer.type != AnswerType.ServerFound)
                            continue; //skip

                        Logger.l($"Server found at: {receiveTask.Result.RemoteEndPoint.Address}");

                        var lobby = ByteUtils.Deserialize<LobbyInfo>(answer.body);
                        lobby.ip = receiveTask.Result.RemoteEndPoint.Address.ToString();
                        onFind.Invoke(lobby);

                    }
                    catch (OperationCanceledException)
                    {
                        Logger.l("Client search canceled.");
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
            }
        }

        public static void Stop()
        {
            if (token_source == null)
            {
                throw new InvalidOperationException("FindLobby is not running!");
            }

            token_source.Cancel();
            token_source = null;
        }
    }
}
