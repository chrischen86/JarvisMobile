using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jarvis.Core.Services;
using System.Threading.Tasks;
using Jarvis.Core.Models.AlphaFlight;
using Android.Graphics;

namespace Jarvis
{
    [Service]
    [IntentFilter(new String[] { "AlphaFlight.OverlayService" })]
    class OverlayService : Service
    {
        private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private IWindowManager _windowManager;
        private LinearLayout _layout;
        private StrikeTableManager _strikeTableManager;

        private bool _keepPolling = true;
        private List<Zone> _zones = new List<Zone>();
        private int _zone = 0;
        private string _userId;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            _userId = intent.GetStringExtra("UserId");
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnCreate()
        {
            base.OnCreate();
            _strikeTableManager = new StrikeTableManager();

            var param = new WindowManagerLayoutParams(
                   WindowManagerLayoutParams.WrapContent,
                   WindowManagerLayoutParams.WrapContent,
                             WindowManagerTypes.SystemAlert,
                             WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal,
                             Android.Graphics.Format.Translucent);
            param.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top;
            //param.X = 0;
            param.Y = 100;
            //param.Title = "Overlay window title";

            //_layout = new LinearLayout(this);
            //_layout.Orientation = Orientation.Vertical;

            //var checkButton = new ImageButton(this);
            //checkButton.SetImageResource(Resource.Drawable.ic_sync);
            //checkButton.Click += delegate
            //{
            //    _keepPolling = !_keepPolling;
            //    checkButton.SetImageResource(_keepPolling ? Resource.Drawable.ic_sync : Resource.Drawable.ic_sync_disabled);
            //};

            //_layout.AddView(checkButton);
            //_strikeTable = new LinearLayout(this);
            //_strikeTable.Orientation = Orientation.Vertical;
            LayoutInflater inflatorservice = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);
            _layout = (LinearLayout)inflatorservice.Inflate(Resource.Layout.StrikeTable, null);

            var checkButton = _layout.FindViewById<ImageButton>(Resource.Id.syncButton);
            checkButton.Click += delegate
            {
                _keepPolling = !_keepPolling;
                checkButton.SetImageResource(_keepPolling ? Resource.Drawable.ic_sync : Resource.Drawable.ic_sync_disabled);
            };

            SetupNodeButtons();

            _windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();
            _windowManager.AddView(_layout, param);

            PollZones();
        }

        private void SetupNodeButtons()
        {
            for (var node = 1; node <= 10; node++)
            {
                var resource = GetResourceByNumber(node);
                var nodeButton = _layout.FindViewById<Button>(resource);
                var nodeValue = node;
                nodeButton.Click += (sender, e) => NodeButton_Click(sender, e, nodeValue);
            }
        }

        private async void NodeButton_Click(object sender, EventArgs e, int node)
        {
            if (_zone <= 0)
            {
                return;
            }
            await _strikeTableManager.ClaimNode(_zone, node, _userId);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (_layout != null)
            {
                _windowManager.RemoveView(_layout);
                _layout = null;
            }

            _keepPolling = false;
        }

        public void onClick(object o, EventArgs e)
        {
            Toast.MakeText(this, "Test Message", ToastLength.Long).Show();
        }

        private Button CreateButton(string text)
        {
            var button = new Button(this);
            button.Alpha = 0.7f;
            button.SetBackgroundColor(Android.Graphics.Color.Black);
            button.SetText(text, TextView.BufferType.Normal);
            return button;
        }
    
        private async void PollZones()
        {
            while (_keepPolling)
            {
                var result = await _strikeTableManager.GetMappingAsync();

                if (result.Any())
                {
                    var zone = result.First();
                    //CreateNodes(zone);
                    //RemoveNodes(result);
                    UpdateZone(zone);
                    UpdateNodes(zone);
                }
                
                if (_keepPolling)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
        }

        private void UpdateZone(Zone zone)
        {
            var zoneButton = _layout.FindViewById<Button>(Resource.Id.zoneButton);
            zoneButton.Text = string.Format("Zone {0}", zone.ZoneNumber);
            _zone = zone.ZoneNumber;
        }

        private void UpdateNodes(Zone zone)
        {
            foreach (var strike in zone.Strikes)
            {
                var resource = GetResourceByNumber(strike.Node.NodeNumber);
                var nodeButton = _layout.FindViewById<Button>(resource);

                var isAvailable = string.IsNullOrEmpty(strike.UserId);
                nodeButton.Enabled = isAvailable;
                //nodeButton.SetBackgroundColor(isAvailable ? Color.Black : Color.WhiteSmoke);
                nodeButton.SetBackgroundResource(isAvailable ? Resource.Color.black : Resource.Drawable.diagonal_line);
            }
        }

        //private void CreateNodes(Zone zone)
        //{
        //    if (_strikeTable.ChildCount <= 0)
        //    {
        //        _strikeTable.AddView(CreateButton(string.Format("Zone {0}", zone.ZoneNumber)));
        //    }
        //}

        private int GetResourceByNumber(int nodeNumber)
        {
            switch(nodeNumber)
            {
                case 1: return Resource.Id.node1Button;
                case 2: return Resource.Id.node2Button;
                case 3: return Resource.Id.node3Button;
                case 4: return Resource.Id.node4Button;
                case 5: return Resource.Id.node5Button;
                case 6: return Resource.Id.node6Button;
                case 7: return Resource.Id.node7Button;
                case 8: return Resource.Id.node8Button;
                case 9: return Resource.Id.node9Button;
                case 10: return Resource.Id.node10Button;
                default:
                    throw new Exception("Out of bounds for nodes");
            }
        }

        private void GetZones()
        {
            _strikeTableManager.GetMappingAsync().ContinueWith(t =>
            {
                var builder = new AlertDialog.Builder(this);
                if (t.IsFaulted)
                {
                    builder.SetTitle("Error");
                    builder.SetMessage(t.Exception.Flatten().InnerException.ToString());
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }
                else if (t.IsCanceled)
                {
                    builder.SetTitle("Task Canceled");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }
                else
                {
                    var result = t.Result;

                }
            }, UIScheduler);
        }
    }
}