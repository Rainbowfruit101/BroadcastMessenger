using Newtonsoft.Json;

namespace BroadcastMessengerApi.Models.Messages;

public class ReceivedMessage : MessageBase
{
    [JsonProperty("Type")] private string _type;

    [JsonIgnore] public override string Type => _type;
}