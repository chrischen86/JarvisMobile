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

namespace Jarvis
{
    [Service]
    [IntentFilter(new String[] { "AlphaFlight.OverlayService" })]
    class OverlayService : Service
    {
        IWindowManager _windowManager;
        Button _button;
        View _middleView;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            var param = new WindowManagerLayoutParams(
                   WindowManagerLayoutParams.WrapContent,
                   WindowManagerLayoutParams.WrapContent,
                             WindowManagerTypes.SystemAlert,
                             WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal,
                             Android.Graphics.Format.Translucent);
            param.Gravity = GravityFlags.Right | GravityFlags.CenterVertical;
            //param.X = 0;
            //param.Y = 0;
            //param.Title = "Overlay window title";

            _button = new Button(this);
            _button.SetText("test overlay button", TextView.BufferType.Normal);
            _button.Alpha = 0.5f;
            _button.SetBackgroundColor(Android.Graphics.Color.Cyan);
            _button.Click += onClick;

            _windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();
            _windowManager.AddView(_button, param);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (_button == null)
            {
                return;
            }

            _windowManager.RemoveView(_button);
            //            _windowManager.RemoveView(_middleView);
            _button = null;
            //          _middleView = null;
        }

        public void onClick(object o, EventArgs e)
        {
            Toast.MakeText(this, "Test Message", ToastLength.Long).Show();
        }
    }
}