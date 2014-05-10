using System;

namespace Rhovlyn.Launcher
{
    public partial class GTKSettingsWindow : Gtk.Window
    {
        public GTKSettingsWindow() : 
            base(Gtk.WindowType.Toplevel)
        {
            this.Build();
        }
    }
}

