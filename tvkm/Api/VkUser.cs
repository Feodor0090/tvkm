using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace tvkm.Api;

public readonly struct VkUser
{
    private static readonly Dictionary<long, string> NamesCache = new();

    public static string GetName(long id, VkApi api)
    {
        if (NamesCache.TryGetValue(id, out var name)) return name;
        try
        {
            switch (id)
            {
                case > 0:
                {
                    IEnumerable<User> users = api.Users.Get(new long[] { id });
                    return !users.Any() ? "UNKNOWN_USER" : new VkUser(users.First()).Name;
                }
                case < 0:
                {
                    IEnumerable<Group> groups = api.Groups.GetById(null, id.ToString(), GroupsFields.Status);
                    return !groups.Any() ? "UNKNOWN_GROUP" : new VkUser(groups.First()).Name;
                }
                default:
                    return "ZERO_ID";
            }
        }
        catch
        {
            return "ERR";
        }
    }

    public static VkUser Get(long id, VkApi api)
    {
        return new VkUser(id, GetName(id, api));
    }

    public void Cache()
    {
        if (!NamesCache.ContainsKey(Id) && Id != 0) NamesCache.Add(Id, Name);
    }

    public VkUser(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public VkUser(User u)
    {
        Id = u.Id;
        if (string.IsNullOrWhiteSpace(u.LastName))
            Name = u.FirstName;
        else
            Name = u.FirstName + " " + u.LastName[0] + '.';
        Cache();
    }

    public VkUser(Group u)
    {
        Id = -u.Id;
        Name = u.Name;
        Cache();
    }

    public readonly long Id;
    public readonly string Name;


    public static VkUser[] ToUsers(IEnumerable<User>? people, IEnumerable<Group>? groups)
    {
        var pc =
            people?.Select(x => new VkUser(x)) ?? Array.Empty<VkUser>();
        var gc =
            groups?.Select(x => new VkUser(x)) ?? Array.Empty<VkUser>();
        return pc.Concat(gc).ToArray();
    }

    public static IEnumerable<(VkUser, T)> MapObjectsWithUsers<T>(IEnumerable<T> input,
        IEnumerable<VkUser> users, Func<T, int> idGetter)
    {
        return input.Select(x => (users.FirstOrDefault(u => u.Id == idGetter(x)), x));
    }
}