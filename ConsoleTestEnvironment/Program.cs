using BroadcastMessengerApi;


var netInterfaceSelector = new NetInterfaceSelector();
var addr = netInterfaceSelector.CheckSelfIp();
var count = 0;
foreach (var address in addr)
{
    Console.WriteLine(
        $"IP Address {count}: {address.LocalEndPoint.Address.AddressFamily}, {address.LocalEndPoint.Address}");
    count++;
}

Console.WriteLine("Выберите интерфейс");
var input = Console.ReadLine();
if (!int.TryParse(input, out var index))
{
    return;
}

var range = addr[index].GetEndPointRange();
var clientFinder = new ClientFinder(addr[index]);
var clients = await clientFinder.FindAll();