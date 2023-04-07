using System.Net.Sockets;
using System.Text;
using BroadcastMessengerApi.Models.Messages;
using Newtonsoft.Json;

namespace BroadcastMessengerApi.Utils;

public static class MessageUtils
{
    private const string MessageEndMarker = @"\e";
    private static byte[] EndMarker => Encoding.GetBytes(MessageEndMarker);
    private static readonly Encoding Encoding = Encoding.UTF8;
    
    public static void SendMessage<TMessage>(TcpClient target, TMessage data)
        where TMessage: MessageBase
    {
        var json = JsonConvert.SerializeObject(data);
        var messageBytes = Encoding.GetBytes(json)
            .Concat(EndMarker)
            .ToArray();
        
        target.Client.Send(messageBytes);
    }

    public static bool TryReceiveMessage<TMessage>(TcpClient target, out TMessage data)
    {
        var json = ReceiveJson(target);
        try
        {
            data = JsonConvert.DeserializeObject<TMessage>(json);
            return data != null;
        }
        catch (Exception e)
        {
            data = default;
            return false;
        }
    }

    private static string ReceiveJson(TcpClient target)
    {
        var allBytes = new List<byte>();
        while (target.Client.Available > 0)
        {
            var buffer = new byte[1024];
            var accepted = target.Client.Receive(buffer);
            buffer = buffer.Take(accepted).ToArray();
            allBytes.AddRange(buffer);

            if (allBytes.EndWith(EndMarker))
            {
                allBytes = allBytes.Take(allBytes.Count - EndMarker.Length).ToList();
                break;
            }
        }
        return Encoding.GetString(allBytes.ToArray());
    }
}