using System.Net;
using System.Net.Sockets;
using BroadcastMessengerApi.Models;

namespace BroadcastMessengerApi;

public class NetInterfaceSelector
{
    private const int Port = 5555;
    public NetInterfaceConfig[] CheckSelfIp()
    {
        var strHostName = Dns.GetHostName();
        Console.WriteLine("Local Machine's Host Name: " + strHostName);

        var ipEntry = Dns.GetHostEntry(strHostName);
        var addr = ipEntry.AddressList
            .Where(address => address.AddressFamily == AddressFamily.InterNetwork)
            .Select(address => new NetInterfaceConfig()
            {
                HostName = strHostName,
                LocalEndPoint = new IPEndPoint(address, Port)
            })
            .ToArray();

        return addr;
    }
}