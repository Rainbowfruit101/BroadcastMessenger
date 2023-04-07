using Newtonsoft.Json;

namespace BroadcastMessengerApi.Models.Messages;

public abstract class MessageBase
{
    public abstract string Type { get; }
}