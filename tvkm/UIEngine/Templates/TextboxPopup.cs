using tvkm.UIEngine.Controls;

namespace tvkm.UIEngine.Templates;

public class TextboxPopup : ListScreen
{
    private readonly TextField field;
    private readonly Action<string> _onSubmit;
    private readonly Action? _onCancel;

    public TextboxPopup(string title, Action<string> onSubmit, Action onCancel, string fieldTitle,
        string fieldContent = "", string buttonText = "Продолжить") : base(title)
    {
        Add(field = new TextField(fieldTitle, fieldContent));
        Add(new Button(buttonText, Submit));
        _onSubmit = onSubmit;
        _onCancel = onCancel;
    }

    public TextboxPopup(string title, string fieldTitle, Action<string> onSubmit) : base(title)
    {
        Add(field = new TextField(fieldTitle));
        Add(new Button("Продолжить", Submit));
        _onSubmit = onSubmit;
    }

    private void Submit()
    {
        _onSubmit(field.Text);
    }

    public override void OnLeave()
    {
        _onCancel?.Invoke();
        base.OnLeave();
    }
}