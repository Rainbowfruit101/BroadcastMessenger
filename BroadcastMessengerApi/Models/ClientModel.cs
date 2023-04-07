using System.Net;
using System.Net.Sockets;

namespace BroadcastMessengerApi.Models;

public class ClientModel
{
    public string Name { get; set; }
    public TcpClient TcpClient { get; set; }
    public IPEndPoint IPEndPoint { get; set; }
}