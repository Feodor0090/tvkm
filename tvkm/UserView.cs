using tvkm.Api;
using tvkm.Dialogs;
using tvkm.UIEngine;
using tvkm.UIEngine.Templates;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using Button = tvkm.UIEngine.Controls.Button;

namespace tvkm;

public class UserView : LongLoadingListScreen
{
    private readonly VkUser _user;
    private readonly VkApi _api;

    public UserView(VkUser user, VkApi api) : base($"{user.Name} (id{user.Id})")
    {
        _user = user;
        _api = api;
    }

    protected override void Load(ScreenStack stack)
    {
        Add(new Button("Посмотреть аватар", () =>
        {
            try
            {
                var user = _api.Users.Get(new long[] {_user.Id}, ProfileFields.All).FirstOrDefault();
                if (user == null)
                {
                    stack.Push(new AlertPopup("Сбой запроса URL.", stack));
                    return;
                }

                string url = user.PhotoMaxOrig.AbsoluteUri;
                ExternalUtils.TryViewPhoto(url, stack);
            }
            catch
            {
                stack.Push(new AlertPopup("Аватарка отвалилась. Может, профиль закрыт?", stack));
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