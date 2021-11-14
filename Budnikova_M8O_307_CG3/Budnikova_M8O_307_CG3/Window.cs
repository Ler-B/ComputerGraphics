using Gtk;
using Gdk;
using Cairo;
using System.Collections.Generic;

using Window = Gtk.Window;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using System.Diagnostics.CodeAnalysis;

//using Line2d = System.Collections.Generic.List<Budnikova_M8O_307_CG3.Vector2d>;


namespace Budnikova_M8O_307_CG3
{
    public class MyWindow : Window
    {
        [UI] private readonly Box _box = null;
        [UI] private readonly DrawingArea _drawingArea = null;

        
        private readonly SpinButtons _parameters;
        private readonly SpinButton _width;
        private readonly CheckButton _hideInvisibleLines;
        private readonly CheckButton _normals, _fill;
        private readonly CheckButtons _projections;
        private readonly CheckSpin2DButton _iPr;
        private readonly Spin3DButton _colorRgbLine2d, _colorRgbFill;
        private readonly Spin3DButton _position, _rotation;
        private readonly Spin3DButton _scale;
        private readonly Spin3DButton _lightSource;
        private readonly ChoiceButton _modelChoice;

        private readonly Spin3DButton _kA;  //kA
        private readonly Spin3DButton _iA; //iA

        private readonly Spin3DButton _iL; //iL
        private readonly Spin3DButton _kD;  //kD
        
        private readonly Spin3DButton _kS; //kS

        private readonly SpinButton _d;
        private readonly SpinButton _k;

        public MyWindow() : this(new Builder("Window.glade")) {}

        private MyWindow(Builder builder) : base(builder.GetRawOwnedObject("MyWindow"))
        {

            builder.Autoconnect(this);
            DeleteEvent += (_,_) => Application.Quit();

            _parameters = new SpinButtons(this, "", new[] {"R", "Meridians", "Paralels"}, new[] {100, 25d, 25}, new[] {0.0, 1, 1},
                new[] {10000, 100, 100.0}, new[] {20.0, 1, 1});
            
            _width = new SpinButton(this, "Line Width", value: 2, step: 1, min: 0.5, max: 10);
            
            _colorRgbLine2d = new Spin3DButton(this, "Color RGB Line2d", x: 255, y: 0, z: 255, min:0, max:255);
            _colorRgbFill = new Spin3DButton(this, "Color RGB Fill", x: 255, y: 255, z: 255, min: 0, max: 255);

            _hideInvisibleLines = new CheckButton(this, "Hide Invisible Lines");

            _normals = new CheckButton(this, "Normals");
            _fill = new CheckButton(this, "Fill");

            _projections = new CheckButtons(this, "",new[] {"xOy", "xOz", "yOz"}, new[] {false, false, false});

            _iPr = new CheckSpin2DButton(this, "Izometric", false, "Phi", "Theta", x: 45, y: 35, min1: -45, max1: 45, step1: 90, min2: -35, max2: 35, step2: 70);

            _scale = new Spin3DButton(this, "Scale", x: 1, y:1, z:1, step: 1, min: -50, max: 50);
            _rotation = new Spin3DButton(this, "Rotation", step: 1, min: -360, max: 360);
            _position = new Spin3DButton(this, "Position", step: 1,  min: -10000, max: 10000);


            _modelChoice = new ChoiceButton(this,"Model" ,new[] {"Flat shading", "Gouraud shading"});
           
            
            _lightSource = new Spin3DButton(this, "Relative Light Source Coordinates", x: 150, y: 0, z: -50, step: 1, min: -1000, max: 1000);

            _iA = new Spin3DButton(this, "iA", x: 0.3, y: 1, z: 1, step: 0.01, min:0, max:1);
            _iL = new Spin3DButton(this, "iL", x: 0.5, y: 0.5, z: 0.5, step: 0.01, min:0, max:1);
            
            _kA = new Spin3DButton(this, "kA", x: 1, y: 0.25, z: 0.5, step: 0.01, min:0, max:1);
            _kD = new Spin3DButton(this, "kD", x: 0.4, y: 0.1, z: 0.8, step: 0.01, min:0, max:1);
            
            _kS = new Spin3DButton(this, "kS", x: 1, y: 0, z: 0, step: 0.01, min:0, max:1);
            
            _d = new SpinButton(this, "D", value: 0.006, step: 0.001, min: 0, max: 1);
            _k = new SpinButton(this, "K", value: 0.5, step: 0.01, min: 0, max: 1);
            
            
            _box.Add(_parameters.Widget);

            _box.Add(_colorRgbLine2d.Widget);
            _box.Add(_colorRgbFill.Widget);
            _box.Add(_width.Widget);
            
            _box.Add(_modelChoice.Widget);
            
            _box.Add(_hideInvisibleLines.Widget);
            _box.Add(_fill.Widget);
            
            _box.Add(_normals.Widget);
            
            _box.Add(_lightSource.Widget);
            
            _box.Add(_iA.Widget);
            _box.Add(_iL.Widget);
            _box.Add(_kA.Widget);
            _box.Add(_kD.Widget);
            _box.Add(_kS.Widget);
            _box.Add(_d.Widget);
            _box.Add(_k.Widget);

            _box.Add(_projections.Widget);
            
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

        private void InitDrawingControl()
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
                    _position.ValueVec2d += (curPosition - buttonPosition);
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
        
        private static void Draw_Points(Context ct, List<Line2d> listPoints, Vector3d rgb, double width)
        {
            foreach (var points in listPoints)
            {
                ct.SetSourceRGB(rgb[0], rgb[1], rgb[2]);
                ct.LineWidth = width;

                var prevX = points[0].X;
                var prevZ = points[0].Y;

                foreach (Vector2d v in points)
                {
                    DrawLine2d(ct, prevX, prevZ, prevX = v.X, prevZ = v.Y);
                }
            }
        }
        
        private static void Draw_Axis(Context ct, List<Line2d> listPoints)
        {
            if (listPoints.Count != 3) throw new ArgumentException("Window.cs : Draw_Axis | listPoints.Count != 3");
            
            for (int i = 0; i < 3; ++i)
            {
                if ( i == 0 ) ct.SetSourceRGB(1, 0, 0);
                if ( i == 1 ) ct.SetSourceRGB(0, 1, 0);
                if ( i == 2 ) ct.SetSourceRGB(0, 0, 1);
                ct.LineWidth = 2;
                
                DrawLine2d(ct, listPoints[i].From.X, listPoints[i].From.Y, listPoints[i].To.X, listPoints[i].To.Y);
                
            }
        }

        private void Draw_Fill_Figure(Context ct, List<Line2d> listPoints, Vector3d rgb, double width)
        {
            for (var i = 0; i < listPoints.Count - 1; i += 2)
            {
                ct.SetSourceRGB(_colorRgbFill.X / 255, _colorRgbFill.Y / 255, _colorRgbFill.Z / 255);
                ct.MoveTo(listPoints[i][0].X, listPoints[i][0].Y);
                ct.LineTo(listPoints[i][1].X, listPoints[i][1].Y);
                ct.LineTo(listPoints[i + 1][0].X, listPoints[i + 1][0].Y);
                ct.LineTo(listPoints[i + 1][1].X, listPoints[i + 1][1].Y);
                ct.ClosePath();
                ct.Fill();
            }

            Draw_Points(ct, listPoints, rgb, width);
        }
        
        private void Calculate_and_Draw_Points(Context ct)
        {
            ct.SetSourceRGB(.2, .2, .2);
            ct.Paint();

            var dx = _drawingArea.Window.Width / 2;
            var dy = _drawingArea.Window.Height / 2;
            
            Figure figure = new(_parameters["R"], _parameters["Meridians"], _parameters["Paralels"]);
            
            var lightSource = new Vector4d(_lightSource.ValueVec4d);

            
            //scale
            var scale = _scale.ValueVec3d;
            scale = scale.LiniarMultiply(new Vector3d(dx / 350d, dy / 350d, dx / 250d));
            
            figure.Scale_XYZ(_scale.X, _scale.Y, _scale.Z);
            
            lightSource =  Matrix4d.Scale_XYZ(_scale.X, _scale.Y, _scale.Z) * lightSource;
            
            //rotation
            if (!_projections["xOy"] && !_projections["xOz"] && !_projections["yOz"] && !_iPr.Value)
            {
                figure.Rotation_XYZ(_rotation.X, _rotation.Y, _rotation.Z);
                lightSource = Matrix4d.Rot_XYZ(_rotation.X, _rotation.Y, _rotation.Z) * lightSource;
            }

            
            //move
            figure.Move_XYZ(_position.X, _position.Z, _position.Y);
            lightSource = Matrix4d.Move_XYZ(_position.X, _position.Z, _position.Y) * lightSource;
            

            
            //FIGURE POINTS
            var figurePoints = new List<Line2d>();
            
            if (!_projections["xOy"] && !_projections["xOz"] && !_projections["yOz"] && !_iPr.Value && !_hideInvisibleLines.Value)
            {
                figurePoints = figure.Get_XYZ_Points(dx: dx, dy: dy);
            }

            if (_iPr.Value)
            {
                figure.Rotation_XYZ(_iPr.X, _iPr.Y, 0);
                figurePoints = _hideInvisibleLines.Value ? figure.Get_XYZ_Points_Without_Invisible_Lines(dx: dx, dy: dy) : figure.Get_XYZ_Points();
                
            } else 
            if (_projections["xOy"] || _projections["xOz"] || _projections["yOz"] || _iPr.Value)
            {
                if (_projections["xOy"])
                {
                    figurePoints.InsertRange(figurePoints.Count, figure.Get_XY_Points(dx: dx, dy: dy));
                }

                if (_projections["xOz"])
                {
                    figurePoints.InsertRange(figurePoints.Count, figure.Get_XZ_Points(dx: dx, dy: dy));
                }

                if (_projections["yOz"])
                {
                    figurePoints.InsertRange(figurePoints.Count, figure.Get_YZ_Points(dx: dx, dy: dy));
                }

                if (_iPr.Value)
                {
                    figurePoints.InsertRange(figurePoints.Count, figure.Get_Izometric(dx: dx, dy: dy));
                }
            } else 
            if (_hideInvisibleLines.Value)
            {
                figurePoints = figure.Get_XYZ_Points_Without_Invisible_Lines(dx: dx, dy: dy);
            }


            var pointsFigure = new List<Line2d>();

            var width = _width.Value;

            var lineColor = new Vector3d(_colorRgbLine2d.X / 255d, _colorRgbLine2d.Y / 255d, _colorRgbLine2d.Z / 255d);

            if (_fill.Value)
            {
                Draw_Fill_Figure(ct, figurePoints, lineColor, width);
            } else if (_modelChoice.Value == "Flat shading")
            {
                Painting(ct, figure.Get_Polygons(), lightSource, dx, dy);
            }
            //AXIS
            var oxyz = figure.Get_XYZ_Axis(dx: dx * 1.8, dy: dy * 1.8);

            var pointsAxis = new List<Line2d> { oxyz[0], oxyz[1], oxyz[2]};

            Draw_Axis(ct, pointsAxis);



            //NORMALS
            if (!_normals.Value) return;
            {
                var figureNormals = figure.Get_XYZ_Normals(dx: dx, dy: dy);

                var normColor = new Vector3d(1, 0, 0);

                Draw_Points(ct, figureNormals, normColor, 3);
            }

        }

        private void Painting(Context ct, List<Polygon4Vec4d> p, Vector4d lS, double dx, double dy)
        {
            FlatShading flatShading = new FlatShading(lS, _kA.ValueVec4d, 
                _iA.ValueVec4d, _iL.ValueVec4d, _kD.ValueVec4d, _kS.ValueVec4d, _k.Value, _d.Value);

            Vector4d rgb = new Vector4d(_colorRgbFill.ValueVec4d) / 255;
            flatShading.Paint(ct, rgb, p, dx, dy);
        }
        
        private static void DrawLine2d(Context context, Vector2d point1, Vector2d point2)
        {
            //Draw(context, point1);
            context.MoveTo(point1.X, point1.Y);
            context.LineTo(point2.X, point2.Y);
            context.Stroke();
        }
        private static void DrawLine2d(Context context, double x1, double y1, double x2, double y2)
        {
            //Draw(context, new Vector2d(x1, y1));
            context.MoveTo(x1, y1);
            context.LineTo(x2, y2);
            context.Stroke();

        }

        private static void Draw(Context c, Vector2d point)
        {
            c.Arc(point.X, point.Y, 0.1, 0, 2 * Math.PI);
            
            c.Stroke();
        }
    }
}