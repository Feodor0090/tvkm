using System.Diagnostics;
using tvkm.UIEngine;

namespace tvkm;

public static class Program
{
    private static readonly ScreenHub hub = new ScreenHub(new DefaultControlsScheme());
    public static App App;

    static void Main(string[] args)
    {
        App = new App(hub.CurrentTab);
        ConfigManager.ReadSettings();
        hub.CurrentTab.Push(new StartupScreen(hub.CurrentTab));
        hub.Loop();
    }
}