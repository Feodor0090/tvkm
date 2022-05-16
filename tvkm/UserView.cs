using tvkm.Api;
using tvkm.Dialogs;
using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;
using VkNet.Model;

namespace tvkm;

public class UserView : LongLoadingListScreen<App>
{
    private readonly VkUser _user;
    private User? fullUser;

    public UserView(VkUser user) : base($"{user.Name} (id{user.Id})")
    {
        _user = user;
    }

    protected override void Load(ScreenStack<App> stack)
    {
        var api = stack.MainScreen.Api;
        fullUser = _user.LoadFull(api.Users);
        Add(new Button<App>("Посмотреть аватар", () =>
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
        Add(new Button<App>("Открыть переписку", () =>
        {
            DialogsScreen d;
            stack.Push(d = new DialogsScreen(stack));
            d.OpenDialog(_user.Id, _user.Name);
            d.Focus = DialogsSection.InputField;
        }));
    }
}