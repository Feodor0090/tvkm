using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

internal sealed class LoginScreen : ListScreen
{
    public LoginScreen(ScreenStack stack) : base("Вход в аккаунт VK")
    {
        var login = new TextField("Логин");
        var password = new TextField("Пароль") { ShowChars = false };
        Add(login);
        Add(password);
        Add(new Button("Далее", () =>
        {
            try
            {
                var error = Program.App.AuthByPassword(login.Text, password.Text);
                if (error != null)
                {
                    stack.Push(new AlertPopup(error, stack));
                    return;
                }

                stack.BackThenPush(Program.App);
            }
            catch (OperationCanceledException)
            {
                // do nothing
            }
            catch
            {
                stack.Push(new AlertPopup("Не удалось войти.", stack));
            }
        }));
    }
}