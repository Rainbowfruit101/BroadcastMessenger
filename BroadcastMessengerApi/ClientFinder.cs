using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using BroadcastMessengerApi.Models;
using BroadcastMessengerApi.Models.Messages;
using BroadcastMessengerApi.Utils;

namespace BroadcastMessengerApi;

public class ClientFinder
{
    private const int Timeout = 1000;
    
    private readonly NetInterfaceConfig _netInterfaceConfig;
    

    public ClientFinder(NetInterfaceConfig netInterfaceConfig)
    {
        _netInterfaceConfig = netInterfaceConfig;
    }
    
    public async Task<IEnumerable<ClientModel>> FindAll()
    {
        var result = new List<ClientModel>();
        
        using var ping = new Ping();
        foreach (var endPoint in _netInterfaceConfig.GetEndPointRange())
        {
            var pingReply = await ping.SendPingAsync(endPoint.Address, Timeout);
            if (pingReply.Status == IPStatus.Success)
            {
                if (TryGetClient(endPoint, out var client))
                {
                    result.Add(client);
                }
            }
        }

        return result;
    }

    private bool TryGetClient(IPEndPoint endPointCandidate, out ClientModel clientModel)
    {
        using var tcpClient = new TcpClient(_netInterfaceConfig.LocalEndPoint);
        tcpClient.Connect(endPointCandidate);
        MessageUtils.SendMessage(tcpClient, new MeetMessage());
        if (MessageUtils.TryReceiveMessage(tcpClient, out MeetMessage message))
        {
            clientModel = new ClientModel()
            {
                Name = message.Name,
                TcpClient = tcpClient,
                IPEndPoint = endPointCandidate
            };
            return true;
        }

        clientModel = null;
        return false;
    }
}