namespace tvkm.UIEngine.Controls;

public interface ILabel
{
    string Text { get; set; }
    char? LeftBorderChar { get; set; }
    char? RightBorderChar { get; set; }
}