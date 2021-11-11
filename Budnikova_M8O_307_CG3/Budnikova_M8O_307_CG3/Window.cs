using Gtk;
using Gdk;
using Cairo;
using System.Collections.Generic;

using Window = Gtk.Window;
using UI = Gtk.Builder.ObjectAttribute;
using System;
//using Line2d = System.Collections.Generic.List<Budnikova_M8O_307_CG3.Vector2d>;


namespace Budnikova_M8O_307_CG3
{
    public class MyWindow : Window
    {
        [UI] private readonly Box _box = null;
        [UI] private readonly DrawingArea _drawingArea = null;

        private readonly SpinButton _r;
        private readonly SpinButton _mer, _par;
        private readonly SpinButton _width;
        private readonly CheckButton _hideInvisibleLines;
        private readonly CheckButton _normals, _fill;
        private readonly CheckButton _oxy, _oxz, _oyz;
        private readonly CheckSpin2DButton _iPr;
        private readonly Spin3DButton _colorRgbLine2d, _colorRgbFill;
        private readonly Spin3DButton _position, _rotation;
        private readonly Spin3DButton _scale;


        public MyWindow() : this(new Builder("Window.glade")) {}

        private MyWindow(Builder builder) : base(builder.GetRawOwnedObject("MyWindow"))
        {

            builder.Autoconnect(this);
            DeleteEvent += (_,_) => Application.Quit();

            _r = new SpinButton(this, "R", value: 100, step:50, min:0, max:10000);
            _mer = new SpinButton(this, "Meridians", value: 5, step: 1, min: 1, max: 100);
            _par = new SpinButton(this, "Paralels", value: 5, step: 1, min: 1, max: 100);
            _width = new SpinButton(this, "Line Width", value: 3, step: 1, min: 0.5, max: 10);

            _colorRgbLine2d = new Spin3DButton(this, "Color RGB Line2d", x: 255, y: 0, z: 255, min:0, max:255);
            _colorRgbFill = new Spin3DButton(this, "Color RGB Fill", x: 100, y: 0, z: 255, min: 0, max: 255);

            _hideInvisibleLines = new CheckButton(this, "Hide Invisible Lines");

            _normals = new CheckButton(this, "Normals");
            _fill = new CheckButton(this, "Fill");

            _oxy = new CheckButton(this, "xOy");
            _oxz = new CheckButton(this, "xOz");
            _oyz = new CheckButton(this, "yOz");

            _iPr = new CheckSpin2DButton(this, "Izometric", false, "Phi", "Theta", x: 45, y: 35, min1: -45, max1: 45, step1: 90, min2: -35, max2: 35, step2: 70);

            _scale = new Spin3DButton(this, "Scale", x: 1, y:1, z:1, step: 1, min: -50, max: 50);
            _rotation = new Spin3DButton(this, "Rotation", step: 1, min: -360, max: 360);
            _position = new Spin3DButton(this, "Position", step: 1,  min: -10000, max: 10000);


            _box.Add(_r.Widget);
            _box.Add(_mer.Widget);
            _box.Add(_par.Widget);

            _box.Add(_colorRgbLine2d.Widget);
            _box.Add(_colorRgbFill.Widget);
            _box.Add(_width.Widget);

            _box.Add(_hideInvisibleLines.Widget);

            _box.Add(_normals.Widget);
            _box.Add(_fill.Widget);

            _box.Add(_oxy.Widget);
            _box.Add(_oxz.Widget);
            _box.Add(_oyz.Widget);

            _box.Add(_iPr.Widget);

            _box.Add(_scale.Widget);
            _box.Add(_rotation.Widget);
            _box.Add(_position.Widget);

            Moving();

            InitDrawingControl();

        }

        public void RequestRedraw()
        {
            _drawingArea.QueueDraw();
        }

        public void InitDrawingControl()
        {
            _drawingArea.Drawn += (_, args) =>
            {
                Calculate_and_Draw_Points(args.Cr);
            };
        }

        private void Moving() {

            _drawingArea.Events |= EventMask.ButtonPressMask | EventMask.PointerMotionMask | EventMask.ButtonReleaseMask;

            uint action = 0;

            Vector2d buttonPosition = new(0, 0);

            _drawingArea.ButtonPressEvent += (_, args) =>
            {
                action = args.Event.Button;
                buttonPosition.X = args.Event.X;
                buttonPosition.Y = args.Event.Y;
            };

            _drawingArea.MotionNotifyEvent += (_, args) =>
            {
                Vector2d curPosition = new(args.Event.X, args.Event.Y);

                if (action == 1)
                {
                    _position.Value += (curPosition - buttonPosition);
                }
                if (action == 3)
                {
                    _rotation.X += (curPosition.Y - buttonPosition.Y);
                    _rotation.Z += -(curPosition.X - buttonPosition.X);
                }

                buttonPosition.X = args.Event.X;
                buttonPosition.Y = args.Event.Y;
            };

            _drawingArea.ButtonReleaseEvent += (_,_) => action = 0;
        }

        private static void Draw_Points(Context ct, List<(Line2d, int r, int g, int b, double width)> listPoints)
        {
            foreach ((Line2d, int r, int g, int b, double width) points in listPoints)
            {
                ct.SetSourceRGB(points.r, points.g, points.b);
                ct.LineWidth = points.width;

                double prevX = points.Item1[0].X;
                double prevZ = points.Item1[0].Y;

                foreach (Vector2d v in points.Item1)
                {
                    DrawLine2d(ct, prevX, prevZ, prevX = v.X, prevZ = v.Y);
                }
            }
        }

        private void Draw_Fill_Figure(Context ct, List<(Line2d, int r, int g, int b, double width)> listPoints)
        {
            for (int i = 0; i < listPoints.Count - 1; i += 2)
            {
                ct.SetSourceRGB(_colorRgbFill.X / 255, _colorRgbFill.Y / 255, _colorRgbFill.Z / 255);
                ct.MoveTo(listPoints[i].Item1[0].X, listPoints[i].Item1[0].Y);
                ct.LineTo(listPoints[i].Item1[1].X, listPoints[i].Item1[1].Y);
                ct.LineTo(listPoints[i + 1].Item1[0].X, listPoints[i + 1].Item1[0].Y);
                ct.LineTo(listPoints[i + 1].Item1[1].X, listPoints[i + 1].Item1[1].Y);
                ct.ClosePath();
                ct.Fill();
            }

            Draw_Points(ct, listPoints);
        }

        private void Calculate_and_Draw_Points(Context ct)
        {
            ct.SetSourceRGB(.2, .2, .2);
            ct.Paint();

            var dx = _drawingArea.Window.Width / 2;
            var dy = _drawingArea.Window.Height / 2;

            Figure s = new(_r.Value, _mer.Value, _par.Value);

            s.Scale_XYZ(_scale.X * dx / 350, _scale.Y * dx / 350, _scale.Z * dx / 350);

            if (!_oxy.Value && !_oxz.Value && !_oyz.Value && !_iPr.Value)
            {
                s.Rotation_XYZ(_rotation.X, _rotation.Y, _rotation.Z);
            }
                
            s.Move_XYZ(_position.X, _position.Z, _position.Y);


            

            //FIGURE
            List<Line2d> figurePoints = new();
            if (!_oxy.Value && !_oxz.Value && !_oyz.Value && !_iPr.Value && !_hideInvisibleLines.Value)
            {
                figurePoints = s.Get_XYZ_Points(dx: dx, dy: dy);
            }

            if (_iPr.Value)
            {
                s.Rotation_XYZ(_iPr.X, _iPr.Y, 0);
                figurePoints = _hideInvisibleLines.Value ? s.Get_XYZ_Points_Without_Invisible_Lines(dx: dx, dy: dy) : s.Get_XYZ_Points();
            } else if (!_hideInvisibleLines.Value)
            {
                if (_oxy.Value)
                {
                    figurePoints.InsertRange(figurePoints.Count, s.Get_XY_Points(dx: dx, dy: dy));
                }

                if (_oxz.Value)
                {
                    figurePoints.InsertRange(figurePoints.Count, s.Get_XZ_Points(dx: dx, dy: dy));
                }

                if (_oyz.Value)
                {
                    figurePoints.InsertRange(figurePoints.Count, s.Get_YZ_Points(dx: dx, dy: dy));
                }

                if (_iPr.Value)
                {
                    figurePoints.InsertRange(figurePoints.Count, s.Get_Izometric(dx: dx, dy: dy));
                }
            } else if (_hideInvisibleLines.Value)
            {
                figurePoints = s.Get_XYZ_Points_Without_Invisible_Lines(dx: dx, dy: dy);
            }


            List<(Line2d, int r, int g, int b, double width)> pointsFigure = new();

            double width = _width.Value;
            
            foreach (Line2d f in figurePoints)
            {
                (Line2d, int r, int g, int b, double width) l = new (f, (int)_colorRgbLine2d.X / 255, (int)_colorRgbLine2d.Y / 255, (int)_colorRgbLine2d.Z / 255, width);
                pointsFigure.Add(l);
            }

            if (_fill.Value)
            {
                Draw_Fill_Figure(ct, pointsFigure);
            } else
            {
                Draw_Points(ct, pointsFigure);
            }

            //AXIS
            List<Line2d> oxyz = s.Get_XYZ_Axis(dx: dx * 1.8, dy: dy * 1.8);

            List<(Line2d, int r, int g, int b, double width)> pointsAxis = new() { (oxyz[0], 1, 0, 0, 2),
                                                                                  (oxyz[1], 0, 1, 0, 2),
                                                                                  (oxyz[2], 0, 0, 1, 2)};

            Draw_Points(ct, pointsAxis);



            //NORMALS
            if (_normals.Value)
            {
                List<Line2d> figureNormals = s.Get_XYZ_Normals(dx: dx, dy: dy);

                List<(Line2d, int r, int g, int b, double width)> pointsNormals = new();

                foreach (Line2d f in figureNormals)
                {
                    (Line2d, int r, int g, int b, double width) l = new(f, 1, 0, 0, 3);
                    pointsNormals.Add(l);
                }

                Draw_Points(ct, pointsNormals);
            }
            
        }

        public static void DrawLine2d(Context context, Vector2d point1, Vector2d point2)
        {
            //Draw(context, point1);
            context.MoveTo(point1.X, point1.Y);
            context.LineTo(point2.X, point2.Y);
            context.Stroke();
        }

        public static void DrawLine2d(Context context, double x1, double y1, double x2, double y2)
        {
            //Draw(context, new Vector2d(x1, y1));
            context.MoveTo(x1, y1);
            context.LineTo(x2, y2);
            context.Stroke();

        }

        public static void Draw(Context c, Vector2d point)
        {
            c.Arc(point.X, point.Y, 0.1, 0, 2 * Math.PI);
            
            c.Stroke();
        }
    }
}