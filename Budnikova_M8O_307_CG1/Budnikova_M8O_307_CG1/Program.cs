using System;
using Gtk;

namespace Budnikova_M8O_307_CG1
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();


            var app = new Application("org.CG.CG", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            MyWindow win = new MyWindow();
            app.AddWindow(win);


            win.ShowAll();
            Application.Run();
        }
    }
}
