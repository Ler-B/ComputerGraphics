using System.Collections;
using System.Collections.Generic;
using Gtk;

using Line2d = System.Collections.Generic.List<Budnikova_M8O_307_CG3.Vector2d>;

namespace Budnikova_M8O_307_CG3
{
    public class SpinButton
    {
        private readonly Gtk.SpinButton _button;
        public Widget Widget { get; }
        public double Value
        {
            get => _button.Value;
            set => _button.Value = value;
        }

        

        public SpinButton(MyWindow window, string label, double value = 0, double min = -1000, double max = 1000, double step = 1)
        {
            var box = new HBox { new Label(label) };
            _button = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = value };
            _button.Halign = Align.Start;
            _button.Margin = 10;
            _button.ValueChanged += (_, _) => window.RequestRedraw();

            box.Add(_button);
            Widget = box;
        }
    };

    public class Spin2DButton
    {
        private readonly Gtk.SpinButton _button1;
        private readonly Gtk.SpinButton _button2;
        public Widget Widget { get; }

        public double X
        {
            get => _button1.Value;
            set => _button1.Value = value;
        }

        public double Y
        {
            get => _button2.Value;
            set => _button2.Value = value;
        }

        public Vector2d Value
        {
            get => new Vector2d(_button1.Value, _button2.Value);
            set
            {
                _button1.Value = value.X;
                _button2.Value = value.Y;
            }
        }

        public Spin2DButton(MyWindow window, string label, double x = 0, double y = 0, double min = -1000, double max = 1000, double step = 1)
        {
            var box = new HBox { new Label(label) };

            _button1 = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = x };
            _button1.Halign = Align.Start;
            _button1.Margin = 10;
            _button1.ValueChanged += (_,_) => window.RequestRedraw();

            _button2 = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = y };
            _button2.Halign = Align.Start;
            _button2.Margin = 10;
            _button2.ValueChanged += (_,_) => window.RequestRedraw();


            box.Add(_button1);
            box.Add(_button2);
            Widget = box;
        }

    }

    public class Spin3DButton
    {
        private readonly Gtk.SpinButton _button1;
        private readonly Gtk.SpinButton _button2;
        private readonly Gtk.SpinButton _button3;

        public Widget Widget { get; }
        public double X
        {
            get => _button1.Value;
            set => _button1.Value = value;
        }

        public double Y
        {
            get => _button2.Value;
            set => _button2.Value = value;
        }

        public double Z
        {
            get => _button3.Value;
            set => _button3.Value = value;
        }

        public Vector2d Value
        {
            get => new Vector2d(_button1.Value, _button2.Value);
            set
            {
                _button1.Value = value.X;
                _button2.Value = value.Y;
            }
        }

        public Spin3DButton(MyWindow window, string label, double x = 0, double y = 0, double z = 0, double min = -1000, double max = 1000, double step = 1)
        {
            var box = new HBox { new Label(label) };

            _button1 = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = x };
            _button1.Halign = Align.Start;
            _button1.Margin = 10;
            _button1.ValueChanged += (_,_) => window.RequestRedraw();

            _button2 = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = y };
            _button2.Halign = Align.Start;
            _button2.Margin = 10;
            _button2.ValueChanged += (_,_) => window.RequestRedraw();

            _button3 = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = z };
            _button3.Halign = Align.Start;
            _button3.Margin = 10;
            _button3.ValueChanged += (_,_) => window.RequestRedraw();

            box.Add(_button1);
            box.Add(_button2);
            box.Add(_button3);
            Widget = box;
        }
    }

    public class CheckButton
    {
        private readonly Gtk.CheckButton _button;
        public Widget Widget { get; }
        
        public bool Value
        {
            get => _button.Active;
            set => _button.Active = value;
        }

        public CheckButton(MyWindow window, string label, bool check = false)
        {
            var box = new HBox { new Label(label) };
            _button = new Gtk.CheckButton { Active = check };
            _button.Clicked += (_,_) => window.RequestRedraw();

            box.Add(_button);
            Widget = box;
        }
    }

    public class CheckSpin2DButton
    {
        private readonly Gtk.CheckButton _button;
        private readonly Gtk.SpinButton _button1;
        private readonly Gtk.SpinButton _button2;
        public Widget Widget { get; }
        public bool Value
        {
            get => _button.Active;
            set => _button.Active = value;
        }

        public double X
        {
            get => _button1.Value;
            set => _button1.Value = value;
        }

        public double Y
        {
            get => _button2.Value;
            set => _button2.Value = value;
        }

        public CheckSpin2DButton(MyWindow window, string labelCheck, bool check, string label1, string label2, double x = 0, double y = 0,
                                    double min1 = -1000, double max1 = 1000, double step1 = 1, double min2 = -1000, double max2 = 1000, double step2 = 1)
        {
            var box = new HBox { new Label(labelCheck) };
            _button = new Gtk.CheckButton { Active = check };
            _button.Clicked += (_,_) => window.RequestRedraw();
            box.Add(_button);

            box.Add(new Label(label1));
            _button1 = new Gtk.SpinButton(min1, max1, step1) { Digits = 3, Value = x };
            _button1.Halign = Align.Start;
            _button1.Margin = 10;
            _button1.ValueChanged += (_,_) => { if (_button.Active) window.RequestRedraw(); };
            box.Add(_button1);

            box.Add(new Label(label2));
            _button2 = new Gtk.SpinButton(min2, max2, step2) { Digits = 3, Value = y };
            _button2.Halign = Align.Start;
            _button2.Margin = 10;
            _button2.ValueChanged += (_,_) => { if (_button.Active) window.RequestRedraw(); };
            box.Add(_button2);

            Widget = box;
        }
    }

    public class Line2d
    {
        private readonly List<Vector2d> _list;
        public int Size => _list.Count;
        
        public Vector2d this[int i]
        {
            get => _list[i];
            set => _list[i] = value;
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public Line2d(Vector2d r, Vector2d l)
        {
            _list = new List<Vector2d> { r, l };
        }

        public Line2d(Vector2d[] l)
        {
            this._list = new List<Vector2d> { l[0], l[1] };
        }
        
    }

    public class Line4d
    {
        private readonly List<Vector4d> _list;
        public int Size => _list.Count;
        
        public Vector4d this[int i]
        {
            get => _list[i];
            set => _list[i] = value;
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public Line4d(Vector4d r, Vector4d l)
        {
            _list = new List<Vector4d> { r, l };
        }

        public Line4d(Vector4d[] l)
        {
            _list = new List<Vector4d> { l[0], l[1] };
        }

        public void Modify(Matrix4d m)
        {
            for (int i = 0; i < _list.Count; ++i)
            {
                _list[i] = m * _list[i];
            }
        }

        public Line4d New_Modify(Matrix4d m)
        {
            Line4d l = new(this[0], this[1]);
            l.Modify(m);
            return l;
        }

        public double Min_Y()
        {
            double min = _list[0].Y;

            for (int i = 0; i < Size; ++i)
            {
                if (this[i].Y < min)
                {
                    min = this[i].Y;
                }
            }

            return min;
        }

        public List<Line2d> To_Line_2d(double dx, double dy)
        {
            List<Line2d> rez = new();

            for (int i = 0; i < Size; ++i)
            {
                int j = (i + 1) % Size;
                Line2d v = new(new Vector2d(this[i].X + dx, this[i].Y + dy), new Vector2d(this[j].X + dx, this[j].Y + dy));
                rez.Add(v);
            }

            return rez;
        }
    }

    public class Polygon3Vec4D 
    {
        private readonly List<Vector4d> _list;
        public int Size => _list.Count;
        
        public Vector4d this[int i]
        {
            get => _list[i];
            set => _list[i] = value;
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public Polygon3Vec4D(Vector4d r, Vector4d l, Vector4d up)
        {
            _list = new List<Vector4d> { r, l, up };
        }

        public Polygon3Vec4D(Vector4d[] l)
        {
            _list = new List<Vector4d> { l[0], l[1], l[2] };
        }

        public void Modify(Matrix4d m)
        {
            for (int i = 0; i < _list.Count; ++i)
            {
                _list[i] = m * _list[i];
            }
        }

        public Polygon3Vec4D New_Modify(Matrix4d m)
        {
            Polygon3Vec4D p = new(this[0], this[1], this[2]);
            p.Modify(m);
            return p;
        }

        public double Min_Y()
        {
            double min = _list[0].Y;

            for (int i = 0; i < Size; ++i)
            {
                if (this[i].Y < min)
                {
                    min = this[i].Y;
                }
            }

            return min;
        }

        public List<Line2d> To_Line_2d(double dx, double dy)
        {
            List<Line2d> rez = new();

            for (int i = 0; i < Size; ++i)
            {
                int j = (i + 1) % Size;
                Line2d v = new(new Vector2d(this[i].X + dx, this[i].Y + dy), new Vector2d(this[j].X + dx, this[j].Y + dy));
                rez.Add(v);
            }

            return rez;
        }
    }

    public class Polygon4Vec4d
    {
        private readonly List<Vector4d> _list;
        public int Size => _list.Count;
        
        public Vector4d this[int i]
        {
            get => _list[i];
            set => _list[i] = value;
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public Polygon4Vec4d()
        {
            _list = new List<Vector4d>();
        }

        public Polygon4Vec4d(Vector4d dr, Vector4d dl, Vector4d ul, Vector4d ur)
        {
            _list = new List<Vector4d> { dr, dl, ul, ur };
        }

        public Polygon4Vec4d(Vector4d[] l)
        {
            _list = new List<Vector4d> { l[0], l[1], l[2], l[3] };
        }

        public void Add(Vector4d a)
        {
            if (Size < 4) _list.Add(new Vector4d(a));
        }

        public void Modify(Matrix4d m)
        {
            for (int i = 0; i < Size; ++i)
            {
                this[i] = m * this[i];
            }
        }

        public Polygon4Vec4d New_Modify(Matrix4d m)
        {
            Polygon4Vec4d p = new(this[0], this[1], this[2], this[3]);
            p.Modify(m);
            return p;
        }

        public double Min_Y()
        {
            double min = _list[0].Y;

            for (int i = 0; i < Size; ++i)
            {
                if (this[i].Y < min)
                {
                    min = this[i].Y;
                }
            }

            return min;
        }

        public List<Line2d> To_Line_2d(double dx, double dy)
        {
            List<Line2d> rez = new();

            for (int i = 0; i < Size; ++i)
            {
                int j = (i + 1) % Size;
                Line2d v = new(new Vector2d(this[i].X + dx, this[i].Y + dy), new Vector2d(this[j].X + dx, this[j].Y + dy));
                rez.Add(v);
            }

            return rez;
        }

    }



}






