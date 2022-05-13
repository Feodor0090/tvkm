using System.Diagnostics;
using tvkm.UIEngine;

namespace tvkm;

public static class Program
{
    private static readonly ScreenHub hub = new ScreenHub(new DefaultControlsScheme());

    static void Main(string[] args)
    {
        ConfigManager.ReadSettings();
        InitializeTab(0);   
        hub.Loop();
    }

    static void InitializeTab(int tab)
    {
        ScreenStack stack = hub[tab];
        stack.ClearThenPush(new StartupScreen(stack, new App(stack)));
    }
}