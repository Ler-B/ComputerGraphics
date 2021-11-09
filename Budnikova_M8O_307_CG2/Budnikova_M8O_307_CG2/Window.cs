using Gtk;
using Gdk;
using Cairo;
using System.Collections.Generic;

using Window = Gtk.Window;
using UI = Gtk.Builder.ObjectAttribute;
using Line = System.Collections.Generic.List<Budnikova_M8O_307_CG2.Vector2d>;


namespace Budnikova_M8O_307_CG2
{
    public class MyWindow : Window
    {
        [UI] private readonly Box _box = null;
        [UI] private readonly DrawingArea _drawing_area = null;

        private readonly Check_Button _normals, _fill;
        private readonly Check_Button _Oxy, _Oxz, _Oyz;
        private readonly Check_Spin_2d_Button _i_pr;
        private readonly Spin_3d_Button _color_RGB_line, _color_RGB_fill;
        private readonly Spin_3d_Button _position, _rotation;
        private readonly Spin_3d_Button _scale; 


        public MyWindow() : this(new Builder("Window.glade")) {}

        private MyWindow(Builder builder) : base(builder.GetRawOwnedObject("MyWindow"))
        {

            builder.Autoconnect(this);
            DeleteEvent += (o, args) => Application.Quit();

            _normals = new Check_Button(this, "Normals", false);
            _fill = new Check_Button(this, "Fill", false);
            _Oxy = new Check_Button(this, "xOy", false);
            _Oxz = new Check_Button(this, "xOz", false);
            _Oyz = new Check_Button(this, "yOz", false);
            _i_pr = new Check_Spin_2d_Button(this, "Izometric", false, "Phi", "Theta", x:0, y:0, min1:-45, max1:45, step1:45, min2: -35, max2: 35, step2: 35);
            _color_RGB_line = new Spin_3d_Button(this, "Color RGB Line", x: 255, y: 0, z: 255, min:0, max:255);
            _color_RGB_fill = new Spin_3d_Button(this, "Color RGB Fill", x: 100, y: 0, z: 255, min: 0, max: 255);
            _scale = new Spin_3d_Button(this, "Scale", x: 1, y:1, z:1, step: 0.1, min: -50, max: 50);
            _rotation = new Spin_3d_Button(this, "Rotation", step: 1, min: -360, max: 360);
            _position = new Spin_3d_Button(this, "Position", step:1,  min: -10000, max: 10000);

            
            _box.Add(_color_RGB_line.Widget);
            _box.Add(_color_RGB_fill.Widget);

            _box.Add(_normals.Widget);
            _box.Add(_fill.Widget);
            _box.Add(_Oxy.Widget);
            _box.Add(_Oxz.Widget);
            _box.Add(_Oyz.Widget);

            _box.Add(_i_pr.Widget);

            _box.Add(_scale.Widget);
            _box.Add(_rotation.Widget);
            _box.Add(_position.Widget);

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
                Calculate_and_Draw_Points(args.Cr);
            };
        }

        private void Moving() {

            _drawing_area.Events |= EventMask.ButtonPressMask | EventMask.PointerMotionMask | EventMask.ButtonReleaseMask;

            uint action = 0;

            Vector2d button_position = new(0, 0);

            _drawing_area.ButtonPressEvent += (o, args) =>
            {
                action = args.Event.Button;
                button_position.X = args.Event.X;
                button_position.Y = args.Event.Y;
            };

            _drawing_area.MotionNotifyEvent += (o, args) =>
            {
                Vector2d cur_position = new(args.Event.X, args.Event.Y);

                if (action == 1)
                {
                    _position.Value += (cur_position - button_position);
                }
                if (action == 3)
                {
                    _rotation.X += (cur_position.Y - button_position.Y);
                    _rotation.Z += -(cur_position.X - button_position.X);
                }

                button_position.X = args.Event.X;
                button_position.Y = args.Event.Y;
            };

            _drawing_area.ButtonReleaseEvent += (o, args) => action = 0;
        }

        private static void Draw_Points(Context ct, List<(Line, int r, int g, int b, double width)> list_points)
        {
            foreach ((Line, int r, int g, int b, double width) points in list_points)
            {
                ct.SetSourceRGB(points.r, points.g, points.b);
                ct.LineWidth = points.width;

                double prev_x = points.Item1[0].X;
                double prev_z = points.Item1[0].Y;

                points.Item1.Add(points.Item1[0]);

                foreach (Vector2d v in points.Item1)
                {
                    DrawLine(ct, prev_x, prev_z, prev_x = v.X, prev_z = v.Y);
                }
            }
        }

        private void Draw_Fill_Figure(Context ct, List<(Line, int r, int g, int b, double width)> list_points)
        {
            for (int i = 0; i < list_points.Count - 1; i += 2)
            {
                ct.SetSourceRGB(_color_RGB_fill.X / 255, _color_RGB_fill.Y / 255, _color_RGB_fill.Z / 255);
                ct.MoveTo(list_points[i].Item1[0].X, list_points[i].Item1[0].Y);
                ct.LineTo(list_points[i].Item1[1].X, list_points[i].Item1[1].Y);
                ct.LineTo(list_points[i + 1].Item1[0].X, list_points[i + 1].Item1[0].Y);
                ct.LineTo(list_points[i + 1].Item1[1].X, list_points[i + 1].Item1[1].Y);
                ct.ClosePath();
                ct.Fill();
            }

            Draw_Points(ct, list_points);
        }

        private void Calculate_and_Draw_Points(Context ct)
        {
            ct.SetSourceRGB(.2, .2, .2);
            ct.Paint();

            var dx = _drawing_area.Window.Width / 2;
            var dy = _drawing_area.Window.Height / 2;

            Sq s = new(100, 200, 50);

            s.Scale_XYZ(_scale.X * dx / 350, _scale.Y * dx / 350, _scale.Z * dx / 350);

            if (!_Oxy.Value && !_Oxz.Value && !_Oyz.Value && !_i_pr.Value)
            {
                s.Rotation_XYZ(_rotation.X, _rotation.Y, _rotation.Z);
            }
                
            s.Move_XYZ(_position.X, _position.Z, _position.Y);


            

            //FIGURE
            List<Line> _Figure_points = new() { };
            if (!_Oxy.Value && !_Oxz.Value && !_Oyz.Value && !_i_pr.Value)
            {
                _Figure_points = s.Get_XYZ_Points(dx: dx, dy: dy);
            }

            if (_i_pr.Value)
            {
                s.Rotation_XYZ(_i_pr.X, _i_pr.Y, 0);
                _Figure_points = s.Get_XYZ_Points(dx: dx, dy: dy);

            } else
            {
                if (_Oxy.Value)
                {
                    _Figure_points.InsertRange(_Figure_points.Count, s.Get_XY_Points(dx: dx, dy: dy));
                }

                if (_Oxz.Value)
                {
                    _Figure_points.InsertRange(_Figure_points.Count, s.Get_XZ_Points(dx: dx, dy: dy));
                }

                if (_Oyz.Value)
                {
                    _Figure_points.InsertRange(_Figure_points.Count, s.Get_YZ_Points(dx: dx, dy: dy));
                }

                if (_i_pr.Value)
                {
                    _Figure_points.InsertRange(_Figure_points.Count, s.Get_Izometric(dx: dx, dy: dy));
                }
            }

            


            List<(Line, int r, int g, int b, double width)> points_figure = new() { };

            foreach (Line f in _Figure_points)
            {
                (Line, int r, int g, int b, double width) l = new (f, (int)_color_RGB_line.X / 255, (int)_color_RGB_line.Y / 255, (int)_color_RGB_line.Z / 255, 4);
                points_figure.Add(l);
            }

            if (_fill.Value)
            {
                Draw_Fill_Figure(ct, points_figure);
            } else
            {
                Draw_Points(ct, points_figure);
            }

            //AXIS
            List<Line> _Oxyz = s.Get_XYZ_Axis(dx: dx * 1.8, dy: dy * 1.8);

            List<(Line, int r, int g, int b, double width)> points_axis = new() { (_Oxyz[0], 1, 0, 0, 2),
                                                                                  (_Oxyz[1], 0, 1, 0, 2),
                                                                                  (_Oxyz[2], 0, 0, 1, 2)};

            Draw_Points(ct, points_axis);



            //NORMALS
            if (_normals.Value)
            {
                List<Line> _Figure_normals = s.Get_XYZ_Normals(dx: dx, dy: dy);

                List<(Line, int r, int g, int b, double width)> points_normals = new() { };

                foreach (Line f in _Figure_normals)
                {
                    (Line, int r, int g, int b, double width) l = new(f, 1, 0, 0, 3);
                    points_normals.Add(l);
                }

                Draw_Points(ct, points_normals);
            }
            
        }

        public static void DrawLine(Context context, Vector2d point1, Vector2d point2)
        {
            context.MoveTo(point1.X, point1.Y);
            context.LineTo(point2.X, point2.Y);
            context.Stroke();
        }

        public static void DrawLine(Context context, double x1, double y1, double x2, double y2)
        {
            context.MoveTo(x1, y1);
            context.LineTo(x2, y2);
            context.Stroke();
        }
    }
}