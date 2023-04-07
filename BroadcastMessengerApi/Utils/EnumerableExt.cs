namespace BroadcastMessengerApi.Utils;

public static class EnumerableExt
{
    public static bool EndWith<T>(this IEnumerable<T> source, IReadOnlyCollection<T> end) => source
        .TakeLast(end.Count)
        .SequenceEqual(end);
}