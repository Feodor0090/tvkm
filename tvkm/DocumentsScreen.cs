using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

public class DocumentsScreen : LongLoadingListScreen<App>
{
    public DocumentsScreen() : base("Документы")
    {
    }

    protected override void Load(ScreenStack<App> stack)
    {
        var docs = stack.MainScreen.Api.Docs.Get();
        //Add(new Button<App>("Загрузить", () => stack.Alert("Не сделано пока")));

        foreach (var doc in docs)
        {
            var s = doc.Size ?? 0;
            var size = s switch
            {
                < 4096 => $"{s}B",
                < 4096 * 1024 => $"{s / 1024}KB",
                _ => $"{s / 1024 / 1024}MB"
            };
            Add(new LinkLabel($"{doc.Title} ({size})", st =>
            {
                ExternalUtils.SaveFile(doc.Title, doc.Uri);
                st.Alert("Файл загружен в рабочую директорию.");
            }));
        }
    }
}