using tvkm.Api;
using tvkm.UIEngine;
using tvkm.UIEngine.Templates;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using static System.Console;

namespace tvkm;

public sealed class FriendsList : LongLoadingListScreen
{
    public FriendsList(VkApi api) : base("Друзья")
    {
        _api = api;
    }

    private readonly VkApi _api;

    /// <inheritdoc />
    protected override void Load(ScreenStack stack)
    {
        var list = _api.Friends.Get(new FriendsGetParams
        {
            Count = 100,
            UserId = _api.UserId ?? 0,
            Fields = ProfileFields.All,
        });
        AddRange(list.Select(x => new FriendItem(new VkUser(x), _api)));
    }

    private sealed class FriendItem : IItem
    {
        public FriendItem(VkUser user, VkApi api)
        {
            _user = user;
            _api = api;
        }

        private readonly VkUser _user;
        private readonly VkApi _api;

        public void Draw(bool selected)
        {
            ForegroundColor = selected ? Settings.SelectionColor : Settings.DefaultColor;
            Write("  ");
            Write(_user.Name);
        }

        public void HandleKey(InputEvent e, ScreenStack stack)
        {
            if (e.Action == InputAction.Activate)
            {
                stack.Push(new UserView(_user, stack.Hub, _api));
            }
        }

        public int Height => 1;
    }
}