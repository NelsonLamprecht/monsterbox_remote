using Android.Content;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using MonsterBoxRemote.Mobile.Droid.Renderers;

[assembly: ExportRenderer(typeof(Picker), typeof(StylelessPicker))]
namespace MonsterBoxRemote.Mobile.Droid.Renderers
{
    public class StylelessPicker : PickerRenderer
    {
        public StylelessPicker(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
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
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}