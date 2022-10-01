using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using MonsterBoxRemote.Mobile.iOS.Renderers;

[assembly: ExportRenderer(typeof(Entry), typeof(StylelessEntryRenderer))]
namespace MonsterBoxRemote.Mobile.iOS.Renderers
{
    public class StylelessEntryRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null) return;

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}