﻿using Android.Content;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using MonsterBoxRemote.Mobile.Droid.Renderers;

[assembly: ExportRenderer(typeof(Entry), typeof(StylelessEntry))]
namespace MonsterBoxRemote.Mobile.Droid.Renderers
{
    class StylelessEntry : EntryRenderer
    {
        public StylelessEntry(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                Control.SetBackgroundColor(Color.Transparent.ToAndroid());
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}