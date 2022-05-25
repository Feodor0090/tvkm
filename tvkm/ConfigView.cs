using System.Reflection;
using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

public class ConfigView : ListScreen<App>
{
    public ConfigView() : base("Конфигурация")
    {
    }

    public override void OnEnter(ScreenStack<App> stack)
    {
        ConfigManager.ReadConfig();
        base.OnEnter(stack);
        Clear();
        var fields = typeof(Settings).GetFields();
        foreach (var field in fields)
        {
            Add(new EditableConfigLine(field));
        }

        Add(new Button<App>("Сохранить всё в файл конфигурации", ConfigManager.SaveConfig));
        Add(new Button<App>("Сброс", () =>
        {
            ConfigManager.CreatePlaceholderConfig();
            while (!stack.Empty)
                stack.Back();
        }));
    }

    protected class EditableConfigLine : Label<App>
    {
        private readonly FieldInfo _field;

        public EditableConfigLine(FieldInfo field) : base($"{field.Name}: {field.GetValue(null)?.ToString() ?? "null"}")
        {
            _field = field;
        }

        public override void HandleKey(InputEvent e, ScreenStack<App> stack)
        {
            if (e.Action == InputAction.Activate)
            {
                stack.Push(new TextboxPopup<App>("Редактирование настройки", _field.Name,
                    _field.GetValue(null)?.ToString() ?? "null",
                    s =>
                    {
                        var dict = new Dictionary<string, string>();
                        dict.Add(_field.Name, s);
                        Settings.Apply(dict);
                        Text = $"{_field.Name}: {_field.GetValue(null)?.ToString() ?? "null"}";
                        stack.Back();
                    }));
            }
        }
    }
}