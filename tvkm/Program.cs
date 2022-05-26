using tvkm.Servers;
using tvkm.UIEngine;

namespace tvkm;

public static class Program
{
    /// <summary>
    /// Singletone hub instance.
    /// </summary>
    private static readonly ScreenHub<App> Hub = new(new DefaultControlsScheme());

    static void Main(string[] args)
    {
        ConfigManager.ReadConfig();
        InitializeTab(0);
        Hub.Loop();
    }

    /// <summary>
    /// Resets a tab and opens startup screen there.
    /// </summary>
    /// <param name="tab">Tab index.</param>
    static void InitializeTab(int tab)
    {
        ScreenStack<App> stack = Hub[tab];
        stack.ClearThenPush(new StartupScreen(stack, new App(stack, new OpenvkSuServer())));
    }
}