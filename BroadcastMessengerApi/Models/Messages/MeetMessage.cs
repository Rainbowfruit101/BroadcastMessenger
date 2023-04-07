namespace BroadcastMessengerApi.Models.Messages;

public class MeetMessage: MessageBase
{
    public const string TypeConst = "Meet";
    
    public override string Type => TypeConst;

    public string Name { get; set; }
}