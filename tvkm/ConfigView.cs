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
        ConfigManager.ReadSettings();
        base.OnEnter(stack);
        
        Clear();
        var fields = typeof(Settings).GetFields();
        foreach (var field in fields)
        {
            Add(new Label<App>($"{field.Name}: {field.GetValue(null)?.ToString() ?? "<NULL>"}"));
        }

        Add(new Label<App>(""));
        Add(new Label<App>("Пока только для чтения. Редактируйте config.txt"));
    }
}