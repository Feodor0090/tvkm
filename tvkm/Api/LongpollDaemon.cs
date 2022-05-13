using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace tvkm.Api;

public class LongpollDaemon
{
    public event Action<LongpollMessage> OnNewMessage = null!;
    public event Action<LongpollMessageEdit> OnMessageEdit = null!;
    public event Action<LongpollWriteStatus> OnMessageWrite = null!;


    public LongpollDaemon(App app, VkApi api)
    {
        _app = app;
        _api = api;
    }

    private readonly App _app;
    private readonly VkApi _api;
    private bool _isRunning;
    private bool _stop;

    private HttpClient _httpClient = null!;

    public void Run()
    {
        if (_isRunning) throw new InvalidOperationException();
        Task.Factory.StartNew(LongpollLoop, TaskCreationOptions.LongRunning);
    }

    private async void LongpollLoop()
    {
        _isRunning = true;
        // loop for reconnection to longpoll server
        while (!_stop)
        {
            try
            {
                var lpp = await _api.Messages.GetLongPollServerAsync(true);
                var activeTs = lpp.Ts;

                _httpClient?.Dispose();
                _httpClient = new HttpClient();
                _httpClient.Timeout = new TimeSpan(0, 1, 0);
                // loop for handling requests
                while (!_stop)
                {
                    var response = await _httpClient.GetAsync(
                        $"https://{lpp.Server}?act=a_check&key={lpp.Key}&ts={activeTs}&wait={Settings.LongpollTimeout}&mode=2&version=3");

                    var ld = JsonConvert.DeserializeObject<LongpollData>(await response.Content.ReadAsStringAsync());
                    activeTs = ld!.Ts!;
                    if (!ld.HasUpdates) continue;

                    var updates = ld.GetUpdates();

                    if (_stop) return;
                    for (var i = 0; i < updates.Length; i++)
                    {
                        var update = updates[i];

                        switch (update.Type)
                        {
                            case 4:
                                OnNewMessage?.Invoke(new LongpollMessage(update, _app.UserId));
                                break;
                            case 5:
                                OnMessageEdit?.Invoke(new LongpollMessageEdit(update));
                                break;
                            case 61:
                                OnMessageWrite?.Invoke(new LongpollWriteStatus(update));
                                break;
                        }
                    }
                }
            }
            catch
            {
                if (_stop) return;
                await Task.Delay(Settings.LongpollErrorPause * 1000);
            }
        }
    }

    public void Dispose()
    {
        _stop = true;
        _httpClient?.Dispose();
    }

    public readonly struct LongpollMessage
    {
        public LongpollMessage(LongpollUpdate upd, long userId)
        {
            var data = upd.Data;
            MessageId = Convert.ToInt32(data[0]);
            Flags = Convert.ToInt32(data[1]);
            TargetId = Convert.ToInt32(data[2]);
            Time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToInt32(data[3]))
                .ToLocalTime();
            Text = (string) data[4];
            Extra = new Dictionary<string, string>();
            var fromId = 0;
            foreach (var dict in data.Skip(5).Cast<JObject>())
            {
                foreach (var p in dict)
                {
                    if (p.Key == "from")
                        fromId = int.Parse(p.Value?.ToString() ?? "0");
                    else if (p.Value != null)
                        Extra.Add(p.Key, p.Value.ToString());
                }
            }

            FromId = fromId != 0 ? fromId : userId;
        }

        public readonly int MessageId;
        public readonly int Flags;
        public readonly int TargetId;
        public readonly long FromId;
        public readonly DateTime Time;
        public readonly string Text;
        public readonly Dictionary<string, string> Extra;

        public Message LoadFull(VkApi api)
        {
            return api.Messages.GetById(new[] {(ulong) MessageId}, Array.Empty<string>(), 0, true)[0];
        }
    }

    public readonly struct LongpollMessageEdit
    {
        public LongpollMessageEdit(LongpollUpdate upd)
        {
            if (upd.Type != 5) throw new ArgumentException();
            var data = upd.Data;
            MessageId = Convert.ToInt32(data[0]);
            PeerId = Convert.ToInt32(data[2]);
        }

        public readonly int MessageId;
        public readonly int PeerId;

        public Message LoadFull(VkApi api)
        {
            return api.Messages.GetById(new[] {(ulong) MessageId}, Array.Empty<string>(), 0, true)[0];
        }
    }

    public readonly struct LongpollWriteStatus
    {
        public LongpollWriteStatus(LongpollUpdate upd)
        {
            PeerId = Convert.ToInt32(upd.Data[0]);
        }

        public readonly int PeerId;
    }
}