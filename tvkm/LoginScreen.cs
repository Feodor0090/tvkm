using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

internal sealed class LoginScreen : ListScreen<App>
{
    public LoginScreen(ScreenStack<App> stack, App app) : base("Вход в аккаунт VK")
    {
        var login = new TextField<App>("Логин");
        var password = new TextField<App>("Пароль") {ShowChars = false};
        Add(login);
        Add(password);
        Add(new Button<App>("Далее", () =>
        {
            try
            {
                var error = app.AuthByPassword(login.Text, password.Text);
                if (error != null)
                {
                    stack.Push(new AlertPopup<App>(error, stack));
                    return;
                }

                stack.BackThenPush(app);
            }
            catch (OperationCanceledException)
            {
                // do nothing
            }
            catch
            {
                stack.Push(new AlertPopup<App>("Не удалось войти.", stack));
            }
        }));
    }
}