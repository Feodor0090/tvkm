namespace tvkm.UIEngine.Controls;

public interface ITextField
{
    bool ShowChars { get; set; }
    string Label { get; set; }
    string Text { get; set; }
    int TextLength { get; }
}