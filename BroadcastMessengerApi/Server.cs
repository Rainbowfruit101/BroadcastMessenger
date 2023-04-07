using System.Net.Sockets;
using BroadcastMessengerApi.Models;
using BroadcastMessengerApi.Models.Messages;
using BroadcastMessengerApi.Utils;

namespace BroadcastMessengerApi;

public class Server
{
    class TypedHandler
    {
        public string StringType { get; init; }
        public Type Type { get; init; }
        public Action<object> Handler { get; init; }
    }
    
    private readonly NetInterfaceConfig _config;
    private readonly TcpListener _tcpListener;
    private readonly List<TypedHandler> _handlers;
    
    public Server(NetInterfaceConfig config)
    {
        _config = config;
        _tcpListener = new TcpListener(_config.LocalEndPoint);
        _handlers = new List<TypedHandler>();
    }

    public void Start()
    {
        _tcpListener.Start();
        while (true)
        {
            var client = _tcpListener.AcceptTcpClient();
            if(!MessageUtils.TryReceiveMessage(client, out ReceivedMessage message))
            {
                continue;
            }

            var handler = GetHandler(message.Type);
             
        }
    }

    public void RegisterTypedHandler<T>(string type, Action<T> handler)
        where T: MessageBase
    {
        _handlers.Add(new TypedHandler()
        {
            StringType = type,
            Type = typeof(T),
            Handler = handler as Action<object>
        });
    }

    private TypedHandler? GetHandler(string type) => _handlers.FirstOrDefault(h => h.StringType == type);
}