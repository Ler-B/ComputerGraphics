using System.Collections.Generic;
using Gtk;

using Line2d = System.Collections.Generic.List<Budnikova_M8O_307_CG2.Vector2d>;

namespace Budnikova_M8O_307_CG2
{
    public class Spin_Button
    {
        private readonly HBox _box;
        private readonly Widget _widget;
        private readonly SpinButton _button;

        public double Value
        {
            get
            {
                return _button.Value;
            }
            set
            {
                _button.Value = value;
            }
        }

        public Widget Widget
        {
            get
            {
                return _widget;
            }
        }

        public Spin_Button(MyWindow window, string label, double value = 0, double min = -1000, double max = 1000, double step = 1)
        {
            _box = new HBox { new Label(label) };
            _button = new SpinButton(min, max, step) { Digits = 3, Value = value };
            _button.Halign = Align.Start;
            _button.Margin = 10;
            _button.ValueChanged += (o, args) => window.RequestRedraw();

            _box.Add(_button);
            _widget = _box;
        }
    };

    public class Spin_2d_Button
    {
        private readonly HBox _box;
        private readonly Widget _widget;
        private readonly SpinButton _button1;
        private readonly SpinButton _button2;

        public double X
        {
            get
            {
                return _button1.Value;
            }
            set
            {
                _button1.Value = value;
            }
        }

        public double Y
        {
            get
            {
                return _button2.Value;
            }
            set
            {
                _button2.Value = value;
            }
        }

        public Vector2d Value
        {
            get
            {
                return new Vector2d(_button1.Value, _button2.Value);
            }
            set
            {
                _button1.Value = value.X;
                _button2.Value = value.Y;
            }
        }

        public Widget Widget
        {
            get
            {
                return _widget;
            }
        }

        public Spin_2d_Button(MyWindow window, string label, double x = 0, double y = 0, double min = -1000, double max = 1000, double step = 1)
        {
            _box = new HBox { new Label(label) };

            _button1 = new SpinButton(min, max, step) { Digits = 3, Value = x };
            _button1.Halign = Align.Start;
            _button1.Margin = 10;
            _button1.ValueChanged += (o, args) => window.RequestRedraw();

            _button2 = new SpinButton(min, max, step) { Digits = 3, Value = y };
            _button2.Halign = Align.Start;
            _button2.Margin = 10;
            _button2.ValueChanged += (o, args) => window.RequestRedraw();


            _box.Add(_button1);
            _box.Add(_button2);
            _widget = _box;
        }

    }

    public class Spin_3d_Button
    {
        private readonly HBox _box;
        private readonly Widget _widget;
        private readonly SpinButton _button1;
        private readonly SpinButton _button2;
        private readonly SpinButton _button3;

        public double X
        {
            get
            {
                return _button1.Value;
            }
            set
            {
                _button1.Value = value;
            }
        }

        public double Y
        {
            get
            {
                return _button2.Value;
            }
            set
            {
                _button2.Value = value;
            }
        }

        public double Z
        {
            get
            {
                return _button3.Value;
            }
            set
            {
                _button3.Value = value;
            }
        }

        public Vector2d Value
        {
            get
            {
                return new Vector2d(_button1.Value, _button2.Value);
            }
            set
            {
                _button1.Value = value.X;
                _button2.Value = value.Y;
            }
        }

        public Widget Widget
        {
            get
            {
                return _widget;
            }
        }

        public Spin_3d_Button(MyWindow window, string label, double x = 0, double y = 0, double z = 0, double min = -1000, double max = 1000, double step = 1)
        {
            _box = new HBox { new Label(label) };

            _button1 = new SpinButton(min, max, step) { Digits = 3, Value = x };
            _button1.Halign = Align.Start;
            _button1.Margin = 10;
            _button1.ValueChanged += (o, args) => window.RequestRedraw();

            _button2 = new SpinButton(min, max, step) { Digits = 3, Value = y };
            _button2.Halign = Align.Start;
            _button2.Margin = 10;
            _button2.ValueChanged += (o, args) => window.RequestRedraw();

            _button3 = new SpinButton(min, max, step) { Digits = 3, Value = z };
            _button3.Halign = Align.Start;
            _button3.Margin = 10;
            _button3.ValueChanged += (o, args) => window.RequestRedraw();

            _box.Add(_button1);
            _box.Add(_button2);
            _box.Add(_button3);
            _widget = _box;
        }
    }

    public class Check_Button
    {
        private readonly HBox _box;
        private readonly Widget _widget;
        private readonly CheckButton _button;

        public bool Value
        {
            get
            {
                return _button.Active;
            }
            set
            {
                _button.Active = value;
            }
        }

        public Widget Widget
        {
            get
            {
                return _widget;
            }
        }

        public Check_Button(MyWindow window, string label, bool check)
        {
            _box = new HBox { new Label(label) };
            _button = new CheckButton { Active = check };
            _button.Clicked += (o, args) => window.RequestRedraw();

            _box.Add(_button);
            _widget = _box;
        }
    }

    public class Check_Spin_2d_Button
    {
        private readonly HBox _box;
        private readonly Widget _widget;
        private readonly CheckButton _button;
        private readonly SpinButton _button1;
        private readonly SpinButton _button2;

        public bool Value
        {
            get
            {
                return _button.Active;
            }
            set
            {
                _button.Active = value;
            }
        }

        public Widget Widget
        {
            get
            {
                return _widget;
            }
        }

        public double X
        {
            get
            {
                return _button1.Value;
            }
            set
            {
                _button1.Value = value;
            }
        }

        public double Y
        {
            get
            {
                return _button2.Value;
            }
            set
            {
                _button2.Value = value;
            }
        }

        public Check_Spin_2d_Button(MyWindow window, string label_check, bool check, string label1, string label2, double x = 0, double y = 0,
                                    double min1 = -1000, double max1 = 1000, double step1 = 1, double min2 = -1000, double max2 = 1000, double step2 = 1)
        {
            _box = new HBox { new Label(label_check) };
            _button = new CheckButton { Active = check };
            _button.Clicked += (o, args) => window.RequestRedraw();
            _box.Add(_button);

            _box.Add(new Label(label1));
            _button1 = new SpinButton(min1, max1, step1) { Digits = 3, Value = x };
            _button1.Halign = Align.Start;
            _button1.Margin = 10;
            _button1.ValueChanged += (o, args) => { if (_button.Active) window.RequestRedraw(); };
            _box.Add(_button1);

            _box.Add(new Label(label2));
            _button2 = new SpinButton(min2, max2, step2) { Digits = 3, Value = y };
            _button2.Halign = Align.Start;
            _button2.Margin = 10;
            _button2.ValueChanged += (o, args) => { if (_button.Active) window.RequestRedraw(); };
            _box.Add(_button2);

            _widget = _box;
        }
    }

    public class Line4d
    {
        private readonly List<Vector4d> list;

        public Vector4d this[int i]
        {
            get
            {
                return list[i];
            }
            set
            {
                list[i] = value;
            }
        }

        public Line4d(Vector4d r, Vector4d l)
        {
            list = new List<Vector4d> { r, l };
        }

        public Line4d(Vector4d[] l)
        {
            this.list = new List<Vector4d> { l[0], l[1] };
        }

        public void Modify(Matrix4d m)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                list[i] = m * list[i];
            }
        }
    }

    public class Polygon3_4d
    {
        private readonly List<Vector4d> list;

        public Vector4d this[int i]
        {
            get
            {
                return list[i];
            }
            set
            {
                list[i] = value;
            }
        }

        public Polygon3_4d(Vector4d r, Vector4d l, Vector4d up)
        {
            list = new List<Vector4d> { r, l, up };
        }

        public Polygon3_4d(Vector4d[] l)
        {
            this.list = new List<Vector4d> { l[0], l[1], l[2] };
        }

        public void Modify(Matrix4d m)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                list[i] = m * list[i];
            }
        }

    }

    public class Polygon4_4d
    {
        private readonly List<Vector4d> list;

        public Vector4d this[int i]
        {
            get
            {
                return list[i];
            }
            set
            {
                list[i] = value;
            }
        }

        public int Size
        {
            get
            {
                return list.Count;
            }
        }

        public Polygon4_4d(Vector4d dr, Vector4d dl, Vector4d ul, Vector4d ur)
        {
            list = new List<Vector4d> { dr, dl, ul, ur };
        }

        public Polygon4_4d(Vector4d[] list)
        {
            this.list = new List<Vector4d> { list[0], list[1], list[2], list[3] };
        }

        public void Modify(Matrix4d m)
        {
            for (int i = 0; i < Size; ++i)
            {
                this[i] = m * this[i];
            }
        }

        public Polygon4_4d New_Modify(Matrix4d m)
        {
            Polygon4_4d p = new(this[0], this[1], this[2], this[3]);
            p.Modify(m);
            return p;
        }

        public double Min_Y()
        {
            double min = list[0].Y;

            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].Y < min)
                {
                    min = list[i].Y;
                }
            }

            return min;
        }

        public List<Line2d> To_Line_2d(double dx, double dy)
        {
            List<Line2d> rez = new() { };

            for (int i = 0; i < Size; ++i)
            {
                int j = (i + 1) % Size;
                Line2d v = new() { new Vector2d(this[i].X + dx, this[i].Y + dy), new Vector2d(this[j].X + dx, this[j].Y + dy) };
                rez.Add(v);
            }

            return rez;
        }

    }



}






