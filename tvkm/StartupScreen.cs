using tvkm.Servers;
using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

internal sealed class StartupScreen : ListScreen<App>
{
    private IServerProvider server = new VkComServer();

    public StartupScreen(ScreenStack<App> stack) : base("TVKM")
    {
        LinkLabel l = null;
        Add(l = new LinkLabel($"Сервер: {server.ReadableName}", st =>
        {
            st.Push(new ListScreen<App>("Выбор сервера", new[]
            {
                new Button<App>("vk.com", () =>
                {
                    server = new VkComServer();
                    l.Text = $"Сервер: {server.ReadableName}";
                    st.Back();
                }),
                new Button<App>("vk.com (свой сервер)", () =>
                {
                    server = new CustomVkComServer();
                    l.Text = $"Сервер: {server.ReadableName}";
                    st.Back();
                }),
                new Button<App>("openvk", () =>
                {
                    server = new OpenvkSuServer();
                    l.Text = $"Сервер: {server.ReadableName}";
                    st.Back();
                }),
                new Button<App>("openvk (свой сервер)", () =>
                {
                    server = new CustomOpenvkServer();
                    l.Text = $"Сервер: {server.ReadableName}";
                    st.Back();
                })
            }));
        }));
        Add(new Button<App>("Восстановить сессию", () =>
        {
            if (File.Exists("session.txt"))
            {
                try
                {
                    var app = new App(stack, server);
                    app.RestoreFromFile();
                    stack.Push(app);
                }
                catch (IndexOutOfRangeException)
                {
                    stack.Alert("Сессия старого формата. Перелогиньтесь.");
                }
                catch (Exception e)
                {
                    stack.Alert(e.Message);
                }
            }
            else
                stack.Push(new AlertPopup<App>("Нет сохранённой сессии.", stack));
        }));
        Add(new Button<App>("Вход по логину/паролю", () => stack.Push(new LoginScreen(stack, new App(stack, server)))));
        Add(new Button<App>("Настройки", () => stack.Push(new ConfigView())));
        Add(new Button<App>("GitHub", () => { ExternalUtils.TryOpenBrowser("github.com/Feodor0090/tvkm", stack); }));
        Add(new Button<App>("Закрыть", stack.Back));
    }
}