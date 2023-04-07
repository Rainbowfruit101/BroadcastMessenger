using System.Net;

namespace BroadcastMessengerApi.Models;

public class NetInterfaceConfig
{
    public string HostName { get; init; }
    public IPEndPoint LocalEndPoint { get; init; }

    public IPEndPoint[] GetEndPointRange()
    {
        var ipString = LocalEndPoint.Address.ToString();
        var separatorIndex = ipString.LastIndexOf('.');
        var substringIp = ipString.Substring(0, separatorIndex);
        
        var ipEndPoints = new List<IPEndPoint>();
        for (var i = 2; i < 255; i++)
        {
            ipEndPoints.Add(
                new IPEndPoint(
                    IPAddress.Parse($"{substringIp}.{i}"),
                    LocalEndPoint.Port
                )
            );
        }

        return ipEndPoints.ToArray();
    }
}