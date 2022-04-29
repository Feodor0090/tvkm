namespace tvkm;

public static class Settings
{
    public static string BrowserPath = "/usr/bin/w3m";
    public static string ImageViewerPath = "/usr/bin/ristretto";
    public static int LongpollTimeout = 25;
    public static int LongpollErrorPause = 5;
    public static bool SendReadEvent = true;

    public static ConsoleColor DefaultColor = ConsoleColor.White;
    public static ConsoleColor SelectionColor = ConsoleColor.Yellow;
    public static ConsoleColor AccentColor = ConsoleColor.Green;
}