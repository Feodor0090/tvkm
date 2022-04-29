namespace tvkm.Api;

public sealed class LongpollData
{
    public string? Ts { get; set; }
    public object[][]? Updates { private get; set; }

    public bool HasUpdates => Updates is { Length: > 0 };

    public LongpollUpdate[] GetUpdates()
    {
        if (!HasUpdates) return Array.Empty<LongpollUpdate>();
        var r = GC.AllocateUninitializedArray<LongpollUpdate>(Updates!.Length);
        for (var i = 0; i < Updates.Length; i++)
        {
            r[i] = new LongpollUpdate
            {
                Type = int.Parse(Updates![i][0].ToString()!),
                Data = Updates[i].Skip(1).ToArray()
            };
        }

        return r;
    }
}