using tvkm.UIEngine.Controls;

namespace tvkm.UIEngine.Templates;

/// <summary>
/// List screen with a text field and "submit" button.
/// </summary>
/// <typeparam name="T">"Main" screen of your application.</typeparam>
public sealed class TextboxPopup<T> : ListScreen<T> where T : IScreen<T>
{
    private readonly TextField<T> _field;
    private readonly Action<string> _onSubmit;
    private readonly Action? _onCancel;

    public TextboxPopup(string title, Action<string> onSubmit, Action onCancel, string fieldTitle,
        string fieldContent = "", string buttonText = "Продолжить") : base(title)
    {
        Add(_field = new TextField<T>(fieldTitle, fieldContent));
        Add(new Button<T>(buttonText, Submit));
        _onSubmit = onSubmit;
        _onCancel = onCancel;
    }

    public TextboxPopup(string title, string fieldTitle, Action<string> onSubmit) : base(title)
    {
        Add(_field = new TextField<T>(fieldTitle));
        Add(new Button<T>("Продолжить", Submit));
        _onSubmit = onSubmit;
    }

    private void Submit()
    {
        _onSubmit(_field.Text);
    }

    public override void OnLeave()
    {
        _onCancel?.Invoke();
        base.OnLeave();
    }
}