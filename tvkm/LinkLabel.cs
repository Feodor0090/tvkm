using tvkm.UIEngine;
using tvkm.UIEngine.Controls;

namespace tvkm;

public class LinkLabel : Label<App>
{
    private readonly Action<ScreenStack<App>> _action;

    public LinkLabel(string text, Action<ScreenStack<App>> action) : base(text)
    {
        _action = action;
    }

    public override void HandleKey(InputEvent e, ScreenStack<App> stack)
    {
        if (e.Action == InputAction.Activate)
            _action.Invoke(stack);
    }
}