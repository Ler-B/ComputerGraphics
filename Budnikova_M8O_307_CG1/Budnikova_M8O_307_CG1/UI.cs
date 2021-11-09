using System;
using Gtk;

namespace Budnikova_M8O_307_CG1
{
    public class Spin_Button
    {
        private HBox _box;
        private Widget _widget;
        private SpinButton _button;

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
            set
            {
                _widget = value;
            }
        }


        public Spin_Button(MyWindow window, string label, double value = 0, double min = -1000, double max = 1000, double step = 1)
        {
            _box = new HBox();
            _box.Add(new Label(label));
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
        private HBox _box;
        private Widget _widget;
        private SpinButton _button1;
        private SpinButton _button2;

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

        public Point Value
        {
            get
            {
                return new Point(_button1.Value, _button2.Value);
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
            set
            {
                _widget = value;
            }
        }

        public Spin_2d_Button(MyWindow window, string label, double value1 = 0, double value2 = 0, double min = -1000, double max = 1000, double step = 1)
        {
            _box = new HBox();
            _box.Add(new Label(label));

            _button1 = new SpinButton(min, max, step) { Digits = 3, Value = value1 };
            _button1.Halign = Align.Start;
            _button1.Margin = 10;
            _button1.ValueChanged += (o, args) => window.RequestRedraw();

            _button2 = new SpinButton(min, max, step) { Digits = 3, Value = value2 };
            _button2.Halign = Align.Start;
            _button2.Margin = 10;
            _button2.ValueChanged += (o, args) => window.RequestRedraw();

            _box.Add(_button1);
            _box.Add(_button2);
            _widget = _box;
        }

    }

    public class Point {
        public double X;
        public double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point operator+(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y); ;
        }
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y); ;
        }
        public static Point operator /(Point a, double b)
        {
            return new Point(a.X / b, a.Y / b);
        }
    }


}