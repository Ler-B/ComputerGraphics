using System;
using Gtk;
using Cairo;
using Gdk;
using Window = Gtk.Window;
using UI = Gtk.Builder.ObjectAttribute;

namespace Budnikova_M8O_307_CG1
{
    public class MyWindow : Window
    {
        [UI] private Box _box = null;
        [UI] private DrawingArea _drawing_area = null;

        private Spin_Button _a;
        private Spin_Button _compression_x, _compression_y;
        private Spin_Button _size, _step;
        private Spin_Button _angle;
        private Spin_2d_Button _position;


        private Spin_2d_Button _dr_ar_size;

        public MyWindow() : this(new Builder("Window.glade"))
        {
        }

        private MyWindow(Builder builder) : base(builder.GetRawOwnedObject("MyWindow"))
        {

            builder.Autoconnect(this);
            DeleteEvent += (o, args) => Application.Quit();
            

            _a = new Spin_Button(this, "a", value: 150, step: 10);
            _size = new Spin_Button(this, "Size", value: 1, step: 0.1, max: 5);
            _step = new Spin_Button(this, "Step", value: 50, step: 1, min: 1, max: 100);
            _angle = new Spin_Button(this,"Rotation", step: 10, min: -360, max: 360);
            _compression_x = new Spin_Button(this, "Compression_X", step: 10, min: -360, max: 360);
            _compression_y = new Spin_Button(this, "Compression_Y", step: 10, min: -360, max: 360);
            _position = new Spin_2d_Button(this, "Position", step:1,  min: -10000, max: 10000);

            _dr_ar_size = new Spin_2d_Button(this, "Size");


            _box.Add(_a.Widget);
            _box.Add(_size.Widget);
            _box.Add(_step.Widget);
            _box.Add(_angle.Widget);
            _box.Add(_compression_x.Widget);
            _box.Add(_compression_y.Widget);
            _box.Add(_position.Widget);
            _box.Add(_dr_ar_size.Widget);

            Moving();

            InitDrawingControl();

        }

        public void RequestRedraw()
        {
            _drawing_area.QueueDraw();
        }

        public void InitDrawingControl()
        {
            _drawing_area.Drawn += (o, args) =>
            {
                DrawnWin(args.Cr);
                DrawnFigure(args.Cr);
            };
        }

        private void Moving() {

            _drawing_area.Events |= EventMask.ButtonPressMask | EventMask.PointerMotionMask | EventMask.ButtonReleaseMask;

            uint action = 0;

            Point button_position = new Point(0, 0);

            _drawing_area.ButtonPressEvent += (o, args) =>
            {
                action = args.Event.Button;
                button_position.X = args.Event.X;
                button_position.Y = args.Event.Y;
            };

            _drawing_area.MotionNotifyEvent += (o, args) =>
            {
                Point cur_position = new Point(args.Event.X, args.Event.Y);

                if (action == 1)
                {
                    _position.Value += (cur_position - button_position);
                }
                if (action == 3)
                {
                    _angle.Value += cur_position.Y- button_position.Y;
                }

                button_position.X = args.Event.X;
                button_position.Y = args.Event.Y;
            };

            _drawing_area.ButtonReleaseEvent += (o, args) => action = 0;
        }

        private void DrawnWin(Context cr)
        {
            cr.SetSourceRGB(.2, .2, .2);
            cr.Paint();

        }

        private void DrawnFigure(Context ct)
        {

            double drawing_area_width = _drawing_area.Window.Width;
            double drawing_area_height = _drawing_area.Window.Height;

            double screen_sizeX = 500;
            double screen_sizeY = 400;

            ct.LineWidth = 1d;
            ct.SetSourceRGB(255, 255, 255);

            double dx = (screen_sizeX / 2 + _position.X) * drawing_area_width / screen_sizeX;
            double dy = (screen_sizeY / 2 + _position.Y) * drawing_area_height / screen_sizeY;

            DrawLine(ct, new Point(0, drawing_area_height / 2), new Point(drawing_area_width, drawing_area_height / 2));
            DrawLine(ct, new Point(drawing_area_width / 2, 0), new Point(drawing_area_width / 2, drawing_area_height));

            ct.LineWidth = 2d;
            ct.SetSourceRGB(0, 150, 130);
            ct.Antialias = Antialias.Subpixel;

            double psy = _angle.Value * Math.PI / 180;

            double compression_x = _compression_x.Value * Math.PI / 180;
            double compression_y = (90 - _compression_y.Value) * Math.PI / 180;

            double x = _a.Value * Math.Cos(psy);
            double y = _a.Value * Math.Sin(psy);

            x *= Math.Cos(compression_x);
            y *= Math.Sin(compression_y);

            x *= _size.Value * drawing_area_width / screen_sizeX;
            y *= _size.Value * drawing_area_height / screen_sizeY;

            x += dx;
            y += dy;

            Point xy = new Point(x, y);

            for (double al = 0; al <= 2.5 * Math.PI; al += 2 * Math.PI / _step.Value)
            {
                double ph_x = Math.Cos(al);
                double ph_y = Math.Sin(al);

                x = _a.Value * Math.Pow(ph_x, 3) * Math.Cos(psy) - _a.Value * Math.Pow(ph_y, 3) * Math.Sin(psy);
                y = _a.Value * Math.Pow(ph_x, 3) * Math.Sin(psy) + _a.Value * Math.Pow(ph_y, 3) * Math.Cos(psy);

                x *= Math.Cos(compression_x);
                y *= Math.Sin(compression_y);

                x *= _size.Value * drawing_area_width / screen_sizeX;
                y *= _size.Value * drawing_area_height / screen_sizeY;

                x += dx;
                y += dy;

                DrawLine(ct, xy, new Point(x, y));
                xy.X = x;
                xy.Y = y;
            }

      
        }

        public void DrawLine(Context context, Point point1, Point point2)
        {
            context.MoveTo(point1.X, point1.Y);
            context.LineTo(point2.X, point2.Y);
            context.Stroke();
        }
    }
}