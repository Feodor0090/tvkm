using tvkm.Api;
using tvkm.Dialogs;
using tvkm.UIEngine;
using tvkm.UIEngine.Templates;
using VkNet;
using VkNet.Model;
using Button = tvkm.UIEngine.Controls.Button;

namespace tvkm;

public class UserView : LongLoadingListScreen
{
    private readonly VkUser _user;
    private readonly VkApi _api;
    private User? fullUser;

    public UserView(VkUser user, VkApi api) : base($"{user.Name} (id{user.Id})")
    {
        _user = user;
        _api = api;
    }

    protected override void Load(ScreenStack stack)
    {
        fullUser = _user.LoadFull(_api.Users);
        Add(new Button("Посмотреть аватар", () =>
        {
            try
            {
                if (fullUser == null)
                {
                    stack.Alert("Сбой получения информации о пользователе.");
                    return;
                }

                string url = fullUser.PhotoMaxOrig.AbsoluteUri;
                ExternalUtils.TryViewPhoto(url, stack);
            }
            catch
            {
                stack.Alert("Аватарка отвалилась. Может, профиль закрыт?");
            }
        }));
        Add(new Button("Открыть переписку", () =>
        {
            DialogsScreen d;
            stack.Push(d = new DialogsScreen(_api));
            d.OpenDialog(_user.Id, _user.Name);
            d.Focus = DialogsScreen.FocusedSection.InputField;
        }));
    }
}