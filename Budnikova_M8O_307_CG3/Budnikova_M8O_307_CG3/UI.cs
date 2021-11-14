using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using GLib;
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
            var box = new HBox {new Label("  ")};
            box.Add(new Label(label));
            _button = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = value };
            _button.Halign = Align.Start;
            _button.Margin = 1;
            _button.ValueChanged += (_, _) => window.RequestRedraw();

            box.Add(_button);
            Widget = box;
            Widget.Halign = Align.Start;
        }
    }

    
    public class SpinButtons
    {
        private readonly List<Gtk.SpinButton> _buttons;
        private readonly string[] _names;
        
        public Widget Widget { get; }
        public double this[string s]
        {
            get
            {
                for (int i = 0; i < _buttons.Count; ++i)
                {
                    if (_names[i] == s)
                    {
                        return _buttons[i].Value;
                    }
                }

                throw new ArgumentException("UI.cs : SpinButtons | this[string s] | No s found");
            }
            set
            {
                for (int i = 0; i < _buttons.Count; ++i)
                {
                    if (_names[i] == s)
                    {
                        _buttons[i].Value = value;
                    }
                }
            }
        }

        
        public SpinButtons(MyWindow window, string label, string[] names, double[] values, double[] min, double[] max, double[] steps)
        {
            if (names.Length != values.Length || values.Length != min.Length ||
                min.Length != max.Length || max.Length != steps.Length) 
                throw new ArgumentException("UI.cs : SpinButtons | Constructor | Argument length mismatch");

            _buttons = new List<Gtk.SpinButton>();
            var box = new HBox { new Label(label) };
        
            _names = names;
            for (int i = 0; i < names.Length; ++i)
            {
                var btn = new Gtk.SpinButton(min[i], max[i], steps[i]) { Digits = 3, Value = values[i] };
                btn.Halign = Align.End;
                btn.Margin = 1;
                
                btn.ValueChanged += (_, _) => window.RequestRedraw();

                _buttons.Add(btn);
            }

            for (int i = 0; i < _buttons.Count; ++i)
            {
                box.Add(new Label("  "));
                box.Add(new Label(names[i]));
                box.Add(new Label("  "));
                box.Add(_buttons[i]);
            }
            
            Widget = box;
            Widget.Halign = Align.Center;
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
        public Vector2d ValueVec2d
        {
            get => new Vector2d(_button1.Value, _button2.Value);
            set
            {
                _button1.Value = value.X;
                _button2.Value = value.Y;
            }
        }
        public Vector3d ValueVec3d => new Vector3d(_button1.Value, _button2.Value, _button3.Value);
        public Vector4d ValueVec4d => new Vector4d(_button1.Value, _button2.Value, _button3.Value, 1);


        public Spin3DButton(MyWindow window, string label, double x = 0, double y = 0, double z = 0, double min = -1000, double max = 1000, double step = 1)
        {
            var box = new HBox {new Label("  ")};
            box.Add(new Label(label));
            box.Add(new Label("  "));
            
            _button1 = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = x };
            _button1.Halign = Align.Start;
            _button1.Margin = 1;
            _button1.ValueChanged += (_,_) => window.RequestRedraw();

            _button2 = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = y };
            _button2.Halign = Align.Start;
            _button2.Margin = 1;
            _button2.ValueChanged += (_,_) => window.RequestRedraw();

            _button3 = new Gtk.SpinButton(min, max, step) { Digits = 3, Value = z };
            _button3.Halign = Align.Start;
            _button3.Margin = 1;
            _button3.ValueChanged += (_,_) => window.RequestRedraw();
            
            box.Add(_button1);
            box.Add(_button2);
            box.Add(_button3);
            Widget = box;
            Widget.Halign = Align.Center;
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
            var box = new HBox {new Label("  ")};
            box.Add(new Label(label));
            box.Add(new Label("  "));
            
            _button = new Gtk.CheckButton { Active = check };
            _button.Margin = 1;
            _button.Clicked += (_,_) => window.RequestRedraw();

            box.Add(_button);
            Widget = box;
            Widget.Halign = Align.Center;
        }
    }
    
    
    public class CheckButtons
    {
        private readonly List<Gtk.CheckButton> _buttons;
        
        public Widget Widget { get; }
        public bool this[string s]
        {
            get
            {
                foreach (var b in _buttons)
                {
                    if (b.Label == s)
                    {
                        return b.Active;
                    }
                }

                throw new ArgumentException("UI.cs : CheckButtons | this[string s] | No s found");
            }
            set
            {
                foreach (var b in _buttons)
                {
                    if (b.Label == s)
                    {
                        b.Active = value;
                    }
                }
            }
        }

        
        public CheckButtons(MyWindow window, string label, string[] names, bool[] check)
        {
            if (names.Length != check.Length) throw new ArgumentException("UI.cs : CheckButtons | Constructor | Argument length mismatch");

            _buttons = new List<Gtk.CheckButton>();
            var box = new HBox { new Label(label) };

            for (int i = 0; i < names.Length; ++i)
            {
                var btn = new Gtk.CheckButton { Active = check[i] };
                btn = new Gtk.CheckButton {Active = check[i]};
                btn.Label = names[i];
                btn.Clicked += (_,_) => window.RequestRedraw();
                _buttons.Add(btn);
            }

            foreach (var b in _buttons)
            {   
                box.Add(new Label("  "));
                box.Add(b);;
                box.Add(new Label("   "));
            }
            
            Widget = box;
            Widget.Halign = Align.Center;
        }
    }
    

    public class CheckSpin2DButton
    {
        private readonly Gtk.CheckButton _buttonCheck;
        private readonly Gtk.SpinButton _button1;
        private readonly Gtk.SpinButton _button2;
        
        public Widget Widget { get; }
        public bool Value
        {
            get => _buttonCheck.Active;
            set => _buttonCheck.Active = value;
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
            var box = new HBox {new Label("  ")};
            box.Add(new Label(labelCheck));
            box.Add(new Label("   "));
            
            _buttonCheck = new Gtk.CheckButton { Active = check };
            _buttonCheck.Clicked += (_,_) => window.RequestRedraw();
            box.Add(_buttonCheck);
            
            box.Add(new Label("  "));

            box.Add(new Label(label1));
            _button1 = new Gtk.SpinButton(min1, max1, step1) { Digits = 3, Value = x };
            _button1.Halign = Align.Start;
            _button1.Margin = 1;
            _button1.ValueChanged += (_,_) => { if (_buttonCheck.Active) window.RequestRedraw(); };
            box.Add(_button1);
            
            box.Add(new Label("  "));

            box.Add(new Label(label2));
            _button2 = new Gtk.SpinButton(min2, max2, step2) { Digits = 3, Value = y };
            _button2.Halign = Align.Start;
            _button2.Margin = 1;
            _button2.ValueChanged += (_,_) => { if (_buttonCheck.Active) window.RequestRedraw(); };
            box.Add(_button2);

            Widget = box;
            Widget.Halign = Align.Center;
        }
    }

    
    public class ChoiceButton
    {
        private readonly AppChooserButton _button;
        
        public Widget Widget { get; }
        public string Value
        {
            get
            {
                if (_button.Accessible.Name != null)
                {
                    return _button.Accessible.Name;
                }
 
                return "";
            }
        }
        
        public ChoiceButton(MyWindow window, string label, string[] labels)
        {
            var box = new HBox { new Label("  ")};
            box.Add(new Label(label));
            box.Add(new Label("  "));
            _button = new AppChooserButton("choice");
            
            var icon = new ThemedIcon("");
            foreach (var l in labels)
            {
                _button.AppendSeparator();
                _button.AppendCustomItem(l, l, icon);
            }

            _button.ActiveCustomItem = labels[0];
            
            
            _button.Halign = Align.Start;
            _button.Margin = 1;
            _button.Changed += (_, _) => window.RequestRedraw();

            box.Add(_button);
            Widget = box;
            Widget.Halign = Align.Start;
        }
    }
}






