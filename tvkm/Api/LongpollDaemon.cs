using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VkNet;
using VkNet.Model;

namespace tvkm.Api;

public class LongpollDaemon
{
    private static LongpollDaemon? _inst;
    public static bool LongpollIsOk { get; private set; }
    public static event Action<bool> LongpollStateChange = null!;
    public static event Action<LongpollMessage> OnNewMessage = null!;
    public static event Action<LongpollMessageEdit> OnMessageEdit = null!;
    public static event Action<LongpollWriteStatus> OnMessageWrite = null!;


    public LongpollDaemon(VkApi api)
    {
        _api = api;
        _inst = this;
    }

    private readonly VkApi _api;
    private bool _isRunning;
    private bool _stop;

    private HttpClient _httpClient = null!;

    public void Run()
    {
        if (_isRunning) throw new InvalidOperationException();
        Task.Factory.StartNew(LongpollLoop, TaskCreationOptions.LongRunning);
    }

    private void ReportLongpollState(bool s)
    {
        LongpollIsOk = s;
        LongpollStateChange?.Invoke(s);
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
                ReportLongpollState(true);
                string activeTs = lpp.Ts;

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

                        if (update.Type == 4)
                        {
                            OnNewMessage?.Invoke(new LongpollMessage(update, _api));
                        }
                        else if (update.Type == 5)
                        {
                            OnMessageEdit?.Invoke(new LongpollMessageEdit(update));
                        }
                        else if (update.Type == 61)
                        {
                            OnMessageWrite?.Invoke(new LongpollWriteStatus(update));
                        }
                    }
                }
            }
            catch
            {
                if (_stop) return;
                ReportLongpollState(false);
                await Task.Delay(Settings.LongpollErrorPause * 1000);
            }
        }
    }

    public void Dispose()
    {
        ReportLongpollState(false);
        _stop = true;
        _httpClient?.Dispose();
    }

    public struct LongpollMessage
    {
        public LongpollMessage(LongpollUpdate upd, VkApi api)
        {
            if (upd.Type != 4) throw new ArgumentException();
            var data = upd.Data;
            MessageId = Convert.ToInt32(data[0]);
            Flags = Convert.ToInt32(data[1]);
            TargetId = Convert.ToInt32(data[2]);
            Time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToInt32(data[3]))
                .ToLocalTime();
            Text = (string)data[4];
            Extra = new Dictionary<string, string>();
            FromId = 0;
            foreach (var dict in data.Skip(5).Cast<JObject>())
            {
                foreach (var p in dict)
                {
                    if (p.Key == "from")
                        FromId = int.Parse(p.Value?.ToString() ?? "0");
                    else if (p.Value != null)
                        Extra.Add(p.Key, p.Value.ToString());
                }
            }

            if (FromId == 0) FromId = api.UserId ?? 0;
        }

        public int MessageId;
        public int Flags;
        public int TargetId;
        public long FromId;
        public DateTime Time;
        public string Text;
        public Dictionary<string, string> Extra;

        public Message LoadFull(VkApi api)
        {
            return api.Messages.GetById(new[] { (ulong)MessageId }, Array.Empty<string>(), 0, true)[0];
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
            return api.Messages.GetById(new[] { (ulong)MessageId }, Array.Empty<string>(), 0, true)[0];
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