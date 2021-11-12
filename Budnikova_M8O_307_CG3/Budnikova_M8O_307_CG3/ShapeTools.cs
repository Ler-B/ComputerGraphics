using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace Budnikova_M8O_307_CG3
{ 
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

        public Vector4d Norm
        {
            get
            {
                Vector4d vec1 = this[3] - this[0];
                Vector4d vec2 = this[1] - this[0];

                if ((vec1 * vec2).Abs() == 0)
                {
                    if (Vector4d.Equal(this[1], this[0]))
                    {
                        vec2 = this[2] - this[0];
                    }
                    else
                    {
                        vec1 = this[2] - this[0];
                    }
                    
                }
                
                Vector4d norm = vec1 * vec2 / (vec1 * vec2).Abs();

                return norm;
            }
        }
        
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
        
        public Polygon4Vec4d(Polygon4Vec4d p)
        {
            _list = new List<Vector4d> { p[0], p[1], p[2], p[3] };
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

        // public int CompareTo(object obj)
        // {
        //     Polygon4Vec4d polygon = obj as Polygon4Vec4d;
        //
        //     double minThis = this[0].Y;
        //     double minP = polygon[0].Y;
        //
        //     foreach (Vector4d v in this)
        //     {
        //         minThis = v.Y < minThis ? v.Y : minThis;
        //     }
        //     
        //     foreach (Vector4d v in polygon)
        //     {
        //         minThis = v.Y < minThis ? v.Y : minThis;
        //     }
        //
        //     return minP < minThis ? 1 : -1;
        // }

    }
}