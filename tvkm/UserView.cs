using tvkm.Api;
using tvkm.Dialogs;
using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;
using VkNet;

namespace tvkm;

public class UserView : LongLoadingListScreen
{
    private readonly VkUser _user;
    private readonly ScreenHub _hub;
    private readonly VkApi _api;

    public UserView(VkUser user, ScreenHub hub, VkApi api) : base($"{user.Name} (id{user.Id})")
    {
        _user = user;
        _hub = hub;
        _api = api;
    }

    protected override void Load(ScreenStack stack)
    {
        Add(new Button("Открыть переписку", () =>
        {
            DialogsScreen d;
            _hub.Push(d = new DialogsScreen(_api));
            d.OpenDialog(_user.Id, _user.Name);
            d.Focus = DialogsScreen.FocusedSection.InputField;
        }));
    }
}