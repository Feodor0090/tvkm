using tvkm.Api;
using tvkm.UIEngine;
using tvkm.UIEngine.Templates;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using static System.Console;

namespace tvkm;

public sealed class FriendsList : LongLoadingListScreen<App>
{
    public FriendsList() : base("Друзья")
    {
    }

    /// <inheritdoc />
    protected override void Load(ScreenStack<App> stack)
    {
        var api = stack.MainScreen.Api;
        var list = api.Friends.Get(new FriendsGetParams
        {
            Count = 100,
            UserId = api.UserId ?? 0,
            Fields = ProfileFields.All,
        });
        AddRange(list.Select(x => new FriendItem(new VkUser(x))));
    }

    private sealed class FriendItem : IItem<App>
    {
        public FriendItem(VkUser user)
        {
            _user = user;
        }

        private readonly VkUser _user;

        public void Draw(bool selected)
        {
            ForegroundColor = selected ? Settings.SelectionColor : Settings.DefaultColor;
            Write("  ");
            Write(_user.Name);
        }

        public void HandleKey(InputEvent e, ScreenStack<App> stack)
        {
            if (e.Action == InputAction.Activate)
            {
                stack.Push(new UserView(_user));
            }
        }

        public int Height => 1;
    }
}