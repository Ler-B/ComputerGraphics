using System;
using Gtk;

namespace Budnikova_M8O_307_CG2
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.Init();


            var app = new Application("org.CG.CG", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            MyWindow win = new();
            app.AddWindow(win);


            win.ShowAll();
            Application.Run();
        }
    }
}
