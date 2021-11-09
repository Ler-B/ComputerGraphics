using System;
using System.Linq;
using System.Collections.Generic;

//using Line4d = System.Collections.Generic.List<Budnikova_M8O_307_CG2.Vector4d>;
using Line2d = System.Collections.Generic.List<Budnikova_M8O_307_CG2.Vector2d>;


namespace Budnikova_M8O_307_CG2
{
    public class Sq
    {
        private readonly double _len1;
        private readonly double _len2;
        private readonly double _len3;

        private readonly Vector4d _center,
                         _x_axis,
                         _y_axis,
                         _z_axis;
        private readonly Polygon4_4d _face;
        private readonly Polygon4_4d _back;
        private readonly Polygon4_4d _up, _down;
        private readonly Polygon4_4d _right, _left;

        private readonly List<Polygon4_4d> _points;
        private readonly List<Line4d> _axis;

        public Sq(double l1, double l2, double l3, double x = 0, double y = 0, double z = 0)
        {
            _len1 = l1;
            _len2 = l2;
            _len3 = l3;

            Vector4d _back_up_left = new(x - _len1 / 2, y + _len2 / 2, z + _len3 / 2, 1);
            Vector4d _back_up_right = new(x + _len1 / 2, y + _len2 / 2, z + _len3 / 2, 1);

            Vector4d _back_down_left = new(x - _len1 / 2, y + _len2 / 2, z - _len3 / 2, 1);
            Vector4d _back_down_right = new(x + _len1 / 2, y + _len2 / 2, z - _len3 / 2, 1);

            Vector4d _face_up_left = new(x - _len1 / 2, y - _len2 / 2, z + _len3 / 2, 1);
            Vector4d _face_up_right = new(x + _len1 / 2, y - _len2 / 2, z + _len3 / 2, 1);

            Vector4d _face_down_left = new(x - _len1 / 2, y - _len2 / 2, z - _len3 / 2, 1);
            Vector4d _face_down_right = new(x + _len1 / 2, y - _len2 / 2, z - _len3 / 2, 1);

            _face = new Polygon4_4d(_face_down_left, _face_down_right, _face_up_right, _face_up_left);
            _right = new Polygon4_4d(_face_down_right, _back_down_right, _back_up_right, _face_up_right);
            _back = new Polygon4_4d(_back_down_right, _back_down_left, _back_up_left, _back_up_right);
            _left = new Polygon4_4d(_back_down_left, _face_down_left, _face_up_left, _back_up_left);
            _down = new Polygon4_4d(_face_down_left, _back_down_left, _back_down_right, _face_down_right);
            _up = new Polygon4_4d(_face_up_right, _back_up_right, _back_up_left, _face_up_left);


            double len = new double[] { _len1, _len2, _len3 }.Min();
            _center = new Vector4d(x, y, z, 1);
            _x_axis = new Vector4d(x + len, y, z, 1);
            _y_axis = new Vector4d(x, y - len, z, 1);
            _z_axis = new Vector4d(x, y, z - len, 1);

            _points = new List<Polygon4_4d> { _face, _right, _back, _left, _up, _down };
            _axis = Get_Axis();
        }

        public List<Line4d> Get_Axis()
        {
            Line4d x = new(_center, _x_axis);
            Line4d y = new(_center, _y_axis);
            Line4d z = new(_center, _z_axis);

            return new List<Line4d> { x, y, z };
        }

        public void Rotation_XYZ(double angle_x, double angle_y, double angle_z)
        {
            foreach (Polygon4_4d p in _points)
            {
                p.Modify(Matrix4d.Rot_XYZ(angle_x, angle_y, angle_z));
            }

            foreach (Line4d p in _axis)
            {
                p.Modify(Matrix4d.Rot_XYZ(angle_x, angle_y, angle_z));
            }
        }

        public void Scale_XYZ(double scale_x, double scale_y, double scale_z)
        {
            foreach (Polygon4_4d p in _points)
            {
                p.Modify(Matrix4d.Scale_XYZ(scale_x, scale_y, scale_z));
            }
        }

        public void Move_XYZ(double dx, double dy, double dz)
        {
            foreach (Polygon4_4d p in _points)
            {
                p.Modify(Matrix4d.Move_XYZ(dx, dy, dz));
            }
        }

        public List<Line2d> Get_XYZ_Points(double dx = 0, double dy = 0)
        {
            List<Line2d> rez_list = new() { };

            double min_y = _points[0][0].Y;

            foreach (Polygon4_4d p in _points)
            {
                if (new double[] { p[0].Y, p[1].Y, p[2].Y, p[3].Y }.Min() < min_y)
                {
                    min_y = new double[] { p[0].Y, p[1].Y, p[2].Y, p[3].Y }.Min();
                }
            }


            foreach (Polygon4_4d p in _points)
            {
                if (Math.Min(p[0].Y, p[1].Y) != min_y)
                {

                    rez_list.Add(Get_2d_line(p[0], p[1], dx, dy));
                }

                if (Math.Min(p[1].Y, p[2].Y) != min_y)
                {
                    rez_list.Add(Get_2d_line(p[1], p[2], dx, dy));
                }

                if (Math.Min(p[2].Y, p[3].Y) != min_y)
                {
                    rez_list.Add(Get_2d_line(p[2], p[3], dx, dy));
                }

                if (Math.Min(p[3].Y, p[0].Y) != min_y)
                {
                    rez_list.Add(Get_2d_line(p[3], p[0], dx, dy));
                }
            }

            return rez_list;
        }

        static Line2d Get_2d_line(Vector4d p1, Vector4d p2, double dx = 0, double dy = 0)
        {
            double x1 = p1.X + dx;
            double y1 = p1.Z + dy;

            double x2 = p2.X + dx;
            double y2 = p2.Z + dy;
            return new Line2d { new Vector2d(x1, y1), new Vector2d(x2, y2) };
        }

        public List<Line2d> Projection(double dx, double dy, Matrix4d m)
        {
            List<Line2d> rez_list = new() { };

            List<Polygon4_4d> new_points = new() { };

            foreach (Polygon4_4d p in _points)
            {
                new_points.Add(p.New_Modify(m));
            }

            foreach (Polygon4_4d p in new_points)
            {
                rez_list.InsertRange(rez_list.Count, p.To_Line_2d(dx, dy));
            }

            return rez_list;
        }

        public List<Line2d> Get_XY_Points(double dx = 0, double dy = 0)
        {
            return Projection(dx, dy, Matrix4d.Proection_XY());
        }

        public List<Line2d> Get_XZ_Points(double dx = 0, double dy = 0)
        {
            return Projection(dx, dy, Matrix4d.Proection_XZ());
        }

        public List<Line2d> Get_YZ_Points(double dx = 0, double dy = 0)
        {
            return Projection(dx, dy, Matrix4d.Proection_YZ());
        }

        public List<Line2d> Get_XYZ_Axis(double dx = 0, double dy = 0)
        {
            List<Line2d> rez_list = new() { };

            foreach (Line4d p in _axis)
            {
                Line2d new_line = new() { new Vector2d(p[0].X + dx, p[0].Z + dy), new Vector2d(p[1].X + dx, p[1].Z + dy) };
                rez_list.Add(new_line);
            }

            return rez_list;
        }

        public List<Line2d> Get_XYZ_Normals(double dx = 0, double dy = 0)
        {
            List<Line2d> rez_list = new() { };

            double min_y = _points[0][0].Y;

            foreach (Polygon4_4d p in _points)
            {
                if (p.Min_Y() < min_y)
                {
                    min_y = p.Min_Y();
                }
            }

            foreach (Polygon4_4d p in _points)
            {
                if (p.Min_Y() != min_y)
                {
                    Vector4d vec1 = p[1] - p[0];
                    Vector4d vec2 = p[3] - p[0];
                    //Vector4d sum = vec1 + vec2;

                    Vector4d norm = vec1 * vec2 / (vec1 * vec2).Abs();

                    //double l1 = ((p[0] - _center) * sum).Abs() / sum.Abs();
                    double l2 = new double[] { _len1, _len2, _len3 }.Min() / 2;


                    Vector4d p1 = (p[0] + p[1] + p[2] + p[3]) / 4;
                    Vector4d p2 = p1 + norm * l2;

                    Line2d new_line = new() { new Vector2d(p1.X + dx, p1.Z + dy), new Vector2d(p2.X + dx, p2.Z + dy) };
                    rez_list.Add(new_line);
                }
            }

            return rez_list;
        }

        public List<Line2d> Get_Izometric(double dx = 0, double dy = 0)
        {
            List<Line2d> rez_list = new() { };

            List<Polygon4_4d> i_list = new() { };

            foreach (Polygon4_4d p in _points)
            {
                i_list.Add(p.New_Modify(Matrix4d.Izometric()));
            }

            foreach (Polygon4_4d p in i_list)
            {
                rez_list = p.To_Line_2d(dx, dy);
            }

            return rez_list;
        }

    }

}



